//using NUnit.Framework;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Net;
//using System;
//using System.Threading.Tasks;
//using DiscordStats.DAL.Concrete;
//using DiscordStats.Models;
//using Microsoft.Extensions.Configuration;
//using Moq;
//using Moq.Contrib.HttpClient;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using DiscordStats.DAL.Abstract;
//using DiscordStats.ViewModel;
//using DiscordStats.ViewModels;
//using DiscordStats.Controllers;
//using Microsoft.AspNetCore.Mvc;
//using RestSharp;
//using System.Text.Json;
//using Microsoft.Net.Http.Headers;
//using System.Text;

//namespace DiscordStats_Tests
//{
//    internal class AD283_Test
//    {
//        private Mock<DiscordDataDbContext> _mockContextServers;
//        private Mock<DiscordDataDbContext> _mockContext1;
//        private Mock<DiscordDataDbContext> _mockContextPresence;
//        private Mock<DiscordDataDbContext> _mockContextMessageInfo;
//        private Mock<DiscordDataDbContext> _mockContextVoiceStates;
//        private Mock<DiscordDataDbContext> _mockContext5;
//        private Mock<DiscordDataDbContext> _mockContextChannels;
//        private Mock<DiscordDataDbContext> _mockContextStatuses;

//        private Mock<DbSet<Server>> _mockServerDbSet;
//        private Mock<DbSet<DiscordUserAndUserWebSiteInfo>> _mockDiscordUserAndUserWebSiteDbSet;
//        private Mock<DbSet<Presence>> _mockPresenceDbSet;
//        private Mock<DbSet<MessageInfo>> _mockMessageInfoDbSet;
//        private Mock<DbSet<VoiceState>> _mockVoiceStateDbSet;
//        private Mock<DbSet<VoiceChannel>> _mockVoiceChannelDbSet;
//        private Mock<DbSet<Channel>> _mockChannelDbSet;
//        private Mock<DbSet<Status>> _mockStatusDbSet;

//        private IServerRepository _serverRepository;
//        private IDiscordUserAndUserWebSiteInfoRepository _userRepo;
//        private IPresenceRepository _presenceRepo;
//        private IMessageInfoRepository _mockMessageInfoRepository;
//        private IVoiceStateRepository _mockVoiceStateRepository;
//        private IVoiceChannelRepository _voiceChannelRepository;
//        private IChannelRepository _channelRepository;
//        private IStatusRepository _mockStatusRepository;

//        DateTime now = DateTime.Now;


//        private Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> entities) where T : class
//        {
//            var mockSet = new Mock<DbSet<T>>();
//            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.Provider);
//            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.Expression);
//            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.ElementType);
//            //mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());
//            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => entities.GetEnumerator());
//            return mockSet;
//        }

//        [SetUp]
//        public void Setup()
//        {
//            var statuses = new List<Status>
//            { 
//                new Status{UserId = "1", Status1 ="Idle", ServerId="1", CreatedAt = now }
            
//            };

