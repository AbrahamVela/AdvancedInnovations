using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    [Keyless]
    public partial class ServerMember
    {
        public int ServerPk { get; set; }
        [Required]
        [Column("ID")]
        [StringLength(128)]
        public string Id { get; set; }
        public int? Members { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }
    }
}
