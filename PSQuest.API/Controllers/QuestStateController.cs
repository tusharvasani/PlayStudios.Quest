using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSQuest.Core.Services;
using PSQuest.Data.Entities;
using PSQuest.Data.Models;
using PSQuest.Data.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSQuest.API.Controllers
{
    [Route("api/state")]
    [ApiController]
    public class QuestStateController : ControllerBase
    {
        private readonly IQuestStateService _questStateService;
        private readonly IMapper _mapper;
        public QuestStateController(IQuestStateService questStateService, IMapper mapper)
        {
            _questStateService = questStateService ??
                throw new ArgumentNullException(nameof(questStateService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet("state")]
        public ActionResult<QuestStateResponse> GetPlayerQuestState(QuestStateRequest request)
        {
            if (request == null)
                return BadRequest();
            PlayerQuestState  response = _questStateService.GetPlayerQuestState(request.PlayerId, string.Empty).Result;
            if (response != null)
                return Ok(_mapper.Map<QuestStateResponse>(response));
            else
                return NotFound("No Player Quest State found for this PlayerId");
        }
    }
}
