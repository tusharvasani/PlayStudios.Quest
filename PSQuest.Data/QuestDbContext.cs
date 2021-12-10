using Microsoft.EntityFrameworkCore;
using PSQuest.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Data
{
    public class QuestDbContext : DbContext
    {
        public QuestDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PlayerQuestState>().HasKey(t => new { t.PlayerId, t.QuestId });
        }
        public DbSet<Player> Player { get; set; }
        public DbSet<PlayerQuestProgress> PlayerQuestProgress { get; set; }
        public DbSet<PlayerQuestState> PlayerQuestState { get; set; }

        
    }
}