//            var ser = new List<Server>
//             {
//                new Server{Id = "789317480325316646", ServerPk = 7, Name = "Seveth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=25, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="private",OnForum="null",Message="null"},
//                new Server{Id = "789317480325316647", ServerPk = 8, Name = "Eith Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=20, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="private",OnForum="null",Message="null"},
//                new Server{Id = "789317480325316640", ServerPk = 1, Name = "First Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=500, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="null",Message="null"},
//                new Server{Id = "789317480325316642", ServerPk = 3, Name = "Third Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=300, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="null",Message="null"},
//                new Server{Id = "789317480325316643", ServerPk = 4, Name = "Fourth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=200, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="null",Message="null"},
//                new Server{Id = "789317480325316641", ServerPk = 2, Name = "Second Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=400, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="true",Message="bye"},
//                new Server{Id = "789317480325316645", ServerPk = 6, Name = "Sixth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=50, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="private",OnForum="null",Message="null"},
//                new Server{Id = "789317480325316648", ServerPk = 9, Name = "Ninth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=5, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="private",OnForum="null",Message="null"},
//                new Server{Id = "789317480325316644", ServerPk = 5, Name = "Fifth Most Member Count", Owner = "true", Icon = "4e428f7fb657dbf3b733e7b691e56997", HasBot = "true", ApproximateMemberCount=100, OwnerId="null", VerificationLevel="null", Description="null", PremiumTier="null", ApproximatePresenceCount=0, Privacy="public",OnForum="null",Message="null"},
//            };
//            var _VoiceChannels = new List<VoiceChannel>
//            {
//                new VoiceChannel{Id = "156209528969494528", Name ="General", Count=1,GuildId="928010025958510632",Time=DateTime.Parse("2022-04-06 20:17:38.263") }
//            };
//            var user = new List<DiscordUserAndUserWebSiteInfo>
//            {
//                new DiscordUserAndUserWebSiteInfo{Id = "697317543555235840", DiscordUserPk = 1, Username = "Abraham", Servers = "23542345", Avatar="0753a332ab63d2f91971ad57e25123d3", FirstName="A", LastName="V", BirthDate="2022-04-01", Email="a@v.com"},
//                new DiscordUserAndUserWebSiteInfo{Id = "697317543555235841", DiscordUserPk = 2, Username = "Shananay", Servers = "1122334455", Avatar="0753a332ab63d2f91971ad57e25123d4", FirstName="S", LastName="V", BirthDate="2022-03-23", Email="s@v.com"}
//            };
//            var cha = new List<Channel>
//             {
//                new Channel{Id = "789317480803074075", ChannelPk=0, Type = "Guild_Text", Name = "Text Channels", Count = 400, GuildId= "789317480325316646"},
//                new Channel{Id = "12351251452136", ChannelPk=1, Type = "Guild_Voice", Name = "Voice Channels", Count = 220, GuildId= "789317480325316646"}
//            };
//            var pres = new List<Presence>
//            {
//                new Presence{Id="kjn235dgs8", PresencePk=1, ApplicationId="w4fc6", Name="Test Name", Details="4w5g45g", CreatedAt=now, LargeImageId="1252wfg24fg23f4v", SmallImageId="kljm4tkjl23465", ServerId="kjn253nbkjh", UserId="drg43wv343g4f", Image="rf43cv3f3"},
//                new Presence{Id="w456h", PresencePk=2,ApplicationId="awe5yh4w", Name="j5w6hj", Details="srtghr5", CreatedAt=now, LargeImageId="w456ujwr546jh", SmallImageId="w54e6j5ew6j", ServerId="e5w6je56j", UserId="w456j5we6j", Image="w46jw456jsef"},
//                new Presence{Id="134dx52f6", PresencePk=3,ApplicationId="qec6ty4e", Name="Test2 Name", Details="eh5r6b", CreatedAt=now, LargeImageId="wg45w4v5b", SmallImageId="qe45gaerg", ServerId="ghqe5gqew4", UserId="aerg456hg", Image="ghea4rgae4gf"},
//                new Presence{Id="hb35653ggf35", PresencePk=4,ApplicationId="aectxtq", Name="Test3 Name", Details="werh6", CreatedAt=now, LargeImageId="wgwrthe4q5hg", SmallImageId="gsedfagaeg", ServerId="sXQQ32D", UserId="adfg345q", Image="asdrgaerg"},
//                new Presence{Id="2f3c46243vg24", PresencePk=5,ApplicationId="c1 35134", Name="Test4 Name", Details="w456h", CreatedAt=now, LargeImageId="2   3rq453g", SmallImageId="aedrgaerge", ServerId="GRDFATYG", UserId="adrghae45ae", Image="aergaerbeyha"},

//            };
//            var mi = new List<MessageInfo>
//            {
//                new MessageInfo{UserId="123", ChannelId="135", ServerId="312", MessageDataPk=1},
//                new MessageInfo{UserId="456", ChannelId="245", ServerId="412", MessageDataPk=2},
//                new MessageInfo{UserId="789", ChannelId="345", ServerId="512", MessageDataPk=3}
//            };
//            var vs = new List<VoiceState>
//            {
//                new VoiceState{VoiceStatePk=1, UserId="123", ChannelId="135", ServerId="312", CreatedAt=now},
//                new VoiceState{VoiceStatePk=2, UserId="456", ChannelId="245", ServerId="412", CreatedAt=now},
//                new VoiceState{VoiceStatePk=3, UserId="789", ChannelId="345", ServerId="512", CreatedAt=now}

