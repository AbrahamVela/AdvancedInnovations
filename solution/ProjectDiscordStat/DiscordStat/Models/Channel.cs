using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    [Table("Channel")]
    public partial class Channel
    {
        [Key]
        public int ChannelPk { get; set; }
        [Column("ID")]
        [StringLength(256)]
        public string? Id { get; set; }
        [StringLength(256)]
        public string? Type { get; set; }
        [StringLength(256)]
        public string? Name { get; set; }
        public int? Count { get; set; }
        [Column("Guild_id")]
        [StringLength(256)]
        public string? GuildId { get; set; }
    }
}
