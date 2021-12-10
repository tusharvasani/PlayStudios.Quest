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
    public class QuestService : IQuestService, IDisposable
    {
        private readonly string _questConfigPath;
        public QuestService(string questConfigPath)
        {
            _questConfigPath = questConfigPath;
        }

        public QuestConfig GetActiveQuestInfo()
        {
            List<QuestConfig> lstQuests = LoadQuestConfigurations().Result;
            if (lstQuests == null)
                throw new Exception("Quest configurations not found!");
            else 
                return lstQuests.Where(conf => conf.IsActive).FirstOrDefault();
        }

        public Task<List<QuestConfig>> LoadQuestConfigurations()
        {
            try
            {
                String quests = File.ReadAllText(_questConfigPath);
                if (!string.IsNullOrEmpty(quests))
                {
                    return Task.Run(() => JsonConvert.DeserializeObject<List<QuestConfig>>(quests));
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
