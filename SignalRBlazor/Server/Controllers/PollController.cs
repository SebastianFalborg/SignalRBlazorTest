using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalRBlazor.Server.Services;
using SignalRBlazor.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRBlazor.Server.Controllers
{
	[ApiController]
    [Route("api/[controller]")]
    public class PollController : ControllerBase
    {
		private readonly PollService pollService;

		public PollController(PollService pollService)
		{
			this.pollService = pollService;
		}

		[HttpGet("{id}")]
		public Poll Get(Guid id)
		{
			var currentPoll = this.pollService.GetPoll(id);

			return currentPoll;
		}

		[HttpGet]
		public IEnumerable<Poll> Get()
		{
			var currentPolls = this.pollService.GetRunningPolls();

			return currentPolls;
		}

		[HttpGet("expired")]
		public IEnumerable<Poll> GetExpiredPolls()
		{
			var currentPolls = this.pollService.GetExpiredPolls();

			return currentPolls;
		}

		[HttpPost]
		public async Task<Guid> Post(Poll poll)
		{
			var newGuid = await this.pollService.AddPoll(poll);

			return newGuid;
		}

		[HttpPost("vote/{id}")]
		public async Task Vote(Guid id, [FromBody] Guid pollOptionId)
		{
			await this.pollService.VoteOnOption(id, pollOptionId);
		}
	}
}
