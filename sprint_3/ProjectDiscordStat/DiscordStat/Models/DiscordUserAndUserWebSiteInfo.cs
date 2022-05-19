using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    [Table("DiscordUserAndUserWebSiteInfo")]
    public partial class DiscordUserAndUserWebSiteInfo
    {
        [Column("ID")]
        [StringLength(256)]
        public string Id { get; set; } = null!;
        [Key]
        public int DiscordUserPk { get; set; }
        [StringLength(128)]
        public string? Username { get; set; }
        [StringLength(256)]
        public string? Servers { get; set; }
        [StringLength(256)]
        public string? Avatar { get; set; }
        [StringLength(256)]
        public string? Role { get; set; }
        [StringLength(128)]
        public string? FirstName { get; set; }
        [StringLength(128)]
        public string? LastName { get; set; }
        [StringLength(256)]
        public string? BirthDate { get; set; }
        [StringLength(256)]
        public string? Email { get; set; }
    }
}
