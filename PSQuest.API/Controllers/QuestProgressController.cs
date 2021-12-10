using Microsoft.AspNetCore.Mvc;
using PSQuest.Core.Services;
using PSQuest.Data.Models;

namespace PSQuest.API.Controllers
{
    [Route("api/progress")]
    [ApiController]
    public class QuestProgressController : ControllerBase
    {
        private readonly IQuestProgressService _questProgressService;
        public QuestProgressController(IQuestProgressService questProgressService)
        {
            _questProgressService = questProgressService;
        }
        [HttpPost("progress")]
        public ActionResult QuestProgress(QuestProgressRequest request)
        {
            if (request == null)
                return BadRequest();

            return Ok(_questProgressService.ComputeQuestProgress(request));
        }
    }
}
