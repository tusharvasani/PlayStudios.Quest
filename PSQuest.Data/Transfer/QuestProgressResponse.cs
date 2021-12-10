using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Data.Transfer
{
    public class QuestProgressResponse
    {
        public int QuestPointsEarned { get; set; }
        public decimal TotalQuestPercentCompleted { get; set; }
        public List<Milestone> MilestonesCompleted { get; set; }
    }

    public class Milestone
    {
        public int MilestoneIndex { get; set; }
        public int ChipsAwarded { get; set; }

    }
}
