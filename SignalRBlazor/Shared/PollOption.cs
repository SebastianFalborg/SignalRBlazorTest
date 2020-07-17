using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRBlazor.Shared
{
    public class PollOption
    {
		public Guid Id { get; set; } = Guid.NewGuid();

		public string Name { get; set; }

		public int Votes { get; set; } = 0;
	}
}
