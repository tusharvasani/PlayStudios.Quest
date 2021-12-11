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
        private readonly QuestDbContext _context;
        private readonly IQuestConfigService _questConfigService;
        
        public QuestService(QuestDbContext context, IQuestConfigService questConfigService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _questConfigService = questConfigService ?? throw new ArgumentNullException(nameof(questConfigService));
        }

        public Task<QuestProgressResponse> ComputeQuestProgress(QuestProgressRequest questProgressRequest)
        {
            // Get Active Quest configurations
            QuestConfig activeQuest = _questConfigService.GetLoadedActiveQuestConfig();
            if (activeQuest == null)
                throw new ArgumentNullException("No Active Quest available", nameof(activeQuest));

            return Task.FromResult(GetQuestMilestonesCompleted(activeQuest, questProgressRequest));
        }

        private QuestProgressResponse GetQuestMilestonesCompleted(QuestConfig questConfig, QuestProgressRequest request)
        {
            QuestProgressResponse response = new QuestProgressResponse();
            List<Milestone> completedMilestones = new List<Milestone>();
            int questPointsEarned = (request.ChipBetAmount * questConfig.RateFromBet) + (request.PlayerLevel * questConfig.LevelBonusRate);
            int LastMilestoneIndex = 0;

            // Get last completed Milestone for Player for a Quest
            PlayerQuestState playerQuestState = GetPlayerQuestState(request.PlayerId, questConfig.QuestId).Result;
            if (playerQuestState != null)
            {
                if (playerQuestState.LastMilestoneIndexCompleted == questConfig.TotalMilestones && playerQuestState.TotalQuestPercentCompleted == 100)
                    throw new Exception("This player has already finished all the milestones of this quest");
                else
                    LastMilestoneIndex = playerQuestState.LastMilestoneIndexCompleted;
            }
            else
            {
                playerQuestState = new PlayerQuestState();
                playerQuestState.PlayerId = request.PlayerId;
                playerQuestState.QuestId = questConfig.QuestId;
            }

            int DbTotalPointsEarned = Convert.ToInt32(_context.PlayerQuestProgress.Where(q => q.QuestId == questConfig.QuestId && q.PlayerId == request.PlayerId).Sum(r => r.QuestPointsEarned));
            if (DbTotalPointsEarned >= questConfig.RequiredPoints)
                completedMilestones.Add(new Milestone() { MilestoneIndex = LastMilestoneIndex, ChipsAwarded = 0 });
            else
            {
                int TotalPointsWithCurrBet = DbTotalPointsEarned + questPointsEarned;
                int NewEarnedMilestoneIndex = 0;
                for (int cnt = 0; cnt < questConfig.Milestones.Count; cnt++)
                {
                    if (TotalPointsWithCurrBet >= questConfig.Milestones[cnt].PointsToCompleteMilestone)
                    {
                        TotalPointsWithCurrBet -= questConfig.Milestones[cnt].PointsToCompleteMilestone;
                        if (cnt == questConfig.Milestones.Count -1)
                            NewEarnedMilestoneIndex = cnt + 1;
                    }
                    else
                    {
                        NewEarnedMilestoneIndex = cnt;
                        break;
                    }
                }

                if (LastMilestoneIndex == NewEarnedMilestoneIndex)
                    completedMilestones.Add(new Milestone() { MilestoneIndex = LastMilestoneIndex, ChipsAwarded = 0 });
                else
                {
                    for (int cnt = LastMilestoneIndex; cnt < NewEarnedMilestoneIndex; cnt++)
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

            // Get total percentage of quest completed
            decimal TotalQuestPercentCompleted = 0;
            int TotalPointsEarnedWithCurrBet = DbTotalPointsEarned + questPointsEarned;
            if (TotalPointsEarnedWithCurrBet >= questConfig.RequiredPoints)
                TotalQuestPercentCompleted = 100;
            else
                TotalQuestPercentCompleted = Math.Round(((decimal)TotalPointsEarnedWithCurrBet / questConfig.RequiredPoints) * 100);
            //decimal TotalQuestPercentCompleted = ((decimal)(completedMilestones.Count + LastMilestoneIndex) / (decimal)questConfig.TotalMilestones) * (decimal)100;

            // Update PlayerQuestState Persistence
            playerQuestState.LastMilestoneIndexCompleted = (completedMilestones.Count > 0 ? completedMilestones.Max(m => m.MilestoneIndex) : LastMilestoneIndex);
            playerQuestState.TotalQuestPercentCompleted = TotalQuestPercentCompleted;
            playerQuestState.DateUpdated = DateTime.Now;
            if (!Convert.ToBoolean(PersistPlayerQuestState(playerQuestState)))
                throw new Exception("Error while updating PlayerQuest Persistence");

            // Compute quest percentage completed
            response.QuestPointsEarned = questPointsEarned;
            response.MilestonesCompleted = completedMilestones;
            response.TotalQuestPercentCompleted = TotalQuestPercentCompleted;

            return response;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public bool InsertPlayerQuestProgress(PlayerQuestProgress questProgress)
        {
            try
            {
                _context.PlayerQuestProgress.Add(questProgress);
                return (_context.SaveChanges() >= 0);
            }
            catch (Exception)
            {
                throw new Exception("Error in inserting Player Quest Progress");
            }
        }
        public Task<PlayerQuestState> GetPlayerQuestState(string playerId, string questId)
        {
            if (string.IsNullOrEmpty(questId))
            {
                questId = _questConfigService.GetLoadedActiveQuestConfig().QuestId;
            }
            return Task.FromResult(_context.PlayerQuestState.Where(state => state.PlayerId == playerId && state.QuestId == questId).FirstOrDefault());
        }

        // Updating the Player's QuestState
        public bool PersistPlayerQuestState(PlayerQuestState questState)
        {
            try
            {
                if (_context.PlayerQuestState.Any(q => q.PlayerId == questState.PlayerId && q.QuestId == questState.QuestId))
                    _context.Update(questState);
                else
                    _context.Add(questState);

                return (_context.SaveChanges() >= 0);
            }
            catch (Exception ex)
            {
                throw new Exception("Error on getting PlayerQuest State" + Environment.NewLine + ex.Message);
            }
        }
    }
}
