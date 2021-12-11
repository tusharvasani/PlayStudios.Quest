using PSQuest.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Core.Services
{
    public interface IQuestConfigService 
    {
        QuestConfig LoadAndGetActiveQuestConfig();
        QuestConfig GetLoadedActiveQuestConfig();

    }
}
