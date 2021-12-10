using PSQuest.Data.Entities;
using PSQuest.Data.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Core.Services
{
    public interface IQuestStateService
    {

        Task<PlayerQuestState> GetPlayerQuestState(string playerId, string questId);

        // This method is responsible to Insert / Update a single record per player and per quest to persist the status of Player Quest
        bool PersistPlayerQuestState(PlayerQuestState questState);
    }
}
