using PSQuest.Data.Models;
using PSQuest.Data.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Core.Services
{
    public interface IQuestProgressRepository
    {
        Task<QuestProgressResponse> ComputeQuestProgress(QuestProgressRequest request); 
    }
}
