using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Data.Entities
{
    public class PlayerQuestState
    {
        [MaxLength(50)]
        public string PlayerId { get; set; }
        public string QuestId { get; set; }
        public decimal TotalQuestPercentCompleted { get; set; }
        public int LastMilestoneIndexCompleted { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
