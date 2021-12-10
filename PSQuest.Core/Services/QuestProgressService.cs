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
    public class QuestProgressService : IQuestProgressService, IDisposable
    {
        private readonly QuestDbContext _context;
        public QuestProgressService(QuestDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public Task<QuestProgressResponse> ComputeQuestProgress(QuestProgressRequest questProgressRequest)
        {
            // Check if Player exists
            Player player = _context.Player.Where(p => p.PlayerId.ToString().ToUpper().Equals(questProgressRequest.PlayerId)).FirstOrDefault();
            if (player == null)
                throw new ArgumentNullException(string.Format("Player with PlayerId: {0} not found", questProgressRequest.PlayerId), nameof(player));

            // Get Active Quest configurations
            QuestService _questService = new QuestService(General.QuestConfigFilePath);
            QuestConfig activeQuest = _questService.GetActiveQuestInfo();
            if (activeQuest == null)
                throw new ArgumentNullException("No Active Quest available", nameof(activeQuest));

            if (questProgressRequest.ChipBetAmount < activeQuest.MinChipAmountBet)
                throw new Exception(string.Format("Invalid ChipBetAmount. Minimum Chip Amount Bet for this quest is: {0}", activeQuest.MinChipAmountBet));

            return Task.FromResult(GetQuestMilestonesCompleted(activeQuest, questProgressRequest));
        }

        private QuestProgressResponse GetQuestMilestonesCompleted(QuestConfig questConfig, QuestProgressRequest request)
        {
            QuestProgressResponse response = new QuestProgressResponse();
            List<Milestone> completedMilestones = new List<Milestone>();
            int questPointsEarned = (request.ChipBetAmount * questConfig.RateFromBet) + (request.PlayerLevel * questConfig.LevelBonusRate);
            int LastMilestoneIndex = 0;

            // Get last completed Milestone for Player for a Quest
            QuestStateService questStateService = new QuestStateService(_context);
            PlayerQuestState playerQuestState = questStateService.GetPlayerQuestState(request.PlayerId, questConfig.QuestId).Result;
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

            int chipAmtBet = request.ChipBetAmount;
            for (int cnt = LastMilestoneIndex; cnt < questConfig.Milestones.Count; cnt++)
            {
                if (chipAmtBet >= questConfig.Milestones[cnt].PointsToCompleteMilestone)
                {
                    completedMilestones.Add(new Milestone() { MilestoneIndex = questConfig.Milestones[cnt].MilestoneIndex, ChipsAwarded = questConfig.Milestones[cnt].ChipsAwarded });
                    chipAmtBet -= questConfig.Milestones[cnt].PointsToCompleteMilestone;
                }
                else
                    break;
            }

            // Insert record in PlayerQuestProgress
            PlayerQuestProgress questProgress = new PlayerQuestProgress();
            questProgress.ChipAmountBet = request.ChipBetAmount;
            questProgress.PlayerId = request.PlayerId;
            questProgress.PlayerLevel = request.PlayerLevel;
            questProgress.QuestId = questConfig.QuestId;
            questProgress.InsertDateTime = DateTime.Now;

            if (!Convert.ToBoolean(InsertPlayerQuestProgress(questProgress)))
                throw new Exception("Error in inserting Player Quest Progress record");

            // Get total percentage of quest completed
            decimal totalQuestPercentCompleted = ((decimal)(completedMilestones.Count + LastMilestoneIndex) / (decimal)questConfig.TotalMilestones) * (decimal)100;

            // Update PlayerQuestState Persistence
            playerQuestState.LastMilestoneIndexCompleted = completedMilestones.Max(m => m.MilestoneIndex);
            playerQuestState.TotalQuestPercentCompleted = totalQuestPercentCompleted;
            playerQuestState.DateUpdated = DateTime.Now;
            if (!Convert.ToBoolean(questStateService.PersistPlayerQuestState(playerQuestState)))
                throw new Exception("Error while updating PlayerQuest Persistence");

            // Compute quest percentage completed
            response.QuestPointsEarned = questPointsEarned;
            response.MilestonesCompleted = completedMilestones;
            response.TotalQuestPercentCompleted = totalQuestPercentCompleted;

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
    }
}
