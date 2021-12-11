using System.Collections.Generic;

namespace PSQuest.Data.Transfer
{
    public class QuestProgressResponse
    {
        public int QuestPointsEarned { get; set; }
        public decimal TotalQuestPercentCompleted { get; set; }
        public List<Milestone> MilestonesCompleted { get; set; }
        //public string InfoMessage { get; set; }
        //public string ErrorMessage { get; set; }
    }

    public class Milestone
    {
        public int MilestoneIndex { get; set; }
        public int ChipsAwarded { get; set; }

    }
}
