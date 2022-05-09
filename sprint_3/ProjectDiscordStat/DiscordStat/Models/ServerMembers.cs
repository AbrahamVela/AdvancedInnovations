using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    [Table("ServerMembers")]
    public partial class ServerMembers
    {
        public ServerMembers()
        {
            
        }
        [Column("ID")]
        [StringLength(128)]
        public string Id { get; set; } = null!;
        [Key]
        public int ServerPk { get; set; }
        [StringLength(256)]

        public int? Members { get; set; }
        [StringLength(256)]
        public DateTime Date { get; set; }
        

    }
}
