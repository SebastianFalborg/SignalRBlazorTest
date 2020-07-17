using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignalRBlazor.Server.Models
{
	[Table("Poll")]
    public class PollEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime PollEnd { get; set; }

        public ICollection<PollOptionEntity> PollOptions { get; set; }
    }
}
