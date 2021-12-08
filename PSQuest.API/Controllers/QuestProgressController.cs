using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSQuest.Core.Services;
using PSQuest.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSQuest.API.Controllers
{
    [Route("api/progress")]
    [ApiController]
    public class QuestProgressController : ControllerBase
    {
        private readonly IQuestProgressRepository questProgressRepository;

        public QuestProgressController(IQuestProgressRepository repository)
        {
            questProgressRepository = repository;
        }
        [HttpPost("progress")]
        public ActionResult GetProgress(QuestProgressRequest request)
        {
            return Ok(questProgressRepository.ComputeQuestProgress(request));
        }
    }
}
