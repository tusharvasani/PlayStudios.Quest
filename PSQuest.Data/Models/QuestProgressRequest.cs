using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Data.Models
{
    public class QuestProgressRequest
    {
        public string PlayerId { get; set; }
        public int PlayerLevel { get; set; }
        public int ChipBetAmount { get; set; }
    }
}
