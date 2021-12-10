using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Data.Entities
{
    public class Player
    {
        [Key]
        public Guid PlayerId { get; set; }
        [Required]
        [MaxLength(100)]
        public string PlayerName { get; set; }
        [MaxLength(100)]
        public string EmailId { get; set; }
    }
}
