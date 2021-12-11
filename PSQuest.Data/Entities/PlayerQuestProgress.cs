using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Data.Entities
{
    public class PlayerQuestProgress
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string PlayerId { get; set; }
        public string QuestId { get; set; }
        public int PlayerLevel { get; set; }
        public int ChipAmountBet { get; set; }
        public int QuestPointsEarned { get; set; }
        public DateTime InsertDateTime { get; set; }

    }
}
