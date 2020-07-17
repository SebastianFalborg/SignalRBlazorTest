using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignalRBlazor.Server.Models
{
	[Table("PollOption")]
    public class PollOptionEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Votes { get; set; }

        public PollEntity Poll { get; set; }
    }
}
