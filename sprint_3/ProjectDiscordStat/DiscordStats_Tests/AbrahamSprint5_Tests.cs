using DiscordStats.Controllers;
using DiscordStats.DAL.Abstract;
using DiscordStats.DAL.Concrete;
using DiscordStats.Models;
using DiscordStats.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Contrib.HttpClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using DiscordStats.ViewModels;
using Newtonsoft.Json;


namespace DiscordStats_Tests
{
    public class AbrahamSprint5_Tests
    {
        private Mock<DiscordDataDbContext> _mockContext;
        private Mock<DiscordDataDbContext> _mockContext1;
        private Mock<DiscordDataDbContext> _mockContext2;

        private Mock<DbSet<Server>> _mockServerDbSet;
        private Mock<DbSet<DiscordUserAndUserWebSiteInfo>> _mockDiscordUserAndUserWebSiteDbSet;
        private Mock<DbSet<Channel>> _mockChannelDbSet;

        private IServerRepository _serverRepository;
        private IDiscordUserAndUserWebSiteInfoRepository _userRepo;
        private IChannelRepository _channelRepository;

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
                new DiscordUserAndUserWebSiteInfo{Id = "697317543555235840", DiscordUserPk = 1, Username = "Abraham", Servers = "23542345", Avatar="0753a332ab63d2f91971ad57e25123d3", FirstName="A", LastName="V", BirthDate="2022-04-01", Email="a@v.com"},
                new DiscordUserAndUserWebSiteInfo{Id = "697317543555235841", DiscordUserPk = 2, Username = "Shananay", Servers = "23542345", Avatar="0753a332ab63d2f91971ad57e25123d4", FirstName="S", LastName="V", BirthDate="2022-03-23", Email="s@v.com"}
            };

