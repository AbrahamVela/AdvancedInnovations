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
using System.Text;

namespace DiscordStats.Controllers
{
    public class StatsController : Controller
    {

        private readonly IDiscordUserAndUserWebSiteInfoRepository _userRepository;
        private readonly IPresenceRepository _presenceRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiController> _logger;
        private readonly IDiscordService _discord;
        private readonly IServerRepository _serverRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IMessageInfoRepository _messageInfoRepository;
        private readonly IVoiceStateRepository _voiceStateRepository;
        private readonly IServerMemberRepository _serverMemberRepository;
        private readonly IStatusRepository _statusRepository;

        public StatsController(ILogger<ApiController> logger, IDiscordUserAndUserWebSiteInfoRepository discordUserRepo, IConfiguration config, IPresenceRepository presenceRepository, IDiscordService discord, IServerRepository serverRepository, IChannelRepository channelRepository, IMessageInfoRepository messageInfoRepository, IVoiceStateRepository voiceStateRepository, IServerMemberRepository serverMemberRepository, IStatusRepository statusRepository)
        {
            _logger = logger;
            _userRepository = discordUserRepo;
            _presenceRepository = presenceRepository;
            _discord = discord;
            _serverRepository = serverRepository;
            _channelRepository = channelRepository;
            _messageInfoRepository = messageInfoRepository;
            _voiceStateRepository = voiceStateRepository;
            _statusRepository = statusRepository;
            _serverMemberRepository = serverMemberRepository;
            _configuration = config;
        }

