using PSQuest.Data.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Core.Services
{
    public class QuestStateRepository : IQuestStateRepository, IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task<QuestStateResponse> GetPlayerQuestState(string playerId)
        {
            throw new NotImplementedException();
        }
    }
}
