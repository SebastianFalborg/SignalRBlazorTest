using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SingalRBlazor.Server.Data;
using SingalRBlazor.Server.Hubs;
using SingalRBlazor.Server.Models;
using SingalRBlazor.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingalRBlazor.Server.Services
{
    public class PollService
    {
		private readonly IHubContext<PollHub> hubContext;
		private readonly ApplicationDbContext applicationDbContext;

		public PollService(IHubContext<PollHub> hubContext, ApplicationDbContext applicationDbContext)
		{
			this.hubContext = hubContext;
			this.applicationDbContext = applicationDbContext;
		}

		public Poll GetPoll(Guid id)
		{
			var pollEntity = applicationDbContext.Polls.Include(p => p.PollOptions).FirstOrDefault(x => x.Id == id);

			if (pollEntity == null)
				return null;

			var poll = MapPollEntity(pollEntity);

			return poll;
		}

		public IEnumerable<Poll> GetRunningPolls()
		{
			var currentPolls = applicationDbContext.Polls.Where(x => x.PollEnd > DateTime.UtcNow).Include(p => p.PollOptions).OrderByDescending(x => x.PollEnd).ToList();

			var mappedCurrentPolls = this.MapPollEntities(currentPolls);

			return mappedCurrentPolls;
		}

		public IEnumerable<Poll> GetExpiredPolls()
		{
			var currentPolls = applicationDbContext.Polls.Where(x => x.PollEnd < DateTime.UtcNow).Include(p => p.PollOptions).OrderByDescending(x => x.PollEnd).ToList();

			var mappedCurrentPolls = this.MapPollEntities(currentPolls);

			return mappedCurrentPolls;
		}

		public async Task<Guid> AddPoll(Poll poll)
		{
			applicationDbContext.Polls.Add(MapPoll(poll));

			await applicationDbContext.SaveChangesAsync();

			await this.hubContext.Clients.Group("PollListUpdates").SendAsync("PollChanged");

			return poll.Id;
		}

		public async Task VoteOnOption(Guid pollId, Guid pollOptionId)
		{
			var poll = this.applicationDbContext.Polls.Include(p => p.PollOptions).FirstOrDefault(x => x.Id == pollId && x.PollEnd > DateTime.UtcNow);

			if (poll == null)
				return;

			var pollOption = poll.PollOptions.FirstOrDefault(x => x.Id == pollOptionId);

			if (pollOption != null)
				pollOption.Votes += 1;

			this.applicationDbContext.SaveChanges();

			await this.hubContext.Clients.Group(pollId.ToString()).SendAsync("PollChanged");
		}

		private PollEntity MapPoll(Poll poll)
		{
			var mappedPollEntity = new PollEntity() { Id = poll.Id, Name = poll.Name, PollEnd = poll.PollEnd };

			var pollOptionEntities = new List<PollOptionEntity>();

			foreach (var pollOption in poll.PollOptions)
			{
				pollOptionEntities.Add(new PollOptionEntity() { Id = pollOption.Id, Name = pollOption.Name, Votes = pollOption.Votes });
			}

			mappedPollEntity.PollOptions = pollOptionEntities;

			return mappedPollEntity;
		}

		private Poll MapPollEntity(PollEntity pollEntity)
		{
			var mappedPoll = new Poll() { Id = pollEntity.Id, Name = pollEntity.Name, PollEnd = pollEntity.PollEnd };

			foreach (var pollOptionEntity in pollEntity.PollOptions)
			{
				mappedPoll.PollOptions.Add(new PollOption() { Id = pollOptionEntity.Id, Name = pollOptionEntity.Name, Votes = pollOptionEntity.Votes });
			}

			return mappedPoll;
		}

		private List<Poll> MapPollEntities(IEnumerable<PollEntity> pollEntities)
		{
			var polls = new List<Poll>();

			foreach (var pollEntity in pollEntities)
			{
				var mappedPoll = new Poll() { Id = pollEntity.Id, Name = pollEntity.Name, PollEnd = pollEntity.PollEnd };

				foreach (var pollOptionEntity in pollEntity.PollOptions)
				{
					mappedPoll.PollOptions.Add(new PollOption() { Id = pollOptionEntity.Id, Name = pollOptionEntity.Name, Votes = pollOptionEntity.Votes });
				}

				polls.Add(mappedPoll);
			}

			return polls;
		}
	}
}
