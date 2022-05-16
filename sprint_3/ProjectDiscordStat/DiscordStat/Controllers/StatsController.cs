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
    [Authorize]
    public class StatsController : Controller
    {

        private readonly IDiscordUserAndUserWebSiteInfoRepository _userRepository;
        private readonly IPresenceRepository _presenceRepository;
        private readonly ILogger<ApiController> _logger;
        private readonly IDiscordService _discord;
        private readonly IServerRepository _serverRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IMessageInfoRepository _messageInfoRepository;
        private readonly IVoiceStateRepository _voiceStateRepository;
        private readonly IServerMemberRepository _serverMemberRepository;
        private readonly IStatusRepository _statusRepository;


        public StatsController(ILogger<ApiController> logger, IDiscordUserAndUserWebSiteInfoRepository discordUserRepo, IPresenceRepository presenceRepository, IDiscordService discord, IServerRepository serverRepository, IChannelRepository channelRepository, IMessageInfoRepository messageInfoRepository, IVoiceStateRepository voiceStateRepository, IServerMemberRepository serverMemberRepository, IStatusRepository statusRepository)
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
        }

        public IActionResult ServerStats(string ServerId)
        {
            if (_serverRepository.GetAll().Any(c => c.Id == ServerId))
                return View((object)ServerId);
            else
                return View();
        }

        [HttpGet]
        public IActionResult GetMessageInfoFromDatabase(string ServerId)
        {
            var item = _messageInfoRepository.GetAll().Where(s => s.ServerId == ServerId).ToList();
            return Json(item);
        }
        
        [HttpGet]
        public IActionResult GetPresencesFromDatabase(string ServerId, string GameName)
        {
            return Json(_presenceRepository.GetAll().Where(s => s.ServerId == ServerId && s.Name == GameName).ToList());
        }
        [HttpGet]
        public IActionResult GetServerMemberFromDatabase(string ServerId)
        {
            var memberCount = _serverMemberRepository.GetAll().Where(s => s.Id == ServerId).OrderBy(d => d.Date).ToList();
            var newMembers = new List<ServerMembers>();
            for(int i = 0; i < memberCount.Count; i++)
            {
                if(i != 0)
                {
                    if( memberCount[i].Date.ToString("yyyy-MM-dd") == memberCount[i-1].Date.ToString("yyyy-MM-dd") && memberCount[i].Members != memberCount[i-1].Members)
                    {
                        newMembers.Add(memberCount[i]);
                    }
                    else if(memberCount[i].Date.ToString("yyyy-MM-dd") != memberCount[i - 1].Date.ToString("yyyy-MM-dd"))
                    {
                        newMembers.Add(memberCount[i]);
                    }
                }
                else
                {
                    newMembers.Add(memberCount[i]);
                }
            }
            return Json(newMembers);
        }
        [HttpGet]
        public async Task<IActionResult> GetServerMemberFromDatabaseWithDate(string ServerId, string startDate, string endDate)
        {
            if (startDate == null)
                startDate = "1-1-0001";
            if (endDate == null)
                endDate = "1-1-0001";
            var memberCount =  _discord.GetServerUsersByDates(DateTime.Parse(startDate), DateTime.Parse(endDate), ServerId);
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
            return Json(newMembers);
        }
        [HttpGet]
        public IActionResult GetAllPresencesFromDatabase(string ServerId)
        {
            return Json(_presenceRepository.GetAll().Where(s => s.ServerId == ServerId).ToList());
        }

        [HttpGet]
        public IActionResult GetUsersFromDatabase(string ServerId)
        {
            return Json(_userRepository.GetAll().Where(s => s.Servers == ServerId).ToList());
        }

        [HttpGet]
        public IActionResult GetVoiceStatesFromDatabase(string ServerId)
        {
            return Json(_voiceStateRepository.GetAll().Where(s => s.ServerId == ServerId).ToList());
        }
        [HttpGet]
        public IActionResult GetStatusesFromDatabase(string ServerId)
        {
            return Json(_statusRepository.GetAll().Where(s => s.ServerId == ServerId).ToList());
        }
        [HttpGet]
        public IActionResult GetStatusesFromDatabaseToDownload(string formatWithDetailsServerId)
        {
            var formatWithDetailsServerIdSplitted = formatWithDetailsServerId.Split(":");
            var data = _statusRepository.GetAll().Where(s => s.ServerId == formatWithDetailsServerIdSplitted[1]).ToList();
            var result = new { dataFromDB = data, format = formatWithDetailsServerIdSplitted[0] };
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetServerMemberFromDatabaseWithDateToDownload(string formatWithDetailsServerId, string startDate, string endDate)
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
    }
}
