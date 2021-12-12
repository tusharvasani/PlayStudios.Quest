using PSQuest.Core.Common;
using PSQuest.Core.Services;
using PSQuest.Data;
using PSQuest.Data.Entities;
using PSQuest.Data.Models;
using PSQuest.Data.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Core.Services
{
    public class QuestService : IQuestService, IDisposable
    {
        private readonly QuestDbContext _myDbContext;
        private readonly IQuestConfigService _questConfigService;

        /// <summary>
        /// Constructor injecting Database Context and Quest Configuration Service
        /// </summary>
        /// <param name="context"></param>
        /// <param name="questConfigService"></param>
        public QuestService(QuestDbContext context, IQuestConfigService questConfigService)
        {
            _myDbContext = context ?? throw new ArgumentNullException(nameof(context));
            _questConfigService = questConfigService ?? throw new ArgumentNullException(nameof(questConfigService));
        }

        /// <summary>
        /// This method is responsible to compute the Quest Progress of a Player
        /// The API request object is passed in the parameter
        /// This method will return the Dto to the Client with computed Quest Progress and Milestones completed with this bet
        /// </summary>
        /// <param name="questProgressRequest"></param>
        /// <returns></returns>
        public Task<QuestProgressResponse> ComputeQuestProgress(QuestProgressRequest questProgressRequest)
        {
            // Get Active Quest configurations
            QuestConfig activeQuest = _questConfigService.GetLoadedActiveQuestConfig();
            if (activeQuest == null)
                throw new ArgumentNullException("No Active Quest available", nameof(activeQuest));

            return Task.FromResult(GetQuestMilestonesCompleted(activeQuest, questProgressRequest));
        }

        /// <summary>
        /// This method computes the Milestones completed with the current Bet
        /// </summary>
        /// <param name="questConfig"></param>
        /// <param name="request"></param>
        /// <returns>QuestProgressResponse with QuestPointsEarned, TotalQuestPercentCompleted and a list of completed Milestones</returns>
        private QuestProgressResponse GetQuestMilestonesCompleted(QuestConfig questConfig, QuestProgressRequest request)
        {
            QuestProgressResponse response = new QuestProgressResponse();
            List<Milestone> completedMilestones = new List<Milestone>();

            int questPointsEarned = GetCurrentBetQuestPointsEarned(request.ChipBetAmount, questConfig.RateFromBet, request.PlayerLevel, questConfig.LevelBonusRate);
            int lastMilestoneIndex = 0;

            // Get last completed Milestone for Player for a Quest
            PlayerQuestState playerQuestState = GetPlayerQuestState(request.PlayerId, questConfig.QuestId).Result;
            if (playerQuestState != null)
            {
                if (playerQuestState.LastMilestoneIndexCompleted == questConfig.TotalMilestones && playerQuestState.TotalQuestPercentCompleted == 100)
                    throw new Exception("This player has already finished all the milestones of this quest");
                else
                    lastMilestoneIndex = playerQuestState.LastMilestoneIndexCompleted;
            }
            else
            {
                playerQuestState = new PlayerQuestState();
                playerQuestState.PlayerId = request.PlayerId;
                playerQuestState.QuestId = questConfig.QuestId;
            }

            int dbTotalPointsEarned = GetTotalPointsEarnedByPlayer(questConfig.QuestId, request.PlayerId);

            // Adding the last milestone index in completedMilestones when a player has already finished all the milestones in a quest
            if (dbTotalPointsEarned >= questConfig.RequiredPoints)
                completedMilestones.Add(new Milestone() { MilestoneIndex = lastMilestoneIndex, ChipsAwarded = 0 });
            else
            {
                int totalPointsWithCurrBet = dbTotalPointsEarned + questPointsEarned;
                int newEarnedMilestoneIndex = 0;
                for (int cnt = 0; cnt < questConfig.Milestones.Count; cnt++)
                {
                    if (totalPointsWithCurrBet >= questConfig.Milestones[cnt].PointsToCompleteMilestone)
                    {
                        totalPointsWithCurrBet -= questConfig.Milestones[cnt].PointsToCompleteMilestone;
                        if (cnt == questConfig.Milestones.Count - 1)
                            newEarnedMilestoneIndex = cnt + 1;
                    }
                    else
                    {
                        newEarnedMilestoneIndex = cnt;
                        break;
                    }
                }

                // Add the last milestone index when a player could not complete a milestone in the current bet.
                if (lastMilestoneIndex == newEarnedMilestoneIndex)
                    completedMilestones.Add(new Milestone() { MilestoneIndex = lastMilestoneIndex, ChipsAwarded = 0 });
                else
                {
                    for (int cnt = lastMilestoneIndex; cnt < newEarnedMilestoneIndex; cnt++)
                    {
                        completedMilestones.Add(new Milestone() { MilestoneIndex = questConfig.Milestones[cnt].MilestoneIndex, ChipsAwarded = questConfig.Milestones[cnt].ChipsAwarded });
                    }
                }
            }

            // Insert record in PlayerQuestProgress
            PlayerQuestProgress questProgress = new PlayerQuestProgress();
            questProgress.ChipAmountBet = request.ChipBetAmount;
            questProgress.PlayerId = request.PlayerId;
            questProgress.PlayerLevel = request.PlayerLevel;
            questProgress.QuestId = questConfig.QuestId;
            questProgress.QuestPointsEarned = questPointsEarned;
            questProgress.InsertDateTime = DateTime.Now;

            if (!Convert.ToBoolean(InsertPlayerQuestProgress(questProgress)))
                throw new Exception("Error in inserting Player Quest Progress record");

            // Compute total percentage of quest completed
            decimal totalQuestPercentCompleted = ComputeTotalQuestPercentCompleted((dbTotalPointsEarned + questPointsEarned), questConfig.RequiredPoints);
            
            //int TotalPointsEarnedWithCurrBet = DbTotalPointsEarned + QuestPointsEarned;
            //if (TotalPointsEarnedWithCurrBet >= questConfig.RequiredPoints)
            //    TotalQuestPercentCompleted = 100;
            //else
            //    TotalQuestPercentCompleted = Math.Round(((decimal)TotalPointsEarnedWithCurrBet / questConfig.RequiredPoints) * 100);
            
            // Update PlayerQuestState Persistence
            playerQuestState.LastMilestoneIndexCompleted = (completedMilestones.Count > 0 ? completedMilestones.Max(m => m.MilestoneIndex) : lastMilestoneIndex);
            playerQuestState.TotalQuestPercentCompleted = totalQuestPercentCompleted;
            playerQuestState.DateUpdated = DateTime.Now;
            if (!Convert.ToBoolean(PersistPlayerQuestState(playerQuestState)))
                throw new Exception("Error while updating PlayerQuestState Persistence");

            // Compute quest percentage completed
            response.QuestPointsEarned = questPointsEarned;
            response.MilestonesCompleted = completedMilestones;
            response.TotalQuestPercentCompleted = totalQuestPercentCompleted;

            return response;
        }

        /// <summary>
        /// This method computes the QuestPercent completed with current bet
        /// </summary>
        /// <param name="totalPointsEarned"></param>
        /// <param name="requiredPointsForQuest"></param>
        /// <returns>decimal value with total quest percent completed</returns>
        private decimal ComputeTotalQuestPercentCompleted(int totalPointsEarned, int requiredPointsForQuest)
        {
            if (totalPointsEarned >= requiredPointsForQuest)
                return 100;
            else
                return Math.Round(((decimal)totalPointsEarned / requiredPointsForQuest) * 100);
        }
        
        /// <summary>
        /// This method fetches the total of already earned points by a player for a quest 
        /// </summary>
        /// <param name="questId"></param>
        /// <param name="playerId"></param>
        /// <returns>Int value with already earned points by a player for a particular quest</returns>
        private int GetTotalPointsEarnedByPlayer(string questId, string playerId)
        {
            return Convert.ToInt32(_myDbContext.PlayerQuestProgress.Where(q => q.QuestId == questId && q.PlayerId == playerId).Sum(r => r.QuestPointsEarned));
        }

        /// <summary>
        /// This method computes the TotalPoints earned by a player for current Bet
        /// </summary>
        /// <param name="chipBetAmount"></param>
        /// <param name="rateFromBet"></param>
        /// <param name="playerLevel"></param>
        /// <param name="levelBonusRate"></param>
        /// <returns>Int value with totalPointsEarned by player for a current bet</returns>
        private int GetCurrentBetQuestPointsEarned(int chipBetAmount, int rateFromBet, int playerLevel, int levelBonusRate)
        {
            return (chipBetAmount * rateFromBet) + (playerLevel * levelBonusRate);
        }

        /// <summary>
        /// This method inserts a record in PlayerQuestProgress table to store the request params and the totalPointsEarned for a quest
        /// </summary>
        /// <param name="questProgress"></param>
        /// <returns>Boolean. True if Insert successfull, false otherwise</returns>
        public bool InsertPlayerQuestProgress(PlayerQuestProgress questProgress)
        {
            try
            {
                _myDbContext.PlayerQuestProgress.Add(questProgress);
                return (_myDbContext.SaveChanges() >= 0);
            }
            catch (Exception)
            {
                throw new Exception("Error in inserting Player Quest Progress");
            }
        }
        
        /// <summary>
        /// This method is responsible to fetch the current state of a quest played by a player.
        /// It also serves the response to the Controller for api/state endpoint
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="questId"></param>
        /// <returns>PlayerQuestState object retrieved from database</returns>
        public Task<PlayerQuestState> GetPlayerQuestState(string playerId, string questId)
        {
            if (string.IsNullOrEmpty(questId))
            {
                questId = _questConfigService.GetLoadedActiveQuestConfig().QuestId;
            }
            return Task.FromResult(_myDbContext.PlayerQuestState.Where(state => state.PlayerId == playerId && state.QuestId == questId).FirstOrDefault());
        }

        /// <summary>
        /// Updating the Player's QuestState
        /// </summary>
        /// <param name="questState"></param>
        /// <returns>Boolean. True is saving to PlayerQuestState is successful, false otherwise</returns>
        public bool PersistPlayerQuestState(PlayerQuestState questState)
        {
            try
            {
                if (_myDbContext.PlayerQuestState.Any(q => q.PlayerId == questState.PlayerId && q.QuestId == questState.QuestId))
                    _myDbContext.Update(questState);
                else
                    _myDbContext.Add(questState);

                return (_myDbContext.SaveChanges() >= 0);
            }
            catch (Exception ex)
            {
                throw new Exception("Error on getting PlayerQuest State" + Environment.NewLine + ex.Message);
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
