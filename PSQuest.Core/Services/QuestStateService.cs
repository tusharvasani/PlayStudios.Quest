using PSQuest.Core.Common;
using PSQuest.Data;
using PSQuest.Data.Entities;
using PSQuest.Data.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSQuest.Core.Services
{
    public class QuestStateService : IQuestStateService, IDisposable
    {
        private readonly QuestDbContext _context;

        public QuestStateService(QuestDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Task<PlayerQuestState> GetPlayerQuestState(string playerId, string questId)
        {
            if (string.IsNullOrEmpty(questId))
            {
                QuestService questService = new QuestService(General.QuestConfigFilePath);
                questId = questService.GetActiveQuestInfo().QuestId;
            }
            return Task.FromResult(_context.PlayerQuestState.Where(state => state.PlayerId == playerId && state.QuestId == questId).FirstOrDefault());
        }

        // Updating the Player's QuestState
        public bool PersistPlayerQuestState(PlayerQuestState questState)
        {
            try
            {
                if (_context.PlayerQuestState.Any(q => q.PlayerId == questState.PlayerId && q.QuestId == questState.QuestId))
                    _context.Update(questState);
                else
                    _context.Add(questState);

                return (_context.SaveChanges() >= 0);
            }
            catch (Exception ex)
            {
                throw new Exception("Error on getting PlayerQuest State" + Environment.NewLine + ex.Message);
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
