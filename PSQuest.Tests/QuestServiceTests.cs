using Microsoft.EntityFrameworkCore;
using Moq;
using PSQuest.Core.Services;
using PSQuest.Data;
using PSQuest.Data.Entities;
using PSQuest.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PSQuest.Tests
{
    public class QuestServiceTests
    {
        private readonly QuestService serviceTest;
        private readonly Mock<QuestDbContext> dbContextMock = new Mock<QuestDbContext>();
        private readonly Mock<IQuestConfigService> questConfigServiceMock = new Mock<IQuestConfigService>();
        public QuestServiceTests()
        {
           // serviceTest = SetupInMemoryDbContext();
            //serviceTest = new QuestService(dbContextMock.Object, questConfigServiceMock.Object);
        }
        //public QuestService SetupInMemoryDbContext()
        //{
        //    var options = new DbContextOptionsBuilder<QuestDbContext>()
        //        .UseInMemoryDatabase(databaseName: "PSQuest")
        //        .Options;

        //    // Insert seed data into the database using one instance of the context
        //    using (var context = new QuestDbContext(options))
        //    {
        //        context.PlayerQuestState.Add(new PlayerQuestState { PlayerId = "P0001", QuestId = "Q0001", LastMilestoneIndexCompleted = 5, TotalQuestPercentCompleted = 100 });
        //        context.PlayerQuestState.Add(new PlayerQuestState { PlayerId = "P0001", QuestId = "Q0002", LastMilestoneIndexCompleted = 3, TotalQuestPercentCompleted = 30 });
        //        context.PlayerQuestState.Add(new PlayerQuestState { PlayerId = "P0002", QuestId = "Q0002", LastMilestoneIndexCompleted = 2, TotalQuestPercentCompleted = 20 });

        //        context.PlayerQuestProgress.Add(new PlayerQuestProgress { PlayerId = "P0001", QuestId = "Q0001", ChipAmountBet = 500, PlayerLevel = 1, QuestPointsEarned = 3020 });
        //        context.PlayerQuestProgress.Add(new PlayerQuestProgress { PlayerId = "P0001", QuestId = "Q0001", ChipAmountBet = 1000, PlayerLevel = 1, QuestPointsEarned = 6040 });
        //        context.PlayerQuestProgress.Add(new PlayerQuestProgress { PlayerId = "P0001", QuestId = "Q0001", ChipAmountBet = 200, PlayerLevel = 1, QuestPointsEarned = 1208 });
        //        context.PlayerQuestProgress.Add(new PlayerQuestProgress { PlayerId = "P0001", QuestId = "Q0002", ChipAmountBet = 1000, PlayerLevel = 1, QuestPointsEarned = 1040 });
        //        context.PlayerQuestProgress.Add(new PlayerQuestProgress { PlayerId = "P0001", QuestId = "Q0002", ChipAmountBet = 1000, PlayerLevel = 1, QuestPointsEarned = 1040 });
        //        context.PlayerQuestProgress.Add(new PlayerQuestProgress { PlayerId = "P0001", QuestId = "Q0002", ChipAmountBet = 1000, PlayerLevel = 1, QuestPointsEarned = 1040 });
        //        context.PlayerQuestProgress.Add(new PlayerQuestProgress { PlayerId = "P0002", QuestId = "Q0002", ChipAmountBet = 500, PlayerLevel = 2, QuestPointsEarned = 1030 });
        //        context.PlayerQuestProgress.Add(new PlayerQuestProgress { PlayerId = "P0002", QuestId = "Q0002", ChipAmountBet = 500, PlayerLevel = 2, QuestPointsEarned = 1030 });
        //        context.SaveChanges();
        //    }
        //    using (var context = new QuestDbContext(options))
        //    {
        //        return new QuestService(context, questConfigServiceMock.Object);
        //    }
        //}
        //[Fact]
        //public void InsertPlayerQuestProgress_Should_InsertRecordIn_PQuestProgressTable()
        //{
        //    //// Sample object to insert
        //    //PlayerQuestProgress playerQuestProgress = new PlayerQuestProgress();
        //    //playerQuestProgress.PlayerId = "P0004";
        //    //playerQuestProgress.QuestId = "Q0002";
        //    //playerQuestProgress.ChipAmountBet = 500;
        //    //playerQuestProgress.PlayerLevel = 1;
        //    //playerQuestProgress.QuestPointsEarned = 1020;
        //    //playerQuestProgress.InsertDateTime = DateTime.Now;
        //    //dbContextMock.Object.PlayerQuestProgress.Add(playerQuestProgress);
        //    //bool result = dbContextMock.Object.SaveChanges() >= 0;
            
        //    //Assert.True(result);
        //}

        //[Fact]
        //public Task<PlayerQuestState> GetPlayerQuestState_WhenQuestIsActive_WithPlayerId()
        //{
        //    //var playerId = Guid.NewGuid();
        //    //QuestConfig activeQuest = (QuestConfig)questConfigServiceMock.Setup(q => q.GetLoadedActiveQuestConfig()).Returns(new QuestConfig());
        //    //PlayerQuestState playerQuestState = new PlayerQuestState()
        //    //{
        //    //    PlayerId = playerId.ToString(),
        //    //    QuestId = activeQuest.QuestId,
        //    //    LastMilestoneIndexCompleted = 1,
        //    //    TotalQuestPercentCompleted = 10
        //    //};
            
        //    //PlayerQuestState playerState = (PlayerQuestState)dbContextMock.Setup(db => db.PlayerQuestState.Where(s => s.PlayerId == playerId.ToString() && s.QuestId == activeQuest.QuestId).FirstOrDefault()).Returns(playerQuestState);
        //    //PlayerQuestState questState = serviceTest.GetPlayerQuestState(playerId.ToString(), activeQuest.QuestId).Result;
        //    //Assert.Equal(playerState.PlayerId, questState.PlayerId);
        //    //return Task.FromResult(questState);
        //}
        //[Fact]
        //public void GetTotalPointsEarnedByPlayer_FromDatabase()
        //{
        //    //string playerId = "P0001";
        //    //string questId = "Q0001";
        //    //int pointsEarned = dbContextMock.Object.PlayerQuestProgress.Where(q => q.QuestId == questId && q.PlayerId == playerId).Sum(r => r.QuestPointsEarned);
        //    //Assert.Equal(10268, pointsEarned);
        //}
    }
}
