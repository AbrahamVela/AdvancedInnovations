//using DiscordStats.Controllers;
//using DiscordStats.DAL.Abstract;
//using DiscordStats.DAL.Concrete;
//using DiscordStats.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Moq;
//using Moq.Contrib.HttpClient;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;

//namespace DiscordStats_Tests
//{
//    public class AbrahamSprint5_Tests
//    {
//        private Mock<DiscordDataDbContext> _mockContext;
//        private Mock<DiscordDataDbContext> _mockContext1;

//        private Mock<DbSet<Server>> _mockServerDbSet;
//        private Mock<DbSet<DiscordUserAndUserWebSiteInfo>> _mockDiscordUserAndUserWebSiteDbSet;

//        private IServerRepository _serverRepository;
//        private IDiscordUserAndUserWebSiteInfoRepository _userRepo;

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

//            var user = new List<DiscordUserAndUserWebSiteInfo>
//            {
//                new DiscordUserAndUserWebSiteInfo{Id = "697317543555235840", DiscordUserPk = 1, Username = "Abraham", Servers = "23542345", Avatar="0753a332ab63d2f91971ad57e25123d3", FirstName="A", LastName="V", BirthDate="2022-04-01", Email="a@v.com"},
//                new DiscordUserAndUserWebSiteInfo{Id = "697317543555235841", DiscordUserPk = 1, Username = "Shananay", Servers = "23542345", Avatar="0753a332ab63d2f91971ad57e25123d4", FirstName="S", LastName="V", BirthDate="2022-03-23", Email="s@v.com"}
//            };

//            _mockServerDbSet = GetMockDbSet<Server>(ser.AsQueryable<Server>());
//            _mockContext = new Mock<DiscordDataDbContext>();
//            _mockContext.Setup(ctx => ctx.Servers).Returns(_mockServerDbSet.Object);
//            _mockContext.Setup(ctx => ctx.Set<Server>()).Returns(_mockServerDbSet.Object);
//            _mockContext.Setup(ctx => ctx.Update(It.IsAny<Server>()))
//                        .Callback((Server s) => { ser.Append(s); })
//                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Server>)null);
//            // do not rely on the return value from Update since it's just null
//            _mockContext.Setup(ctx => ctx.SaveChanges())
//                        .Returns(0);

//            _mockDiscordUserAndUserWebSiteDbSet = GetMockDbSet<DiscordUserAndUserWebSiteInfo>(user.AsQueryable<DiscordUserAndUserWebSiteInfo>());
//            _mockContext1 = new Mock<DiscordDataDbContext>();
//            _mockContext1.Setup(ctx => ctx.DiscordUserAndUserWebSiteInfos).Returns(_mockDiscordUserAndUserWebSiteDbSet.Object);
//            _mockContext1.Setup(ctx => ctx.Set<DiscordUserAndUserWebSiteInfo>()).Returns(_mockDiscordUserAndUserWebSiteDbSet.Object);
//            _mockContext1.Setup(ctx => ctx.Update(It.IsAny<DiscordUserAndUserWebSiteInfo>()))
//                        .Callback((DiscordUserAndUserWebSiteInfo u) => { user.Append(u); })
//                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<DiscordUserAndUserWebSiteInfo>)null);
//            // do not rely on the return value from Update since it's just null
//            _mockContext1.Setup(ctx => ctx.SaveChanges())
//                        .Returns(0);
//        }

//        [Test]
//        public async Task Account_ShouldReturnTrueForModelNotNull()
//        {
//            // Arrange
//            var handler = new Mock<HttpMessageHandler>();
//            var _user = new DiscordUserAndUserWebSiteInfoRepository(_mockContext1.Object);
//            var actualJson = System.Text.Json.JsonSerializer.Serialize(_user);
//            var response = new HttpResponseMessage()
//            {
//                Content = new  StringContent(actualJson)
//            };
//            handler.SetupAnyRequest()
//                    .ReturnsAsync(response);

//            var configForSmsApi = new Dictionary<string, string>
//            {
//                {"API:BotToken", "fakeBotToken"},
//            };

//            var configuration = new ConfigurationBuilder()
//            .AddInMemoryCollection(configForSmsApi)
//            .Build();

//            AccountController controller = new AccountController(null, null, configuration, _serverRepository, null, null, null, _user);
//            controller.ControllerContext = new ControllerContext();

//            // Act
//            ViewResult result = (ViewResult)await controller.Account();

//            // Assert
//            Assert.AreEqual(actualJson, result);

//        }
//    }
//}
