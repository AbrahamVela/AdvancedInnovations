using DiscordStats.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using RestSharp;
using System.Net.Http;
using System.Net.Http.Headers;
using Azure.Core;
using System.Net;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;
using DiscordStats.DAL.Abstract;
using DiscordStats.ViewModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using DiscordStats.ViewModels;

namespace DiscordStats.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class ApiController : Controller
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        private readonly IDiscordUserRepository _discordUserRepository;
        private readonly IPresenceRepository _presenceRepository;
        private readonly ILogger<ApiController> _logger;
        private readonly IDiscordService _discord;
        private readonly IDiscordServicesForChannels _discordServicesForChannels;
        private readonly IServerRepository _serverRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IMessageInfoRepository _messageInfoRepository;
        private readonly IVoiceChannelRepository _voiceChannelRepository;
<<<<<<< Updated upstream
       
        public ApiController(ILogger<ApiController> logger, IDiscordUserRepository discordUserRepo, IPresenceRepository presenceRepository, IDiscordService discord, IDiscordServicesForChannels discordServicesForChannels, IServerRepository serverRepository, IChannelRepository channelRepository, IVoiceChannelRepository voiceChannelRepository, IMessageInfoRepository messageInfoRepository)
=======
        private readonly IVoiceStateRepository _voiceStateRepository;
        private readonly IServerMemberRepository _serverMemberRepository;

        public ApiController(ILogger<ApiController> logger, IDiscordUserAndUserWebSiteInfoRepository discordUserRepo, IPresenceRepository presenceRepository, IDiscordService discord, IDiscordServicesForChannels discordServicesForChannels, IServerRepository serverRepository, IChannelRepository channelRepository, IVoiceChannelRepository voiceChannelRepository, IMessageInfoRepository messageInfoRepository, IVoiceStateRepository voiceStateRepository, IServerMemberRepository serverMemberRepository)

>>>>>>> Stashed changes
        {
            _logger = logger;
            _discordUserRepository = discordUserRepo;
            _presenceRepository = presenceRepository;
            _discord = discord;
            _discordServicesForChannels = discordServicesForChannels;
            _serverRepository = serverRepository;
            _channelRepository = channelRepository;
            _messageInfoRepository = messageInfoRepository;
            _voiceChannelRepository = voiceChannelRepository;
<<<<<<< Updated upstream
=======
            _voiceStateRepository = voiceStateRepository;
            _serverMemberRepository = serverMemberRepository;
>>>>>>> Stashed changes
        }


        [HttpPost]
        public async Task<IActionResult> PostUsers(DiscordUser[] users)
        {
            foreach (var user in users)
            {
                var duplicate = false;

                Task.Delay(300).Wait();
                await Task.Run(() =>
                {
                    var allDiscordUsers = _discordUserRepository.GetAll().ToList();

                    for (int i = 0; i < allDiscordUsers.Count(); i++)
                    {
                        if (user.Id == allDiscordUsers[i].Id)
                        {
                            duplicate = true;
                        }
                    }
                    if (!duplicate)
                    {
                        _discordUserRepository.AddOrUpdate(user);
                    }
                });

            }
            return Json("It worked");
        }

        [HttpPost]
        public async Task<IActionResult> PostVoiceChannels(VoiceChannel[] voiceChannels)
        {
            foreach (var channel in voiceChannels)
            {
                channel.Time = DateTime.Now;
            }
            var itWorked = await _discord.VoiceChannelEntryAndUpdateDbCheck(voiceChannels);

            return Json("It Worked");
        }
        [HttpPost]
        public async Task<IActionResult> PostPresence(Presence[] presences)
        {

            foreach (var presence in presences)
            {

                    var itWorked = await _discord.PresenceEntryAndUpdateDbCheck(presences);
                    return Json(itWorked);
                
            }
            return Json("fail");
        }

        public IActionResult GetPresenceDataFromDb()
        {
            _logger.LogInformation("GetPresenceDataFromDb");            
            List<Presence> presences = _presenceRepository.GetAll().ToList(); // .Where(a => a. Privacy == "public").OrderByDescending(m => m.ApproximateMemberCount).Take(5);
            PresenceChartDataVM presenceChartDataVM = new();
            var presencesNameAndCount = presenceChartDataVM.AllPresenceNameListAndCount(presences);

            return Json(new { userPerGame = presencesNameAndCount });
        }





        [HttpPost]
        public async Task<IActionResult> PostServers(Server[] servers)
        {
           


                foreach (var server in servers)
            {

                bool duplicateCount = false;
                var currentServerMembers = _serverMemberRepository.GetAll().Where(s => s.Id == server.Id).ToList();
                foreach (var s in currentServerMembers)
                {
                    if (s.Date.Date == DateTime.Now.Date && s.Date.Hour == DateTime.Now.Hour)
                        duplicateCount = true;
                }
                if (!duplicateCount)
                {
                    ServerMembers newServerCount = new();
                    newServerCount.Date = DateTime.Now;
                    newServerCount.Members = server.ApproximateMemberCount;
                    newServerCount.Id = server.Id;
                    _serverMemberRepository.AddOrUpdate(newServerCount);
                }

                var duplicate = false;

                Task.Delay(300).Wait();
                await Task.Run(() =>
                {
                    var allServers = _serverRepository.GetAll().ToList();
                    var duplicateServer = new Server();
                    for (int i = 0; i < allServers.Count(); i++)
                    {
                        if (server.Id == allServers[i].Id)
                        {
                            duplicate = true;
                            duplicateServer = allServers[i];
                        }
                    }
                    if (!duplicate)
                    {
                        _serverRepository.AddOrUpdate(server);
                    }
                    if (duplicate)
                    {
                        duplicateServer.Name = server.Name;
                        duplicateServer.Id = server.Id;
                        duplicateServer.ApproximateMemberCount = server.ApproximateMemberCount;
                        duplicateServer.ApproximatePresenceCount = server.ApproximatePresenceCount;
                        duplicateServer.Icon = server.Icon;
                        duplicateServer.HasBot = server.HasBot;
                        duplicateServer.OwnerId = server.OwnerId;
                        duplicateServer.PremiumTier = server.PremiumTier;
                        duplicateServer.VerificationLevel = server.VerificationLevel;
                        _serverRepository.AddOrUpdate(duplicateServer);
                    }
                });

            }
            return Json("It worked");
        }


        [HttpPost]
        public async Task<IActionResult> PostChannels(Channel[] channels)
        {
            var itWorked = await _discordServicesForChannels.ChannelEntryAndUpdateDbCheck(channels);

            return Json(itWorked);
        }


        [HttpPost]
        public async Task<IActionResult> PostMessageData(MessageInfo message)
        {
            _messageInfoRepository.AddOrUpdate(message);
            await _channelRepository.UpdateMessageCount(message);
            return Json("It worked");
        }
    }
}
