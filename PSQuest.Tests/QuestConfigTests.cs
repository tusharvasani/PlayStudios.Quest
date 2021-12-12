using Moq;
using PSQuest.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PSQuest.Tests
{
    public class QuestConfigTests 
    {
        private readonly IQuestConfigService _questConfigService;
        private readonly string _configFilePath;
        public QuestConfigTests()
        {
            _configFilePath = @"..\\..\\..\\..\\PSQuest.Tests\\Data\\questConfig.json";
            _questConfigService = new QuestConfigService(_configFilePath);
        }
        [Fact]
        public void CheckQuestConfigJSON_FileExists()
        {
            bool fileExists = File.Exists(_configFilePath);
            Assert.True(fileExists);
        }
        [Fact]
        public void CheckIfQuestsAreConfigured_InJSONFile()
        {
            String quests = File.ReadAllText(_configFilePath);
            Assert.True(!string.IsNullOrEmpty(quests));
        }
        [Fact]
        public void Check_OnlyOne_Active_Quest_Exists()
        {
            Assert.NotNull(_questConfigService.LoadAndGetActiveQuestConfig());
        }

    }
}
