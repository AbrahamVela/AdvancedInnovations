using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    [Table("MessageInfo")]
    public partial class MessageInfo
    {
        [Required]
        [Column("ID")]
        [StringLength(256)]
        public string Id { get; set; }
        [Key]
        public int MessageDataPk { get; set; }
        [StringLength(256)]
        public string ServerId { get; set; }
        [StringLength(256)]
        public string ChannelId { get; set; }
        [StringLength(256)]
        public string UserId { get; set; }
        [StringLength(256)]
        public string CreatedAt { get; set; }
        [StringLength(1000)]
        public string Emojis { get; set; }
        [StringLength(1000)]
        public string Reactions { get; set; }
        [Column("ReactionURL")]
        [StringLength(2000)]
        public string ReactionUrl { get; set; }
    }
}
