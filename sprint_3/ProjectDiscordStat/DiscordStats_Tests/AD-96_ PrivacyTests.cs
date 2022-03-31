﻿using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using DiscordStats.DAL.Concrete;
using DiscordStats.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Contrib.HttpClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DiscordStats.DAL.Abstract;
using DiscordStats.ViewModel;
using DiscordStats.ViewModels;
using DiscordStats.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DiscordStats_Tests
{
    internal class AD_96__PrivacyTests
    {
        private Mock<DiscordDataDbContext> _mockContext;

        private Mock<DbSet<Server>> _mockServerDbSet;
        private List<Server> _servers = FakeData.Servers;

        private IServerRepository _serverRepository;

        private Mock<DbSet<ServerPartial>> _mockPartialServerDbSet;
        private Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> entities) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());
            return mockSet;
        }

        [SetUp]
        public void Setup()
        {
            var ser = new List<Server>
        {
            new Server{Id = "789317480325316640", ServerPk = 1, Name = "input/output server", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=5, OwnerId="1234124523525463asdgads", VerificationLevel="3", Description=null, PremiumTier="NONE", ApproximatePresenceCount=2, Privacy="private", OnForum="yes", Message="this is the message"},
            new Server{Id = "928010025958510632", ServerPk = 2, Name = "Advanced Innovations", Owner = "false", Icon = "d8f49d144185733c210456853906b631", HasBot = "true", ApproximateMemberCount=5, OwnerId="786safd67sdfg657sf6", VerificationLevel="2", Description=null, PremiumTier="NONE", ApproximatePresenceCount=1, Privacy="public", OnForum="no", Message="this is the message"},
            new Server{Id = "151516415641361", ServerPk = 3, Name = "fake server", Owner = "false", Icon = "", HasBot = "false", ApproximateMemberCount=23532, OwnerId="678bf86sg97n7jdf", VerificationLevel="1", Description=null, PremiumTier="NONE", ApproximatePresenceCount=30, Privacy="private", OnForum="yes", Message="this is the message"}
        }.AsQueryable();

            _mockServerDbSet = GetMockDbSet<Server>(ser.AsQueryable<Server>());
            var q = _mockServerDbSet.As<IQueryable<Server>>();
            q.Setup(m => m.GetEnumerator()).Returns(() => ser.GetEnumerator());
            _mockContext = new Mock<DiscordDataDbContext>();
            _mockContext.Setup(ctx => ctx.Servers).Returns(_mockServerDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<Server>()).Returns(_mockServerDbSet.Object);
            _mockContext.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);


        }

        [Test]
        public void UpdatePrivacyReturnsTrue()
        {
            _serverRepository = new ServerRepository(_mockContext.Object);
            var truth = _serverRepository.UpdatePrivacy("789317480325316640", "public");
            Assert.IsTrue(truth);
        }

        [Test]
        public void UpdatePrivacyReturnsFalse()
        {
            _serverRepository = new ServerRepository(_mockContext.Object);
            var truth = _serverRepository.UpdatePrivacy("WrongIdNotInDbSet", "public");
            Assert.IsFalse(truth);
        }

        [Test]
        public void UpdatePrivacyCorrectlyUpdatesPrivacy()
        {
            _serverRepository = new ServerRepository(_mockContext.Object);
            var privacyLevelBeforeUpdate = _serverRepository.GetAll().FirstOrDefault(x => x.Id == "789317480325316640").Privacy;
            _serverRepository.UpdatePrivacy("789317480325316640", "public");
            var privacyLevelAfterUpdate = _serverRepository.GetAll().FirstOrDefault(x => x.Id == "789317480325316640").Privacy;

            Assert.AreEqual(privacyLevelBeforeUpdate, "private");
            Assert.AreEqual(privacyLevelAfterUpdate, "public");
        }

        [Test]
        public void UpdatePrivacyNotPublicOrPrivate()
        {
            _serverRepository = new ServerRepository(_mockContext.Object);
            var truth = _serverRepository.UpdatePrivacy("789317480325316640", "Random Words Here");
            Assert.IsFalse(truth);
        }

        [Test]
        public void AllServerVMOnlyShowsPublicServers()
        {
            _serverRepository = new ServerRepository(_mockContext.Object);
            var allServersVm = new AllServersVM(_serverRepository);
            var list = allServersVm.AllServerNameAndMemCountContainer();
            var testServer = new Server() { Id = "928010025958510632", ServerPk = 2, Name = "Advanced Innovations", Owner = "false", Icon = "d8f49d144185733c210456853906b631", HasBot = "true", ApproximateMemberCount = 5, OwnerId = "786safd67sdfg657sf6", VerificationLevel = "2", Description = null, PremiumTier = "NONE", ApproximatePresenceCount = 1, Privacy = "public", OnForum = "no", Message = "this is the message" };

            Assert.IsTrue(list?.Count == 1);
            Assert.AreEqual(testServer.Id, list[0].Id);
        }
    }
}
