using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Data.Models
{
    public class QuestProgressRequest
    {
        [Required]
        public string PlayerId { get; set; }
        [Required] 
        public int PlayerLevel { get; set; }
        [Required]
        public int ChipBetAmount { get; set; }
    }
}
