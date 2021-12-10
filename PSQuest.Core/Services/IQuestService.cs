using PSQuest.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Core.Services
{
    public interface IQuestService 
    {
        Task<List<QuestConfig>> LoadQuestConfigurations();
        QuestConfig GetActiveQuestInfo();

    }
}
