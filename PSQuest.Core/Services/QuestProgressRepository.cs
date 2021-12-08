using PSQuest.Data.Models;
using PSQuest.Data.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Core.Services
{
    public class QuestProgressRepository : IQuestProgressRepository, IDisposable
    {
        public Task<QuestProgressResponse> ComputeQuestProgress(QuestProgressRequest request)
        {

            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
