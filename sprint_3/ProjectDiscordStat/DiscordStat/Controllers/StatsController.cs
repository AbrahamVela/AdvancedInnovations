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

        public StatsController(ILogger<ApiController> logger, IDiscordUserAndUserWebSiteInfoRepository discordUserRepo, IPresenceRepository presenceRepository, IDiscordService discord, IServerRepository serverRepository, IChannelRepository channelRepository, IMessageInfoRepository messageInfoRepository, IVoiceStateRepository voiceStateRepository)
        {
            _logger = logger;
            _userRepository = discordUserRepo;
            _presenceRepository = presenceRepository;
            _discord = discord;
            _serverRepository = serverRepository;
            _channelRepository = channelRepository;
            _messageInfoRepository = messageInfoRepository;
            _voiceStateRepository = voiceStateRepository;
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
        public IActionResult GetAllPresencesFromDatabase(string ServerId)
        {
            return Json(_presenceRepository.GetAll().Where(s => s.ServerId == ServerId).ToList());
        }

        [HttpGet]
        public IActionResult GetUsersFromDatabase(string ServerId)
        {
            var test = _userRepository.GetAll().Where(s => s.Servers == ServerId).ToList();
            return Json(_userRepository.GetAll().Where(s => s.Servers == ServerId).ToList());
        }

        //[HttpGet]
        //public IActionResult GetVoiceStatesFromDatabase(string ServerId)
        //{

        //    return Json(_voiceStateRepository.GetAll().Where(s => s.ServerId == ServerId).ToList());
        //}

        [HttpPost]
        public FileResult ActiveMessageTime(string data)
        {
            StringBuilder dataJsonFile = new StringBuilder();
            dataJsonFile.AppendLine(data);


            var currentWorkingDirectory = Environment.GetEnvironmentVariable("USERPROFILE");
            var downloadsDirectory = Path.Combine(currentWorkingDirectory, "Desktop\\");
            CreateADirectory(downloadsDirectory);
            string fileName = downloadsDirectory + "UsersActiveMessaginTime_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".json";
            CreateFile(fileName, dataJsonFile);

            string textFile = "Your data info is located on your desktop.";
            return File(Encoding.UTF8.GetBytes(textFile.ToString()), "application/json", fileName);
        }

        [HttpPost]
        public FileResult ActivePresenceTime(string data)
        {
            StringBuilder dataJsonFile = new StringBuilder();
            dataJsonFile.AppendLine(data);


            var currentWorkingDirectory = Environment.GetEnvironmentVariable("USERPROFILE");
            var downloadsDirectory = Path.Combine(currentWorkingDirectory, "Desktop\\");
            CreateADirectory(downloadsDirectory);
            string fileName = downloadsDirectory + "UsersActiveGamingTime_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".json";
            CreateFile(fileName, dataJsonFile);

            string textFile = "Your data info is located on your desktop.";
            return File(Encoding.UTF8.GetBytes(textFile.ToString()), "application/json", fileName);
        }

        [HttpPost]
        public FileResult HoursPerGame(string data)
        {
            StringBuilder dataJsonFile = new StringBuilder();
            dataJsonFile.AppendLine(data);


            var currentWorkingDirectory = Environment.GetEnvironmentVariable("USERPROFILE");
            var downloadsDirectory = Path.Combine(currentWorkingDirectory, "Desktop\\");
            CreateADirectory(downloadsDirectory);
            string fileName = downloadsDirectory + "UsersHourPerGame_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".json";
            CreateFile(fileName, dataJsonFile);

            string textFile = "Your data info is located on your desktop.";
            return File(Encoding.UTF8.GetBytes(textFile.ToString()), "application/json", fileName);
        }

        public void CreateADirectory(string startingPath)
        {
            //creates new directory if it isnt there
            if (!Directory.Exists(startingPath))
            {
                Directory.CreateDirectory(startingPath);

            }
            else return;
        }

        public void CreateFile(string fileName, StringBuilder dataJsonFile)
        {
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }

                // Create a new file     
                using (FileStream fs = System.IO.File.Create(fileName))
                {
                    //// Add some text to file    
                    Byte[] dataJsonFileAsByte = new UTF8Encoding(true).GetBytes(dataJsonFile.ToString());
                    fs.Write(dataJsonFileAsByte, 0, dataJsonFileAsByte.Length);

                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
    }
}