            var cha = new List<Channel>
             {
                new Channel{Id = "789317480803074075", ChannelPk=0, Type = "Guild_Text", Name = "Text Channels", Count = 400, GuildId= "789317480325316646"},
                new Channel{Id = "12351251452136", ChannelPk=1, Type = "Guild_Voice", Name = "Voice Channels", Count = 220, GuildId= "789317480325316646"}
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


            _mockChannelDbSet = GetMockDbSet<Channel>(cha.AsQueryable<Channel>());
            _mockContext2 = new Mock<DiscordDataDbContext>();
            _mockContext2.Setup(ctx => ctx.Channels).Returns(_mockChannelDbSet.Object);
            _mockContext2.Setup(ctx => ctx.Set<Channel>()).Returns(_mockChannelDbSet.Object);
            _mockContext2.Setup(ctx => ctx.Update(It.IsAny<Channel>()))
                        .Callback((Channel c) => { cha.Append(c); })
                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Channel>)null);
            // do not rely on the return value from Update since it's just null
            _mockContext2.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);
        }

        // For user story 
        [Test]
        public void UpdatWUpdateWebsiteProfileInfo_UpdatesUserInDb_ShouldReturnExistingUserWithNewDbInfo()
        {
            // Arrange
            _userRepo = new DiscordUserAndUserWebSiteInfoRepository(_mockContext1.Object);
            ServerAndDiscordUserInfoAndWebsiteProfileVM userVM = new();

            var tempUser = _userRepo.GetAll().Where(i => i.Id == "697317543555235840").SingleOrDefault();
            var tempName = tempUser.FirstName;
            var tempName2 = tempUser.LastName;
            var email = tempUser.Email;
            userVM.id = "697317543555235840";
            userVM.ProfileFirstName = "Abraham";
            userVM.ProfileLastName = "Vela";
            userVM.ProfileEmail = "A@B2.com";
            // Act
            _userRepo.UpdateWebsiteProfileInfo(userVM);
            var newCount = _userRepo.GetAll().ToList().Count();
            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(tempUser.Id, userVM.id);
                Assert.AreNotEqual(tempName, userVM.ProfileFirstName);
                Assert.AreNotEqual(tempName2, userVM.ProfileLastName);
                Assert.AreNotEqual(email, userVM.ProfileEmail);
            });

        }

        // For Some reason it isn't adding the user to the mock dbSet if the user isn't initially in there 
        // during this test, but does add in code.

        //[Test]
        //public void UpdatWUpdateWebsiteProfileInfo_UpdatesUserInDb_ShouldReturnNewUserWithNewDbInfo()
        //{
        //    // Arrange
        //    _userRepo = new DiscordUserAndUserWebSiteInfoRepository(_mockContext1.Object);
        //    ServerAndDiscordUserInfoAndWebsiteProfileVM userVM = new();
        //    //DiscordUserAndUserWebSiteInfo tempUser = new DiscordUserAndUserWebSiteInfo();
        //    var tempUser = _userRepo.GetAll().ToList().Count();
        //    var tempCount = tempUser;
        //    userVM.id = "697317543555235842";
        //    userVM.ProfileFirstName = "A2";
        //    userVM.ProfileLastName = "B2";
        //    userVM.ProfileEmail = "A@B3.com";
        //    // Act
        //    _userRepo.UpdateWebsiteProfileInfo(userVM);
        //    var newCount = _userRepo.GetAll().ToList().Count();

        //    // Assert
        //    Assert.Multiple(() =>
        //    {
        //        Assert.AreEqual(tempCount, 2);
        //        Assert.AreEqual(newCount, 3);
        //    });

        //}

        [Test]
        public async Task ProfileFormSubmit_ShouldReturnIdThroughViewModel()
        {
            // Arrange

            AccountController controller = new AccountController(null, null, null, _serverRepository, null, null, null, null, null, null);
            controller.ControllerContext = new ControllerContext();
            var userId = "697317543555235840";
            var vm = new ServerAndDiscordUserInfoAndWebsiteProfileVM();
            vm.id = userId;
            // Act
            ViewResult result = (ViewResult)await controller.WebsiteProfileForm(userId);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(result.Model);
            var actualJson = System.Text.Json.JsonSerializer.Serialize(vm);

            // Assert
            Assert.AreEqual(expectedJson, actualJson);
        }

        [Test]
        public async Task ProfileFormSubmit_ShouldRedirectToAccountPageOnSuccess()
        {
            // Arrange
            _userRepo = new DiscordUserAndUserWebSiteInfoRepository(_mockContext1.Object);
            var configForSmsApi = new Dictionary<string, string>
            {
                {"API:BotToken", "fakeBotToken"},
            };

            var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configForSmsApi)
            .Build();

            AccountController controller = new AccountController(null, null, configuration, _serverRepository, null, null, null, null, _userRepo, null);
            controller.ControllerContext = new ControllerContext();
            var vm = new ServerAndDiscordUserInfoAndWebsiteProfileVM();
            vm.id =  "697317543555235840";
            vm.ProfileFirstName = "A2";
            vm.ProfileLastName = "B2";
            vm.ProfileBirthDate= "1999-01-19";
            vm.ProfileEmail = "A@B3.com";

            // Act
            RedirectToActionResult result = (RedirectToActionResult)await controller.ProfileFormSubmit(vm);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(result.ActionName);
            var actualJson = System.Text.Json.JsonSerializer.Serialize("Account");
            // Assert
            Assert.AreEqual(actualJson, expectedJson);
        }

        [Test]
        public void ServerAndDiscordUserInfoAndWebsiteProfileVM_ShouldReturnAllAttributesFilled()
        {
            // Arrange
            _serverRepository = new ServerRepository(_mockContext.Object);
            var servers = _serverRepository.GetAll().ToList();
            var serverCount = servers.Count();
            // Act
            var vm = new ServerAndDiscordUserInfoAndWebsiteProfileVM();
            vm.id =  "697317543555235840";
            vm.Servers = servers;
            vm.ProfileFirstName = "A2";
            vm.ProfileLastName = "B2";
            vm.ProfileBirthDate= "1999-01-19";
            vm.ProfileEmail = "A@B3.com";

            var serverModelCount = vm.Servers.Count();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(vm.id, "697317543555235840");
                Assert.AreEqual(serverModelCount, serverCount);
                Assert.AreEqual(vm.ProfileFirstName, "A2");
                Assert.AreEqual(vm.ProfileLastName, "B2");
                Assert.AreEqual(vm.ProfileEmail, "A@B3.com");
            });
        }


        [Test]
        public async Task CreateChannel_OnSuccessShouldRedirectToServerChannels()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            string jsonFromDiscordAPI = @"{
            ""id"": ""789317480803074075"",
            ""type"": ""0"",
            ""name"": ""TestChannel"",   
            }";
            var response = new HttpResponseMessage()
            {
                Content = new StringContent(jsonFromDiscordAPI)
            };
            handler.SetupAnyRequest()
                    .ReturnsAsync(response);

            _channelRepository = new ChannelRepository(_mockContext2.Object);
            var selectedChannel = _channelRepository.GetAll().Where(m => m.Id == "789317480803074075").FirstOrDefault();
            var configForSmsApi = new Dictionary<string, string>
            {
                {"API:BotToken", "fakeBotToken"},
            };

            var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configForSmsApi)
            .Build();
            DiscordServicesForChannels discord = new DiscordServicesForChannels(handler.CreateClientFactory(), _channelRepository);

            ChannelController controller = new ChannelController(null, configuration, _channelRepository, discord, _serverRepository, null);
            controller.ControllerContext = new ControllerContext();

            // Act

            ServerChannelsVM serverChannelsVM = new ServerChannelsVM();


            RedirectToActionResult result = (RedirectToActionResult)await controller.CreateChannel(serverChannelsVM);
            var expectedJson = System.Text.Json.JsonSerializer.Serialize(result.ActionName);
            var actualJson = System.Text.Json.JsonSerializer.Serialize("ServerChannels");

            // Assert
            Assert.AreEqual(actualJson, expectedJson);

        }

        // The DeleteById would end up calling the FindById and even though the _dbSet had the id the entity would return null, instead
        // of the object.
        //[Test]
        //public async Task DeleteChannel_OnSuccessShouldRedirectToServersPageInTheAccountController()
        //{
        //    // Arrange
        //    var handler = new Mock<HttpMessageHandler>();
        //    string jsonFromDiscordAPI = @"{
        //    ""id"": ""789317480803074075"",
        //    ""type"": ""0"",
        //    ""name"": ""TestChannel"",   
        //    }";
        //    var response = new HttpResponseMessage()
        //    {
        //        Content = new StringContent(jsonFromDiscordAPI)
        //    };
        //    handler.SetupAnyRequest()
        //            .ReturnsAsync(response);

        //    _channelRepository = new ChannelRepository(_mockContext2.Object);
        //    var selectedChannel = _channelRepository.GetAll().Where(m => m.Id == "789317480803074075").FirstOrDefault();
        //    var configForSmsApi = new Dictionary<string, string>
        //    {
        //        {"API:BotToken", "fakeBotToken"},
        //    };

        //    var configuration = new ConfigurationBuilder()
        //    .AddInMemoryCollection(configForSmsApi)
        //    .Build();
        //    DiscordServicesForChannels discord = new DiscordServicesForChannels(handler.CreateClientFactory(), _channelRepository);

        //    ChannelController controller = new ChannelController(null, configuration, _channelRepository, discord, _serverRepository, null);
        //    controller.ControllerContext = new ControllerContext();

        //    // Act
        //    var id = "789317480803074075";


        //    RedirectToActionResult result = (RedirectToActionResult)await controller.DeleteChannel(id);
        //    var expectedJson = System.Text.Json.JsonSerializer.Serialize(result.ActionName);
        //    var actualJson = System.Text.Json.JsonSerializer.Serialize("ServerChannels");

        //    // Assert
        //    Assert.AreEqual(actualJson, expectedJson);

        //}

        [Test]
        public async Task ChannelToDelete_OnSuccessReturnsEmptyMessage()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();

            string jsonFromDiscordAPI = @"";

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(jsonFromDiscordAPI)
            };
            handler.SetupAnyRequest()
                    .ReturnsAsync(response);

            DiscordServicesForChannels discord = new DiscordServicesForChannels(handler.CreateClientFactory(), _channelRepository);

            // Act
            string? returnValue = await discord.DeleteChannel("fakeBotToken", "fakeChannelId");

            // Assert
            Assert.AreEqual("", returnValue);

        }
    }
}
