using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PSQuest.Core.Services;
using PSQuest.Data.Entities;
using PSQuest.Data.Models;
using PSQuest.Data.Transfer;

namespace PSQuest.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class QuestController : ControllerBase
    {
        private readonly IQuestService _questProgressService;
        private readonly IMapper _mapper;
        public QuestController(IQuestService questProgressService, IMapper mapper)
        {
            _questProgressService = questProgressService;
            _mapper = mapper;
        }

        [HttpPost("progress")]
        public ActionResult<QuestProgressResponse> QuestProgress(QuestProgressRequest request)
        {
            if (request == null)
                return BadRequest();

            return Ok(_questProgressService.ComputeQuestProgress(request).Result);
        }

        [HttpGet("state")]
        public ActionResult<QuestStateResponse> GetPlayerQuestState(QuestStateRequest request)
        {
            if (request == null)
                return BadRequest();
            PlayerQuestState response = _questProgressService.GetPlayerQuestState(request.PlayerId, string.Empty).Result;
            if (response != null)
                return Ok(_mapper.Map<QuestStateResponse>(response));
            else
                return NotFound("No Player Quest State found for this PlayerId");
        }
    }
}
