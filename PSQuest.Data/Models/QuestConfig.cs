using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Data.Models
{
    public class QuestConfig
    {
        private string questId;

        public string QuestId
        {
            get { return questId; }
            set { questId = value; }
        }
        private string questName;

        public string QuestName
        {
            get { return questName; }
            set { questName = value; }
        }
        
        private int totalMilestones;
        public int TotalMilestones
        {
            get { return totalMilestones; }
            set { totalMilestones = value; }
        }
        private int requiredPoints;

        public int RequiredPoints
        {
            get { return requiredPoints; }
            set { requiredPoints = value; }
        }
        private int rateFromBet;
        public int RateFromBet
        {
            get { return rateFromBet; }
            set { rateFromBet = value; }
        }
        private int levelBonusRate;

        public int LevelBonusRate
        {
            get { return levelBonusRate; }
            set { levelBonusRate = value; }
        }
        private bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        private List<QuestMilestone> milestones;

        public List<QuestMilestone> Milestones
        {
            get { return milestones; }
            set { milestones = value; }
        }

    }
    public class QuestMilestone
    {
        private int milestoneIndex;

        public int MilestoneIndex
        {
            get { return milestoneIndex; }
            set { milestoneIndex = value; }
        }
        private int pointsToCompleteMilestone;

        public int PointsToCompleteMilestone
        {
            get { return pointsToCompleteMilestone; }
            set { pointsToCompleteMilestone = value; }
        }
        private int chipsAwarded;

        public int ChipsAwarded
        {
            get { return chipsAwarded; }
            set { chipsAwarded = value; }
        }
    }
}