//            };
//            //Statuses
//            _mockStatusDbSet = GetMockDbSet<Status>(statuses.AsQueryable<Status>());
//            _mockContextStatuses = new Mock<DiscordDataDbContext>();
//            _mockContextStatuses.Setup(ctx => ctx.Statuses).Returns(_mockStatusDbSet.Object);
//            _mockContextStatuses.Setup(ctx => ctx.Set<Status>()).Returns(_mockStatusDbSet.Object);
//            _mockContextStatuses.Setup(ctx => ctx.Update(It.IsAny<Status>()))
//                        .Callback((Status p) => { statuses.Append(p); })
//                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Status>)null);
//            _mockContextStatuses.Setup(ctx => ctx.SaveChanges())
//                        .Returns(0);
//            _mockContextStatuses.Setup(x => x.Add(It.IsAny<Status>())).Callback<Status>((s) => statuses.Add(s));

//            //Voice Channels
//            _mockVoiceChannelDbSet = GetMockDbSet<VoiceChannel>(_VoiceChannels.AsQueryable<VoiceChannel>());
//            _mockContext5 = new Mock<DiscordDataDbContext>();
//            _mockContext5.Setup(ctx => ctx.VoiceChannels).Returns(_mockVoiceChannelDbSet.Object);
//            _mockContext5.Setup(ctx => ctx.Set<VoiceChannel>()).Returns(_mockVoiceChannelDbSet.Object);
//            _mockContext5.Setup(ctx => ctx.Update(It.IsAny<VoiceChannel>()))
//                        .Callback((VoiceChannel p) => { _VoiceChannels.Append(p); })
//                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<VoiceChannel>)null);
//            _mockContext5.Setup(ctx => ctx.SaveChanges())
//                        .Returns(0);
//            _mockContext5.Setup(x => x.Add(It.IsAny<VoiceChannel>())).Callback<VoiceChannel>((s) => _VoiceChannels.Add(s));

//            //Channels
//            _mockChannelDbSet = GetMockDbSet<Channel>(cha.AsQueryable<Channel>());
//            _mockContextChannels = new Mock<DiscordDataDbContext>();
//            _mockContextChannels.Setup(ctx => ctx.Channels).Returns(_mockChannelDbSet.Object);
//            _mockContextChannels.Setup(ctx => ctx.Set<Channel>()).Returns(_mockChannelDbSet.Object);
//            _mockContextChannels.Setup(ctx => ctx.Update(It.IsAny<Channel>()))
//                        .Callback((Channel c) => { cha.Append(c); })
//                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Channel>)null);
//            _mockContextChannels.Setup(ctx => ctx.SaveChanges())
//                        .Returns(0);
//            //Servers
//            _mockServerDbSet = GetMockDbSet<Server>(ser.AsQueryable<Server>());
//            _mockContextServers = new Mock<DiscordDataDbContext>();
//            _mockContextServers.Setup(ctx => ctx.Servers).Returns(_mockServerDbSet.Object);
//            _mockContextServers.Setup(ctx => ctx.Set<Server>()).Returns(_mockServerDbSet.Object);
//            _mockContextServers.Setup(ctx => ctx.Update(It.IsAny<Server>()))
//                        .Callback((Server s) => { ser.Append(s); })
//                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Server>)null);
//            _mockContextServers.Setup(ctx => ctx.SaveChanges())
//                        .Returns(0);
//            //DiscordUser
//            _mockDiscordUserAndUserWebSiteDbSet = GetMockDbSet<DiscordUserAndUserWebSiteInfo>(user.AsQueryable<DiscordUserAndUserWebSiteInfo>());
//            _mockContext1 = new Mock<DiscordDataDbContext>();
//            _mockContext1.Setup(ctx => ctx.DiscordUserAndUserWebSiteInfos).Returns(_mockDiscordUserAndUserWebSiteDbSet.Object);
//            _mockContext1.Setup(ctx => ctx.Set<DiscordUserAndUserWebSiteInfo>()).Returns(_mockDiscordUserAndUserWebSiteDbSet.Object);
//            _mockContext1.Setup(ctx => ctx.Update(It.IsAny<DiscordUserAndUserWebSiteInfo>()))
//                        .Callback((DiscordUserAndUserWebSiteInfo u) => { user.Append(u); })
//                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<DiscordUserAndUserWebSiteInfo>)null);
//            _mockContext1.Setup(ctx => ctx.SaveChanges())
//                        .Returns(0);
//            //Presence
//            _mockPresenceDbSet = GetMockDbSet<Presence>(pres.AsQueryable<Presence>());
//            _mockContextPresence = new Mock<DiscordDataDbContext>();
//            _mockContextPresence.Setup(ctx => ctx.Presences).Returns(_mockPresenceDbSet.Object);
//            _mockContextPresence.Setup(ctx => ctx.Set<Presence>()).Returns(_mockPresenceDbSet.Object);
//            _mockContextPresence.Setup(ctx => ctx.Update(It.IsAny<Presence>()))
//                        .Callback((Presence s) => { pres.Append(s); })
//                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Presence>)null);
//            _mockContextPresence.Setup(ctx => ctx.SaveChanges())
//                        .Returns(0);
//            //MessageInfo
//            _mockMessageInfoDbSet = GetMockDbSet<MessageInfo>(mi.AsQueryable<MessageInfo>());
//            _mockContextMessageInfo = new Mock<DiscordDataDbContext>();
//            _mockContextMessageInfo.Setup(ctx => ctx.MessageInfos).Returns(_mockMessageInfoDbSet.Object);
//            _mockContextMessageInfo.Setup(ctx => ctx.Set<MessageInfo>()).Returns(_mockMessageInfoDbSet.Object);
//            _mockContextMessageInfo.Setup(ctx => ctx.Update(It.IsAny<MessageInfo>()))
//                        .Callback((MessageInfo s) => { mi.Append(s); })
//                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<MessageInfo>)null);
//            _mockContextMessageInfo.Setup(ctx => ctx.SaveChanges())
//                        .Returns(0);
//            //Voice States
//            _mockVoiceStateDbSet = GetMockDbSet<VoiceState>(vs.AsQueryable<VoiceState>());
//            _mockContextVoiceStates = new Mock<DiscordDataDbContext>();
//            _mockContextVoiceStates.Setup(ctx => ctx.VoiceStates).Returns(_mockVoiceStateDbSet.Object);
//            _mockContextVoiceStates.Setup(ctx => ctx.Set<VoiceState>()).Returns(_mockVoiceStateDbSet.Object);
//            _mockContextVoiceStates.Setup(ctx => ctx.Update(It.IsAny<VoiceState>()))
//                        .Callback((VoiceState s) => { vs.Append(s); })
//                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<VoiceState>)null);
//            _mockContextVoiceStates.Setup(ctx => ctx.SaveChanges())
//                        .Returns(0);

