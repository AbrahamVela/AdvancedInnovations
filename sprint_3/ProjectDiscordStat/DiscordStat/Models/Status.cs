using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    [Table("Status")]
    public partial class Status
    {
        [Key]
        [Column("StatusPK")]
        public int StatusPk { get; set; }
        [StringLength(256)]
        public string UserId { get; set; }
        [Column("Status")]
        [StringLength(256)]
        public string Status1 { get; set; }
        [StringLength(256)]
        public string ServerId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
    }
}