        public async Task<IActionResult> ServerStats(string ServerId)
        {

            bool authenticated = false;
            var usersInGuild = await _discord.GetCurrentGuildUsers(_configuration["API:BotToken"], ServerId);
            if (usersInGuild == null)
            {
                return RedirectToAction("Account", "Account");
            }
            var name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            foreach (var u in usersInGuild)
            {
                if (u.user.UserName == name)
                    authenticated = true;
            }
            if (authenticated)
            {

                if (_serverRepository.GetAll().Any(c => c.Id == ServerId))
                    return View((object)ServerId);
                else
                    return View();
            }
            else
                return RedirectToAction("Account", "Account");

        }
        [HttpGet]
        public async Task<IActionResult> GetVoiceStatesFromDatabase(string ServerId)
        {
            bool authenticated = false;
            var usersInGuild = await _discord.GetCurrentGuildUsers(_configuration["API:BotToken"], ServerId);
            if (usersInGuild == null)
            {
                return RedirectToAction("Account", "Account");
            }
            var name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            foreach (var u in usersInGuild)
            {
                if (u.user.UserName == name)
                    authenticated = true;
            }
            if (authenticated)
            {
                return Json(_voiceStateRepository.GetAll().Where(s => s.ServerId == ServerId).ToList());
            }
            else
                return RedirectToAction("Account", "Account");
        }
        [HttpGet] 
        public async Task<IActionResult> GetVoiceStatesFromDatabaseForGraphAndDownload(string formatWithServerId)
        {
            var formatWithServerIdSplitted = formatWithServerId.Split(":");
            bool authenticated = false;
            var usersInGuild = await _discord.GetCurrentGuildUsers(_configuration["API:BotToken"], formatWithServerIdSplitted[1]);
            if (usersInGuild == null)
            {
                return RedirectToAction("Account", "Account");
            }
            var name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            foreach (var u in usersInGuild)
            {
                if (u.user.UserName == name)
                    authenticated = true;
            }
            if (authenticated)
            {
                var data = _voiceStateRepository.GetAll().Where(s => s.ServerId == formatWithServerIdSplitted[1]).ToList();
                var result = new { dataFromDB = data, format = formatWithServerIdSplitted[0] };
                return Json(result);
            }
            else
                return RedirectToAction("Account", "Account");
        }
        [HttpGet]
        public IActionResult GetMessageInfoFromDatabase(string ServerId)
        {
            var item = _messageInfoRepository.GetAll().Where(s => s.ServerId == ServerId).ToList();
            return Json(item);
        }
        [HttpGet]
        public async Task<IActionResult> GetMessageInfoFromDatabaseForGraphAndDownload(string formatWithServerId)
        {
            var formatWithServerIdSplitted = formatWithServerId.Split(":");

            bool authenticated = false;
            var usersInGuild = await _discord.GetCurrentGuildUsers(_configuration["API:BotToken"], formatWithServerIdSplitted[1]);
            if (usersInGuild == null)
            {
                return RedirectToAction("Account", "Account");
            }
            var name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            foreach (var u in usersInGuild)
            {
                if (u.user.UserName == name)
                    authenticated = true;
            }
            if (authenticated)
            {
                var data = _messageInfoRepository.GetAll().Where(s => s.ServerId == formatWithServerIdSplitted[1]).ToList();
                var result = new { dataFromDB = data, format = formatWithServerIdSplitted[0] };
                return Json(result);
            }
            else
                return RedirectToAction("Account", "Account");

        }
        [HttpGet]
        public async Task<IActionResult> GetAllPresencesFromDatabase(string ServerId)
        {
            bool authenticated = false;
            var usersInGuild = await _discord.GetCurrentGuildUsers(_configuration["API:BotToken"], ServerId);
            if (usersInGuild == null)
            {
                return RedirectToAction("Account", "Account");
            }
            var name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            foreach (var u in usersInGuild)
            {
                if (u.user.UserName == name)
                    authenticated = true;
            }
            if (authenticated)
            {
                return Json(_presenceRepository.GetAll().Where(s => s.ServerId == ServerId).ToList());
            }
            else
                return RedirectToAction("Account", "Account");
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPresencesFromDatabaseForGraphAndDownload(string formatWithServerId)
        {

            var formatWithServerIdSplitted = formatWithServerId.Split(":");

            bool authenticated = false;
            var usersInGuild = await _discord.GetCurrentGuildUsers(_configuration["API:BotToken"], formatWithServerIdSplitted[1]);
            if (usersInGuild == null)
            {
                return RedirectToAction("Account", "Account");
            }
            var name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            foreach (var u in usersInGuild)
            {
                if (u.user.UserName == name)
                    authenticated = true;
            }
            if (authenticated)
            {
                var data = _presenceRepository.GetAll().Where(s => s.ServerId ==  formatWithServerIdSplitted[1]).ToList();
                var result = new { dataFromDB = data, format = formatWithServerIdSplitted[0] };
                return Json(result);
            }
            else
                return RedirectToAction("Account", "Account");
        }


        [HttpGet]
        public async Task<IActionResult> ActionResult(string formatWithDetailsServerId)
        {
            var formatWithDetailsServerIdSplitted = formatWithDetailsServerId.Split(":");
            bool authenticated = false;
            var usersInGuild = await _discord.GetCurrentGuildUsers(_configuration["API:BotToken"], formatWithDetailsServerIdSplitted[1]);
            if (usersInGuild == null)
            {
                return RedirectToAction("Account", "Account");
            }
            var name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            foreach (var u in usersInGuild)
            {
                if (u.user.UserName == name)
                    authenticated = true;
            }
            if (authenticated)
            {
                var data = _statusRepository.GetAll().Where(s => s.ServerId == formatWithDetailsServerIdSplitted[1]).ToList();
                var result = new { dataFromDB = data, format = formatWithDetailsServerIdSplitted[0] };
                return Json(result);
            }
            else
                return RedirectToAction("Account", "Account");
        }
        [HttpGet]
        public async Task<IActionResult> GetPresencesFromDatabase(string ServerId, string GameName)
        {
            bool authenticated = false;
            var usersInGuild = await _discord.GetCurrentGuildUsers(_configuration["API:BotToken"], ServerId);
            if (usersInGuild == null)
            {
                return RedirectToAction("Account", "Account");
            }
            var name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            foreach (var u in usersInGuild)
            {
                if (u.user.UserName == name)
                    authenticated = true;
            }
            if (authenticated)
            {
                var a = _presenceRepository.GetAll().Where(s => s.ServerId == ServerId && s.Name == GameName).ToList();
                return Json(_presenceRepository.GetAll().Where(s => s.ServerId == ServerId && s.Name == GameName).ToList());
            }
            else
                return RedirectToAction("Account", "Account");

        }

        [HttpGet]
        public async Task<IActionResult> GetPresencesFromDatabaseForGraphAndDownload(string formatWithDetailsServerId, string GameName)
        {
            var formatWithDetailsServerIdSplitted = formatWithDetailsServerId.Split(":");

            bool authenticated = false;
            var usersInGuild = await _discord.GetCurrentGuildUsers(_configuration["API:BotToken"], formatWithDetailsServerIdSplitted[1]);
            if (usersInGuild == null)
            {
                return RedirectToAction("Account", "Account");
            }
            var name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            foreach (var u in usersInGuild)
            {
                if (u.user.UserName == name)
                    authenticated = true;
            }
            if (authenticated)
            {
                var data = _presenceRepository.GetAll().Where(s => s.ServerId == formatWithDetailsServerIdSplitted[1] && s.Name == GameName).ToList();
                var result = new { dataFromDB = data, format = formatWithDetailsServerIdSplitted[0] };
                return Json(result);
            }
            else
                return RedirectToAction("Account", "Account");

        }


        [HttpGet]
        public async Task<IActionResult> GetServerMemberFromDatabaseWithDateForGraphAndDownload(string formatWithDetailsServerId, string startDate, string endDate)
        {
            var formatWithDetailsServerIdSplitted = formatWithDetailsServerId.Split(":");
            if (startDate == null)
                startDate = "1-1-0001";
            if (endDate == null)
                endDate = "1-1-0001";
            var memberCount = _discord.GetServerUsersByDates(DateTime.Parse(startDate), DateTime.Parse(endDate), formatWithDetailsServerIdSplitted[1]);
            var newMembers = new List<ServerMembers>();
            for (int i = 0; i < memberCount.Count; i++)
            {
                if (i != 0)
                {
                    if (memberCount[i].Date.ToString("yyyy-MM-dd") == memberCount[i - 1].Date.ToString("yyyy-MM-dd") && memberCount[i].Members != memberCount[i - 1].Members)
                    {
                        newMembers.Add(memberCount[i]);
                    }
                    else if (memberCount[i].Date.ToString("yyyy-MM-dd") != memberCount[i - 1].Date.ToString("yyyy-MM-dd"))
                    {
                        newMembers.Add(memberCount[i]);
                    }
                }
                else
                {
                    newMembers.Add(memberCount[i]);
                }
            }

            var data = newMembers;
            var result = new { dataFromDB = data, format = formatWithDetailsServerIdSplitted[0], startDate = startDate, endDate = endDate };
            return Json(result);
        }



        [HttpGet]
        public async Task<IActionResult> GetUsersFromDatabase(string ServerId)
        {
            bool authenticated = false;
            var usersInGuild = await _discord.GetCurrentGuildUsers(_configuration["API:BotToken"], ServerId);
            if (usersInGuild == null)
            {
                return RedirectToAction("Account", "Account");
            }
            var name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            foreach (var u in usersInGuild)
            {
                if (u.user.UserName == name)
                    authenticated = true;
            }
            if (authenticated)
            {
                return Json(_userRepository.GetAll().Where(s => s.Servers == ServerId).ToList());
            }
            else
                return RedirectToAction("Account", "Account");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetPresenceDataFromDb()
        {
            _logger.LogInformation("GetPresenceDataFromDb");
            List<Presence> presences = _presenceRepository.GetAll().ToList();
            PresenceChartDataVM presenceChartDataVM = new();
            var presencesNameAndCount = presenceChartDataVM.AllPresenceNameListAndCount(presences);

            return Json(new { userPerGame = presencesNameAndCount });
        }
    }
}
