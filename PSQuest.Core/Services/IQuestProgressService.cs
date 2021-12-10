using PSQuest.Data.Entities;
using PSQuest.Data.Models;
using PSQuest.Data.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Core.Services
{
    public interface IQuestProgressService
    {
        Task<QuestProgressResponse> ComputeQuestProgress(QuestProgressRequest request);

        bool InsertPlayerQuestProgress(PlayerQuestProgress questProgress);
    }
}
