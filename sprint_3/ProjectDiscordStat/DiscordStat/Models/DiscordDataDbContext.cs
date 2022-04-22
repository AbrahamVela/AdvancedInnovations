﻿using System;
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
        public virtual DbSet<VoiceChannel> VoiceChannels { get; set; } = null!;
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
                    .HasName("PK__Channel__38C3B126A82456E7");
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
                    .HasName("PK__DiscordU__1F12BE959EAB2B70");
            });

            modelBuilder.Entity<MessageInfo>(entity =>
            {
                entity.HasKey(e => e.MessageDataPk)
                    .HasName("PK__MessageI__2389D5B52AD19637");
            });

            modelBuilder.Entity<Presence>(entity =>
            {
                entity.HasKey(e => e.PresencePk)
                    .HasName("PK__Presence__4981B3D9DC211D36");
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.HasKey(e => e.ServerPk)
                    .HasName("PK__Server__C56B038615E5A574");
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

            modelBuilder.Entity<VoiceChannel>(entity =>
            {
                entity.HasKey(e => e.VoiceChannelPk)
                    .HasName("PK__VoiceCha__004F00F9D419B0CF");
            });

            modelBuilder.Entity<Webhook>(entity =>
            {
                entity.HasKey(e => e.WebhookPk)
                    .HasName("PK__Webhook__238C26FD4175FBB5");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
