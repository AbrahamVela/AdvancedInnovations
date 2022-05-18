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

        public virtual DbSet<Channel> Channels { get; set; }
        public virtual DbSet<ChannelWebhookJoin> ChannelWebhookJoins { get; set; }
        public virtual DbSet<DiscordUserAndUserWebSiteInfo> DiscordUserAndUserWebSiteInfos { get; set; }
        public virtual DbSet<MessageInfo> MessageInfos { get; set; }
        public virtual DbSet<Presence> Presences { get; set; }
        public virtual DbSet<Server> Servers { get; set; }
        public virtual DbSet<ServerChannelJoin> ServerChannelJoins { get; set; }
        public virtual DbSet<ServerMember> ServerMembers { get; set; }
        public virtual DbSet<ServerPresenceJoin> ServerPresenceJoins { get; set; }
        public virtual DbSet<ServerUserJoin> ServerUserJoins { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<VoiceChannel> VoiceChannels { get; set; }
        public virtual DbSet<VoiceState> VoiceStates { get; set; }
        public virtual DbSet<Webhook> Webhooks { get; set; }

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
                    .HasName("PK__Channel__38C3B12676306AE0");
            });

            modelBuilder.Entity<ChannelWebhookJoin>(entity =>
            {
                entity.HasOne(d => d.ChannelPkNavigation)
                    .WithMany(p => p.ChannelWebhookJoins)
                    .HasForeignKey(d => d.ChannelPk)
                    .HasConstraintName("ChannelWebhookJoinChannelPk");

                entity.HasOne(d => d.WebhookPkNavigation)
                    .WithMany(p => p.ChannelWebhookJoins)
                    .HasForeignKey(d => d.WebhookPk)
                    .HasConstraintName("ChannelWebhookJoinWebhookPk");
            });

            modelBuilder.Entity<DiscordUserAndUserWebSiteInfo>(entity =>
            {
                entity.HasKey(e => e.DiscordUserPk)
                    .HasName("PK__DiscordU__1F12BE9551ACDA16");
            });

            modelBuilder.Entity<MessageInfo>(entity =>
            {
                entity.HasKey(e => e.MessageDataPk)
                    .HasName("PK__MessageI__2389D5B5EEF956AF");
            });

            modelBuilder.Entity<Presence>(entity =>
            {
                entity.HasKey(e => e.PresencePk)
                    .HasName("PK__Presence__4981B3D98A743D37");
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.HasKey(e => e.ServerPk)
                    .HasName("PK__Server__C56B03869BDDD7E9");
            });

            modelBuilder.Entity<ServerChannelJoin>(entity =>
            {
                entity.HasOne(d => d.ChannelPkNavigation)
                    .WithMany(p => p.ServerChannelJoins)
                    .HasForeignKey(d => d.ChannelPk)
                    .HasConstraintName("ServerChannelJoinChannelPk");

                entity.HasOne(d => d.ServerPkNavigation)
                    .WithMany(p => p.ServerChannelJoins)
                    .HasForeignKey(d => d.ServerPk)
                    .HasConstraintName("ServerChannelJoinServerPk");
            });

            modelBuilder.Entity<ServerMember>(entity =>
            {
                entity.Property(e => e.ServerPk).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ServerPresenceJoin>(entity =>
            {
                entity.HasOne(d => d.PresencePkNavigation)
                    .WithMany(p => p.ServerPresenceJoins)
                    .HasForeignKey(d => d.PresencePk)
                    .HasConstraintName("ServerPresenceJoinPresencePk");

                entity.HasOne(d => d.ServerPkNavigation)
                    .WithMany(p => p.ServerPresenceJoins)
                    .HasForeignKey(d => d.ServerPk)
                    .HasConstraintName("ServerPresenceJoinServerPk");
            });

            modelBuilder.Entity<ServerUserJoin>(entity =>
            {
                entity.HasOne(d => d.DiscordUserPkNavigation)
                    .WithMany(p => p.ServerUserJoins)
                    .HasForeignKey(d => d.DiscordUserPk)
                    .HasConstraintName("ServerUserJoinDiscordUserPk");

                entity.HasOne(d => d.ServerPkNavigation)
                    .WithMany(p => p.ServerUserJoins)
                    .HasForeignKey(d => d.ServerPk)
                    .HasConstraintName("ServerUserJoinServerPk");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(e => e.StatusPk)
                    .HasName("PK__Status__C8EDEB6A878DDBF6");
            });

            modelBuilder.Entity<VoiceChannel>(entity =>
            {
                entity.HasKey(e => e.VoiceChannelPk)
                    .HasName("PK__VoiceCha__004F00F9857C2CD9");
            });

            modelBuilder.Entity<VoiceState>(entity =>
            {
                entity.HasKey(e => e.VoiceStatePk)
                    .HasName("PK__VoiceSta__02A65DC36766BDEF");
            });

            modelBuilder.Entity<Webhook>(entity =>
            {
                entity.HasKey(e => e.WebhookPk)
                    .HasName("PK__Webhook__238C26FDA5CE029A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