//            _mockStatusRepository = new StatusRepository(_mockContextStatuses.Object);
//            _serverRepository = new ServerRepository(_mockContextServers.Object);
//            _userRepo = new DiscordUserAndUserWebSiteInfoRepository(_mockContext1.Object);
//            _presenceRepo = new PresenceRepository(_mockContextPresence.Object);
//            _mockMessageInfoRepository = new MessageInfoRepository(_mockContextMessageInfo.Object);
//            _mockVoiceStateRepository = new VoiceStateRepository(_mockContextVoiceStates.Object);
//            _channelRepository = new ChannelRepository(_mockContextChannels.Object);

//        }
//        [Test]
//        public void Check_Post_Users()
//        {
//            _userRepo = new DiscordUserAndUserWebSiteInfoRepository(_mockContext1.Object);
//            var users = _userRepo.GetAll().ToList();
//            var json = JsonSerializer.Serialize(users[1]);
          
//            var content = new StringContent(json, Encoding.UTF8, "application/json");
//                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7228/Api/PostUsers")
//                {
//                    Headers =
//                {
//                    { HeaderNames.Accept, "application/json" }
//                },
//                    Content = content
//                };
//            HttpClient httpClient = new HttpClient();
           
//            var response =  httpClient.Send(httpRequestMessage);
//            Assert.IsFalse(response.IsSuccessStatusCode);
//        }
//        [Test]
//        public void Check_Post_Voice_Channels()
//        {
//            _voiceChannelRepository = new VoiceChannelRepository(_mockContext5.Object);
//            var vc = _voiceChannelRepository.GetAll().ToList();
//            var json = JsonSerializer.Serialize(vc[0]);
//            var content = new StringContent(json, Encoding.UTF8, "application/json");
//            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7228/Api/PostVoiceChannels")
//            {
//                Headers =
//                {
//                    { HeaderNames.Accept, "application/json" }
//                },
//                Content = content
//            };
//            HttpClient httpClient = new HttpClient();

