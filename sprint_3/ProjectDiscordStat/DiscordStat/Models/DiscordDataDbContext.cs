using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DiscordStats.Models
{
    public partial class DiscordDataDbContext : DbContext
    {
        public DiscordDataDbContext()
        {
        }

        public DiscordDataDbContext(DbContextOptions<DiscordDataDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Channel> Channels { get; set; } = null!;
        public virtual DbSet<ChannelWebhookJoin> ChannelWebhookJoins { get; set; } = null!;
        public virtual DbSet<DiscordUserAndUserWebSiteInfo> DiscordUserAndUserWebSiteInfos { get; set; } = null!;
        public virtual DbSet<MessageInfo> MessageInfos { get; set; } = null!;
        public virtual DbSet<Presence> Presences { get; set; } = null!;
        public virtual DbSet<Server> Servers { get; set; } = null!;
        public virtual DbSet<ServerChannelJoin> ServerChannelJoins { get; set; } = null!;
        public virtual DbSet<ServerPresenceJoin> ServerPresenceJoins { get; set; } = null!;
        public virtual DbSet<ServerUserJoin> ServerUserJoins { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<VoiceChannel> VoiceChannels { get; set; } = null!;
        public virtual DbSet<VoiceState> VoiceStates { get; set; } = null!;
        public virtual DbSet<Webhook> Webhooks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DiscordDataConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>(entity =>
            {
                entity.HasKey(e => e.ChannelPk)
                    .HasName("PK__Channel__38C3B1263A374A42");
            });

            modelBuilder.Entity<DiscordUserAndUserWebSiteInfo>(entity =>
            {
                entity.HasKey(e => e.DiscordUserPk)
                    .HasName("PK__DiscordU__1F12BE9588163016");
            });

            modelBuilder.Entity<MessageInfo>(entity =>
            {
                entity.HasKey(e => e.MessageDataPk)
                    .HasName("PK__MessageI__2389D5B53C16D937");
            });

            modelBuilder.Entity<Presence>(entity =>
            {
                entity.HasKey(e => e.PresencePk)
                    .HasName("PK__Presence__4981B3D9EF057BB6");
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.HasKey(e => e.ServerPk)
                    .HasName("PK__Server__C56B0386CBCF582E");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(e => e.StatusPk)
                    .HasName("PK__Status__C8EDEB6AFC03A95D");
            });

            modelBuilder.Entity<VoiceChannel>(entity =>
            {
                entity.HasKey(e => e.VoiceChannelPk)
                    .HasName("PK__VoiceCha__004F00F9C0E4B4A5");
            });

            modelBuilder.Entity<VoiceState>(entity =>
            {
                entity.HasKey(e => e.VoiceStatePk)
                    .HasName("PK__VoiceSta__02A65DC3B236EE8B");
            });

            modelBuilder.Entity<Webhook>(entity =>
            {
                entity.HasKey(e => e.WebhookPk)
                    .HasName("PK__Webhook__238C26FD1E340158");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
