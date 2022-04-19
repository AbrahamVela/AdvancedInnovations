using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    [Table("VoiceState")]
    public partial class VoiceState
    {
        [Key]
        public int MessageDataPk { get; set; }
        [StringLength(256)]
        public string? ServerId { get; set; }
        [StringLength(256)]
        public string? ChannelId { get; set; }
        [StringLength(256)]
        public string? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
    }
}
