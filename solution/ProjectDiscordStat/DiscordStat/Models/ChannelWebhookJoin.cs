using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    [Table("ChannelWebhookJoin")]
    public partial class ChannelWebhookJoin
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        public int? ChannelPk { get; set; }
        public int? WebhookPk { get; set; }
    }
}
