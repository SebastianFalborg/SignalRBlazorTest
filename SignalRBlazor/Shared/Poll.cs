using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRBlazor.Shared
{
    public class Poll
    {
		public Guid Id { get; set; } = Guid.NewGuid();

		public string Name { get; set; }

		public DateTime PollEnd { get; set; } = DateTime.UtcNow.AddMinutes(5);

		public List<PollOption> PollOptions { get; set; } = new List<PollOption>();
	}
}
