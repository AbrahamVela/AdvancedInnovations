using DiscordStats.Controllers;
using DiscordStats.DAL.Abstract;
using DiscordStats.DAL.Concrete;
using DiscordStats.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiscordStats_Tests
{
    internal class EthanSprint7_Tests
    {
        private Mock<DiscordDataDbContext> _mockContext;
        private Mock<DiscordDataDbContext> _mockContext1;
        private Mock<DiscordDataDbContext> _mockContext2;
        private Mock<DiscordDataDbContext> _mockContext3;
        private Mock<DiscordDataDbContext> _mockContext4;


        private Mock<DbSet<Server>> _mockServerDbSet;
        private Mock<DbSet<DiscordUserAndUserWebSiteInfo>> _mockDiscordUserAndUserWebSiteDbSet;
        private Mock<DbSet<Status>> _mockStatusDbSet;
        private Mock<DbSet<MessageInfo>> _mockMessageInfoDbSet;
        private Mock<DbSet<Presence>> _mockPresenceDbSet;

        private IServerRepository _serverRepository;
        private IDiscordUserAndUserWebSiteInfoRepository _userRepo;
        private IStatusRepository _statusRepository;
        private IMessageInfoRepository _messageInfoRepo;
        private IPresenceRepository _presenceRepo;

        DateTime now = DateTime.Now;


        private Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> entities) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.ElementType);
            //mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => entities.GetEnumerator());
            return mockSet;
        }

        [SetUp]
        public void Setup()
        {
            var ser = new List<Server>
             {
                new Server{Id = "789317480325316646", ServerPk = 7, Name = "Seveth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=25, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="private",OnForum="null",Message="null"},
                new Server{Id = "789317480325316647", ServerPk = 8, Name = "Eith Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=20, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="private",OnForum="null",Message="null"},
                new Server{Id = "789317480325316640", ServerPk = 1, Name = "First Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=500, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="null",Message="null"},
                new Server{Id = "789317480325316642", ServerPk = 3, Name = "Third Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=300, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="null",Message="null"},
                new Server{Id = "789317480325316643", ServerPk = 4, Name = "Fourth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=200, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="null",Message="null"},
                new Server{Id = "789317480325316641", ServerPk = 2, Name = "Second Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=400, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="true",Message="bye"},
                new Server{Id = "789317480325316645", ServerPk = 6, Name = "Sixth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=50, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="private",OnForum="null",Message="null"},
                new Server{Id = "789317480325316648", ServerPk = 9, Name = "Ninth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=5, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="private",OnForum="null",Message="null"},
                new Server{Id = "789317480325316644", ServerPk = 5, Name = "Fifth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=100, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="null",Message="null"},
            };

            var user = new List<DiscordUserAndUserWebSiteInfo>
            {
                new DiscordUserAndUserWebSiteInfo{Id = "697317543555235840", DiscordUserPk = 1, Username = "Abraham", Servers = "23542345", Avatar="0753a332ab63d2f91971ad57e25123d3", Role="Mod", FirstName="A", LastName="V", BirthDate="2022-04-01", Email="a@v.com"},
                new DiscordUserAndUserWebSiteInfo{Id = "697317543555235841", DiscordUserPk = 2, Username = "Shananay", Servers = "1122334455", Avatar="0753a332ab63d2f91971ad57e25123d4", Role="N/A",FirstName="S", LastName="V", BirthDate="2022-03-23", Email="s@v.com"}
            };

            var statuses = new List<Status>
            {
                new Status{StatusPk=1, UserId="123", ServerId="789317480325316646", CreatedAt=now},
                new Status{StatusPk=2, UserId="456", ServerId="789317480325316646", CreatedAt=now},
                new Status{StatusPk=3, UserId="789", ServerId="512", CreatedAt=now}
            };
            var messageInfos = new List<MessageInfo>
            {
                new MessageInfo{MessageDataPk=1, UserId="123", ServerId="789317480325316646", ChannelId="664", Emojis="1f440,", Reactions="1f440,", ReactionUrl=null,CreatedAt=now.ToString()},
                new MessageInfo{MessageDataPk=2, UserId="456", ServerId="789317480325316646", ChannelId="664", Emojis="1f440,", Reactions="1f440,", ReactionUrl=null,CreatedAt=now.ToString()},
                new MessageInfo{MessageDataPk=3, UserId="789", ServerId="512", ChannelId="664", Emojis="1f440,", Reactions="1f440,", ReactionUrl=null,CreatedAt=now.ToString()}
            };
            var pres = new List<Presence>
            {
                new Presence{Id="kjn235dgs8", PresencePk=1, ApplicationId="w4fc6", Name="Test Name", Details="4w5g45g", CreatedAt=now, LargeImageId="1252wfg24fg23f4v", SmallImageId="kljm4tkjl23465", ServerId="kjn253nbkjh", UserId="drg43wv343g4f", Image="rf43cv3f3", ActivityType="Playing"},
                new Presence{Id="w456h", PresencePk=2,ApplicationId="awe5yh4w", Name="j5w6hj", Details="srtghr5", CreatedAt=now, LargeImageId="w456ujwr546jh", SmallImageId="w54e6j5ew6j", ServerId="e5w6je56j", UserId="w456j5we6j", Image="w46jw456jsef", ActivityType="Playing"},
                new Presence{Id="134dx52f6", PresencePk=3,ApplicationId="qec6ty4e", Name="Test2 Name", Details="eh5r6b", CreatedAt=now, LargeImageId="wg45w4v5b", SmallImageId="qe45gaerg", ServerId="ghqe5gqew4", UserId="aerg456hg", Image="ghea4rgae4gf", ActivityType="Playing"},
                new Presence{Id="hb35653ggf35", PresencePk=4,ApplicationId="aectxtq", Name="Test3 Name", Details="werh6", CreatedAt=now, LargeImageId="wgwrthe4q5hg", SmallImageId="gsedfagaeg", ServerId="sXQQ32D", UserId="adfg345q", Image="asdrgaerg", ActivityType="Listening"},
                new Presence{Id="2f3c46243vg24", PresencePk=5,ApplicationId="c1 35134", Name="Test4 Name", Details="w456h", CreatedAt=now, LargeImageId="2   3rq453g", SmallImageId="aedrgaerge", ServerId="GRDFATYG", UserId="adrghae45ae", Image="aergaerbeyha", ActivityType="Listening"},

            };

            _mockServerDbSet = GetMockDbSet<Server>(ser.AsQueryable<Server>());
            _mockContext = new Mock<DiscordDataDbContext>();
            _mockContext.Setup(ctx => ctx.Servers).Returns(_mockServerDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<Server>()).Returns(_mockServerDbSet.Object);
            _mockContext.Setup(ctx => ctx.Update(It.IsAny<Server>()))
                        .Callback((Server s) => { ser.Append(s); })
                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Server>)null);
            // do not rely on the return value from Update since it's just null
            _mockContext.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);

            _mockDiscordUserAndUserWebSiteDbSet = GetMockDbSet<DiscordUserAndUserWebSiteInfo>(user.AsQueryable<DiscordUserAndUserWebSiteInfo>());
            _mockContext1 = new Mock<DiscordDataDbContext>();
            _mockContext1.Setup(ctx => ctx.DiscordUserAndUserWebSiteInfos).Returns(_mockDiscordUserAndUserWebSiteDbSet.Object);
            _mockContext1.Setup(ctx => ctx.Set<DiscordUserAndUserWebSiteInfo>()).Returns(_mockDiscordUserAndUserWebSiteDbSet.Object);
            _mockContext1.Setup(ctx => ctx.Update(It.IsAny<DiscordUserAndUserWebSiteInfo>()))
                        .Callback((DiscordUserAndUserWebSiteInfo u) => { user.Append(u); })
                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<DiscordUserAndUserWebSiteInfo>)null);
            // do not rely on the return value from Update since it's just null
            _mockContext1.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);

            _mockStatusDbSet = GetMockDbSet<Status>(statuses.AsQueryable<Status>());
            _mockContext2 = new Mock<DiscordDataDbContext>();
            _mockContext2.Setup(ctx => ctx.Statuses).Returns(_mockStatusDbSet.Object);
            _mockContext2.Setup(ctx => ctx.Set<Status>()).Returns(_mockStatusDbSet.Object);
            _mockContext2.Setup(ctx => ctx.Update(It.IsAny<Status>()))
                        .Callback((Status s) => { statuses.Append(s); })
                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Status>)null);
            // do not rely on the return value from Update since it's just null
            _mockContext2.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);

            _mockMessageInfoDbSet = GetMockDbSet<MessageInfo>(messageInfos.AsQueryable<MessageInfo>());
            _mockContext3 = new Mock<DiscordDataDbContext>();
            _mockContext3.Setup(ctx => ctx.MessageInfos).Returns(_mockMessageInfoDbSet.Object);
            _mockContext3.Setup(ctx => ctx.Set<MessageInfo>()).Returns(_mockMessageInfoDbSet.Object);
            _mockContext3.Setup(ctx => ctx.Update(It.IsAny<MessageInfo>()))
                        .Callback((MessageInfo s) => { messageInfos.Append(s); })
                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<MessageInfo>)null);
            // do not rely on the return value from Update since it's just null
            _mockContext3.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);

            _mockPresenceDbSet = GetMockDbSet<Presence>(pres.AsQueryable<Presence>());
            _mockContext4 = new Mock<DiscordDataDbContext>();
            _mockContext4.Setup(ctx => ctx.Presences).Returns(_mockPresenceDbSet.Object);
            _mockContext4.Setup(ctx => ctx.Set<Presence>()).Returns(_mockPresenceDbSet.Object);
            _mockContext4.Setup(ctx => ctx.Update(It.IsAny<Presence>()))
                        .Callback((Presence s) => { pres.Append(s); })
                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Presence>)null);
            // do not rely on the return value from Update since it's just null
            _mockContext4.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);


            _serverRepository = new ServerRepository(_mockContext.Object);
            _userRepo = new DiscordUserAndUserWebSiteInfoRepository(_mockContext1.Object);
            _statusRepository = new StatusRepository(_mockContext2.Object);
            _messageInfoRepo = new MessageInfoRepository(_mockContext3.Object);
            _presenceRepo = new PresenceRepository(_mockContext4.Object);

        }



        [Test]
        public async Task GetPresencesFromDatabaseReturnsCorrectData()
        {
            StatsController statsController = new StatsController(null, _userRepo, null, _presenceRepo, null, _serverRepository, null, _messageInfoRepo, null, null, null);
            statsController.ControllerContext = new ControllerContext();

            // Act
            JsonResult result = (JsonResult)await statsController.GetPresencesFromDatabaseT("GRDFATYG", "Test4 Name");
            var serializedResult = JsonConvert.SerializeObject(result.Value);
            var correctList = new List<Presence> { new Presence() { Id = "2f3c46243vg24", PresencePk = 5, ApplicationId = "c1 35134", Name = "Test4 Name", Details = "w456h", CreatedAt = now, LargeImageId = "2   3rq453g", SmallImageId = "aedrgaerge", ServerId = "GRDFATYG", UserId = "adrghae45ae", Image = "aergaerbeyha", ActivityType = "Listening" } };
            var correctSerializedResult = JsonConvert.SerializeObject(correctList);

            // Assert
            Assert.AreEqual(serializedResult, correctSerializedResult);
        }

        [Test]
        public async Task GetAllPresencesFromDatabaseReturnsCorrectData()
        {
            StatsController statsController = new StatsController(null, _userRepo, null, _presenceRepo, null, _serverRepository, null, _messageInfoRepo, null, null, null);
            statsController.ControllerContext = new ControllerContext();

            // Act
            JsonResult result = (JsonResult)await statsController.GetAllPresencesFromDatabaseT("GRDFATYG");
            var serializedResult = JsonConvert.SerializeObject(result.Value); 
            var correctList = new List<Presence> { new Presence() { Id = "2f3c46243vg24", PresencePk = 5, ApplicationId = "c1 35134", Name = "Test4 Name", Details = "w456h", CreatedAt = now, LargeImageId = "2   3rq453g", SmallImageId = "aedrgaerge", ServerId = "GRDFATYG", UserId = "adrghae45ae", Image = "aergaerbeyha", ActivityType = "Listening" } };
            var correctSerializedResult = JsonConvert.SerializeObject(correctList);

            // Assert
            Assert.AreEqual(serializedResult, correctSerializedResult);
        }

        [Test]
        public void GetMessageInfoFromDatabaseReturnsCorrectData()
        {
            StatsController statsController = new StatsController(null, _userRepo, null, _presenceRepo, null, _serverRepository, null, _messageInfoRepo, null, null, null);
            statsController.ControllerContext = new ControllerContext();

            // Act
            JsonResult result = (JsonResult)statsController.GetMessageInfoFromDatabase("512");
            var serializedResult = JsonConvert.SerializeObject(result.Value);
            var correctList = new List<MessageInfo> { new MessageInfo() { MessageDataPk = 3, UserId = "789", ServerId = "512", ChannelId = "664", Emojis = "1f440,", Reactions = "1f440,", ReactionUrl = null, CreatedAt = now.ToString() } };
            var correctSerializedResult = JsonConvert.SerializeObject(correctList);

            // Assert
            Assert.AreEqual(serializedResult, correctSerializedResult);
        }

        [Test]
        public async Task GetUsersFromDatabaseReturnsCorrectData()
        {
            StatsController statsController = new StatsController(null, _userRepo, null, _presenceRepo, null, _serverRepository, null, _messageInfoRepo, null, null, null);
            statsController.ControllerContext = new ControllerContext();

            // Act
            JsonResult result = (JsonResult)await statsController.GetUsersFromDatabaseT("1122334455");
            var serializedResult = JsonConvert.SerializeObject(result.Value);
            var correctList = new List<DiscordUserAndUserWebSiteInfo> { new DiscordUserAndUserWebSiteInfo() { Id = "697317543555235841", DiscordUserPk = 2, Username = "Shananay", Servers = "1122334455", Avatar = "0753a332ab63d2f91971ad57e25123d4", Role = "N/A", FirstName = "S", LastName = "V", BirthDate = "2022-03-23", Email = "s@v.com" } };
            var correctSerializedResult = JsonConvert.SerializeObject(correctList);

            // Assert
            Assert.AreEqual(serializedResult, correctSerializedResult);
        }
    }
}