//            var response = httpClient.Send(httpRequestMessage);
//            Assert.IsFalse(response.IsSuccessStatusCode);
//        }
//        [Test]
//        public void Check_Post_Voice_Presence()
//        {
//            _presenceRepo = new PresenceRepository(_mockContextPresence.Object);
//            var presences = _presenceRepo.GetAll().ToList();
//            var json = JsonSerializer.Serialize(presences[0]);
//            var content = new StringContent(json, Encoding.UTF8, "application/json");
//            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7228/Api/PostPresence")
//            {
//                Headers =
//                {
//                    { HeaderNames.Accept, "application/json" }
//                },
//                Content = content
//            };
//            HttpClient httpClient = new HttpClient();

//            var response = httpClient.Send(httpRequestMessage);
//            Assert.IsFalse(response.IsSuccessStatusCode);
//        }
//        [Test]
//        public void Check_Post_Servers()
//        {
//            _serverRepository = new ServerRepository(_mockContextServers.Object);
//            var presences = _serverRepository.GetAll().ToList();
//            var json = JsonSerializer.Serialize(presences[0]);
//            var content = new StringContent(json, Encoding.UTF8, "application/json");
//            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7228/Api/PostPresence")
//            {
//                Headers =
//                {
//                    { HeaderNames.Accept, "application/json" }
//                },
//                Content = content
//            };
//            HttpClient httpClient = new HttpClient();

//            var response = httpClient.Send(httpRequestMessage);
//            Assert.IsFalse(response.IsSuccessStatusCode);
//        }
//        [Test]
//        public void Check_Post_Servers_Channels()
//        {
//            _channelRepository = new ChannelRepository(_mockContextChannels.Object);
//            var items = _channelRepository.GetAll().ToList();
//            var json = JsonSerializer.Serialize(items[0]);
//            var content = new StringContent(json, Encoding.UTF8, "application/json");
//            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7228/Api/PostChannels")
//            {
//                Headers =
//                {
//                    { HeaderNames.Accept, "application/json" }
//                },
//                Content = content
//            };
//            HttpClient httpClient = new HttpClient();

//            var response = httpClient.Send(httpRequestMessage);
//            Assert.IsFalse(response.IsSuccessStatusCode);
//        }
//        [Test]
//        public void Check_Post_Message_Data()
//        {
//            _mockMessageInfoRepository = new MessageInfoRepository(_mockContextMessageInfo.Object);
//            var items = _mockMessageInfoRepository.GetAll().ToList();
//            var json = JsonSerializer.Serialize(items[0]);
//            var content = new StringContent(json, Encoding.UTF8, "application/json");
//            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7228/Api/PostMessageData")
//            {
//                Headers =
//                {
//                    { HeaderNames.Accept, "application/json" }
//                },
//                Content = content
//            };
//            HttpClient httpClient = new HttpClient();

//            var response = httpClient.Send(httpRequestMessage);
//            Assert.IsFalse(response.IsSuccessStatusCode);
//        }
//        [Test]
//        public void Check_Voice_States()
//        {
//            _mockVoiceStateRepository = new VoiceStateRepository(_mockContextVoiceStates.Object);
//            var items = _mockVoiceStateRepository.GetAll().ToList();
//            var json = JsonSerializer.Serialize(items[0]);
//            var content = new StringContent(json, Encoding.UTF8, "application/json");
//            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7228/Api/PostVoiceStates")
//            {
//                Headers =
//                {
//                    { HeaderNames.Accept, "application/json" }
//                },
//                Content = content
//            };
//            HttpClient httpClient = new HttpClient();

//            var response = httpClient.Send(httpRequestMessage);
//            Assert.IsFalse(response.IsSuccessStatusCode);
//        }
//        [Test]
//        public void Check_Statuses()
//        {
//            _mockStatusRepository = new StatusRepository(_mockContextStatuses.Object);
//            var items = _mockStatusRepository.GetAll().ToList();
//            var json = JsonSerializer.Serialize(items[0]);
//            var content = new StringContent(json, Encoding.UTF8, "application/json");
//            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7228/Api/PostStatuses")
//            {
//                Headers =
//                {
//                    { HeaderNames.Accept, "application/json" }
//                },
//                Content = content
//            };
//            HttpClient httpClient = new HttpClient();

//            var response = httpClient.Send(httpRequestMessage);
//            Assert.IsFalse(response.IsSuccessStatusCode);
//        }
//    }
//}
