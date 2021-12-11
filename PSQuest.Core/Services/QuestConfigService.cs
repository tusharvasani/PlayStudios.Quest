using Newtonsoft.Json;
using PSQuest.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Core.Services
{
    public class QuestConfigService : IQuestConfigService, IDisposable
    {
        private readonly string _questConfigPath;

        private QuestConfig _activeQuestConfig;

        public QuestConfig ActiveQuestConfig
        {
            get { return _activeQuestConfig; }
            set { _activeQuestConfig = value; }
        }

        public QuestConfigService(string questConfigPath)
        {
            _questConfigPath = questConfigPath;
            _activeQuestConfig = LoadAndGetActiveQuestConfig();
        }

        public QuestConfig LoadAndGetActiveQuestConfig()
        {
            List<QuestConfig> lstQuests = null;
            String quests = File.ReadAllText(_questConfigPath);
            if (!string.IsNullOrEmpty(quests))
            {
                try
                {
                    lstQuests = JsonConvert.DeserializeObject<List<QuestConfig>>(quests);
                }
                catch (Exception)
                {
                    throw new Exception(string.Format("Invalid Quest Configuration found, Please check Quest Configuration at path: {0}",  _questConfigPath));
                }
            }
            if (lstQuests == null)
                throw new Exception("Quest configurations not found!");
            else
                return lstQuests.Where(conf => conf.IsActive).FirstOrDefault();
        }

        public QuestConfig GetLoadedActiveQuestConfig()
        {
            return _activeQuestConfig;

        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
