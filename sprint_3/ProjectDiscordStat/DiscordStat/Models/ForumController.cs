using DiscordStats.Models;
using DiscordStats.DAL.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using DiscordStats.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiscordStats.ViewModel;
using DiscordStats.DAL.Concrete;

namespace DiscordStats.Controllers
{
    public class ForumController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDiscordService _discord;
        private readonly IConfiguration _configuration;
        private readonly IServerRepository _serverRepository;
        private readonly CaptchaService _CaptchaService;

        public ForumController(ILogger<HomeController> logger, IDiscordService discord, IConfiguration config, IServerRepository serverRepository,CaptchaService captchaService)
        {
            _logger = logger;
            _discord = discord;
            _configuration = config;
            _serverRepository = serverRepository;
            _CaptchaService = captchaService;
        }

        public IActionResult Index()
        {
            IEnumerable<Server> servers = _serverRepository.GetAll().Where(x => x.Privacy == "public");
            return View(servers);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Discord")]
        public IActionResult Forum()
        {
            var servers = _serverRepository.GetAll();

            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var selectList = new SelectList(
            servers.Where(m => m.OwnerId == userId).ToList().Select(s => new { Text = $"{s.Name}", Value = s.Id }),
            "Value", "Text");
            ViewData["Id"] = selectList;

            ViewData["Message"] = servers.Where(m => m.OwnerId == userId).ToList().Select(s => s.Message);

            return View();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> ServerOnForum(ForumViewModel vm)
        {
           var captchaResult = await _CaptchaService.VerifyToken(vm.Token);
            if(!captchaResult)
            {
                return RedirectToAction("Index");

            }

            //var messageLength = server.Message.Length;
            if (vm.server.Id != null)
            {             
                string onForum = "true";
                if(vm.server.Message == null)
                {
                    vm.server.Message = "null";
                }
                _serverRepository.UpdateOnServerWithForumInfo(vm.server.Id, onForum, vm.server.Message);
                return RedirectToAction("Index");

            }
            else
            {
                IEnumerable<Server>? servers = _serverRepository.GetAll();

                var selectList = new SelectList(
                servers.Where(m => m.Owner == "true").ToList().Select(s => new { Text = $"{s.Name}", Value = s.Id }),
                "Value", "Text");
                ViewData["ServerBroadcasting"] = selectList;
                return RedirectToAction("Forum");
            }
                     
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Discord")]
        public IActionResult ServerOffForum([Bind("Id, Message")] Server server)
        {
            if (server.Id != null)
            {
                string onForum = "false";
                server.Message = "null";
                _serverRepository.UpdateOnServerWithForumInfo(server.Id, onForum, server.Message);
                return RedirectToAction("Index");
            }
            else
            {
                IEnumerable<Server>? servers = _serverRepository.GetAll();

                var selectList = new SelectList(
                servers.Where(m => m.Owner == "true").ToList().Select(s => new { Text = $"{s.Name}", Value = s.Id }),
                "Value", "Text");
                ViewData["ServerBroadcasting"] = selectList;
                return RedirectToAction("Forum");
            }

        }
    }
}
