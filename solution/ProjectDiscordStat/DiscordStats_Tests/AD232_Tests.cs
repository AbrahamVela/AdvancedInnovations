using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System;
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
using RestSharp;
using System.Text.Json;

namespace DiscordStats_Tests
{
    internal class AD232_Tests
    {
        private Mock<DiscordDataDbContext> _mockContext;

        private Mock<DbSet<ServerMembers>> _mockServerMembersDbSet;

        private IServerMemberRepository _serverMemberRepository;

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
            var _members = new List<ServerMembers>
            {
                new ServerMembers{ Id = "1", ServerPk= 1, Members=1, Date= DateTime.Parse("2022-04-01 18:39:12.897") },
                new ServerMembers{ Id = "1", ServerPk= 1, Members=1, Date= DateTime.Parse("2022-04-01 18:39:12.897") },
                new ServerMembers{ Id = "1", ServerPk= 1, Members=1, Date= DateTime.Parse("2022-04-20 18:39:12.897") },
                new ServerMembers{ Id = "2", ServerPk= 2, Members=1, Date= DateTime.Parse("2022-04-01 18:39:12.897") }
            };

            _mockServerMembersDbSet = GetMockDbSet<ServerMembers>(_members.AsQueryable<ServerMembers>());
            _mockContext = new Mock<DiscordDataDbContext>();
            _mockContext.Setup(ctx => ctx.ServerMembers).Returns(_mockServerMembersDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<ServerMembers>()).Returns(_mockServerMembersDbSet.Object);
            _mockContext.Setup(ctx => ctx.Update(It.IsAny<ServerMembers>()))
                        .Callback((ServerMembers s) => { _members.Append(s); })
                        .Returns((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<ServerMembers>)null);
            // do not rely on the return value from Update since it's just null
            _mockContext.Setup(ctx => ctx.SaveChanges())
                        .Returns(0);

        }

        [Test]
        public void Get_Count_Of_records_with_date_should_return_two()
        {
            var handler = new Mock<HttpMessageHandler>();
            _serverMemberRepository = new ServerMemberRepository(_mockContext.Object);
            DiscordService discord = new DiscordService(handler.CreateClientFactory(), null, null, null, null, null, null, _serverMemberRepository);

            var servers = _serverMemberRepository.GetAll().ToList();
            var startDate = DateTime.Parse("2022-04-01");
            var endDate = DateTime.Parse("2022-04-05");
            var datedServers = discord.GetServerUsersByDates(startDate, endDate, "1");

            Assert.AreEqual(2, datedServers.Count);

        }
        [Test]
        public void Get_Count_Of_records_with_StartDate_No_EndDate_should_return_three()
        {
            var handler = new Mock<HttpMessageHandler>();
            _serverMemberRepository = new ServerMemberRepository(_mockContext.Object);
            DiscordService discord = new DiscordService(handler.CreateClientFactory(), null, null, null, null, null, null, _serverMemberRepository);

            var servers = _serverMemberRepository.GetAll().ToList();
            var startDate = DateTime.Parse("2022-04-01");
            DateTime endDate = new();
            var datedServers = discord.GetServerUsersByDates(startDate, endDate, "1");

            Assert.AreEqual(3, datedServers.Count);

        }

        [Test]
        public void Get_Count_Of_records_with_EndDate_No_StartDate_should_return_three()
        {
            var handler = new Mock<HttpMessageHandler>();
            _serverMemberRepository = new ServerMemberRepository(_mockContext.Object);
            DiscordService discord = new DiscordService(handler.CreateClientFactory(), null, null, null, null, null, null, _serverMemberRepository);

            var servers = _serverMemberRepository.GetAll().ToList();
            DateTime startDate = new();
            var endDate = DateTime.Parse("2022-04-22");
            var datedServers = discord.GetServerUsersByDates(startDate, endDate, "1");

            Assert.AreEqual(3, datedServers.Count);

        }
        [Test]
        public void Get_Count_Of_records_with_No_Dates_should_return_one()
        {
            var handler = new Mock<HttpMessageHandler>();
            _serverMemberRepository = new ServerMemberRepository(_mockContext.Object);
            DiscordService discord = new DiscordService(handler.CreateClientFactory(), null, null, null, null, null, null, _serverMemberRepository);

            var servers = _serverMemberRepository.GetAll().ToList();
            DateTime startDate = new();
            DateTime endDate = new();
            var datedServers = discord.GetServerUsersByDates(startDate, endDate, "2");

            Assert.AreEqual(1, datedServers.Count);

        }

    }
}
