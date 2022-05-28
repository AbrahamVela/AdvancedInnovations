using Microsoft.AspNetCore.Mvc;
using DiscordStats.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;
using DiscordStats.DAL.Abstract;
using DiscordStats.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiscordStats.DAL.Concrete;

namespace DiscordStats.Controllers
{
    [Authorize(AuthenticationSchemes = "Discord")]
    public class ChannelController : Controller
    {
        private readonly ILogger<ChannelController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IChannelRepository _channelRepository;
        private readonly IDiscordServicesForChannels _discordServicesForChannels;
        private readonly IServerRepository _serverRepository;
        private readonly IDiscordService _discord;
        private readonly CaptchaService _CaptchaService;

        public ChannelController(ILogger<ChannelController> logger, IConfiguration configuration, IChannelRepository channelRepository, IDiscordServicesForChannels discordServicesForChannels, IServerRepository serverRepository, IDiscordService discord, CaptchaService captchaService)

        {
            _logger = logger;
            _configuration = configuration;
            _channelRepository = channelRepository;
            _serverRepository = serverRepository;
            _discordServicesForChannels = discordServicesForChannels;
            _discord = discord;
            _CaptchaService = captchaService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ServerChannels(string? serverId)
        {
            bool authenticated = false;
            var server = await _discord.GetFullGuild(_configuration["API:BotToken"], serverId);
            var usersInGuild = await _discord.GetCurrentGuildUsers(_configuration["API:BotToken"], serverId);
            if (usersInGuild == null || server == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            var owner = usersInGuild.Where(m => m.user.Id == server.Owner_Id).First();
            if (owner.user.UserName == name)
                authenticated = true;
            foreach (var u in usersInGuild)
            {
                if (u.user.UserName == name)
                    authenticated = true;
            }
            if (authenticated)
            {
                string botToken = _configuration["API:BotToken"];
                var servers = _serverRepository.GetAll();
                var selectedServer = servers.Where(m => m.Id == serverId).FirstOrDefault();

                CreateChannelVM vm = new();
                vm.channelsVM = new();
               
                ChannelsFromDatabase channelsWithCount = new(_channelRepository);
                if (selectedServer != null)
                {
                    if (selectedServer.HasBot == "true")
                    {
                        vm.channelsVM.serverChannels = await _discordServicesForChannels.GetServerChannels(botToken, selectedServer.Id);
                        vm.channelsVM.serverChannels = channelsWithCount.ServersWithCount(vm.channelsVM.serverChannels);
                        var selectList = new SelectList(
                         vm.channelsVM.serverChannels.Where(m => m.type == "4").ToList().Select(s => new { Text = $"{s.name}", Value = s.id }),
                        "Value", "Text");
                        ViewData["Id"] = selectList;
                        vm.channelsVM.guild_id = serverId;
                        ViewBag.hasBot = "true";

                    }
                    else
                    {
                        ViewBag.hasBot = "false";
                    }
                }
                else
                {
                    ViewBag.hasBot = "false";
                }

                //return View(channels);
                return View(vm);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteChannel(string id)
        {
            string botToken = _configuration["API:BotToken"];
            var channel = _channelRepository.GetAll().Where(c => c.Id == id).SingleOrDefault();
            _channelRepository.DeleteById(channel.ChannelPk);
            await _discordServicesForChannels.DeleteChannel(botToken, id);
            return RedirectToAction("Servers", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChannel(CreateChannelVM create)
        {
            var captchaResult = await _CaptchaService.VerifyToken(create.Token);
            if (!captchaResult)
            {
                return RedirectToAction("Index");

            }

            string botToken = _configuration["API:BotToken"];
            if (create.channelsVM.type_text == true) create.channelsVM.type = "0";
            if (create.channelsVM.type_text == false && create.channelsVM.type_voice == false) create.channelsVM.type = "0";
            if (create.channelsVM.type_voice == true) create.channelsVM.type = "2";

            var response = await _discordServicesForChannels.CreateChannel(botToken, create.channelsVM.guild_id, create.channelsVM.name, create.channelsVM.type, create.channelsVM.parent_id);

            response.GuildId = create.channelsVM.guild_id;
            _channelRepository.AddOrUpdate(response);

            return RedirectToAction("ServerChannels", new { serverId = create.channelsVM.guild_id });
        }



        public async Task<IActionResult> ChannelWebhooks(Channel channel)
        {
            bool authenticated = true;
            string botToken = _configuration["API:BotToken"];
            string channelId = channel.Id;
            IEnumerable<WebhookUsageVM> webhooks = await _discordServicesForChannels.GetChannelWebHooks(botToken, channelId);

            if (webhooks != null)
            {

                ViewBag.channel_id = channelId;

                return View(webhooks);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> WebhookForm(string channelId)
        {
            ViewBag.channelId = channelId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WebhookForm(WebhookUsageVM vm)
        {
            if (_CaptchaService != null)
            {
                if (vm.CapToken == null)
                {
                    vm.CapToken = "";
                }
                var captchaResult = await _CaptchaService.VerifyToken(vm.CapToken);
                if (!captchaResult)
                {
                    return RedirectToAction("Index", "Home");

                }
            }
            string botToken = _configuration["API:BotToken"];
            var webhook = await _discordServicesForChannels.CreateWebhook(botToken, vm.channelId, vm.name);
            WebhookUsageVM webhookObject = JsonConvert.DeserializeObject<WebhookUsageVM>(webhook);
            return RedirectToAction("WebhookMessage", webhookObject);
        }

        public async Task<IActionResult> WebhookMessage(WebhookUsageVM webhook)
        {
            WebhookUsageVM vm = new WebhookUsageVM();
            vm.name = webhook.name;
            vm.Id = webhook.Id;
            vm.Token = webhook.Token;
            vm.guild_id = webhook.guild_id;
            vm.channel_id = webhook.channel_id;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WebhookMessage(WebhookUsageVM webhook, string whatever)
        {
            
            string botToken = _configuration["API:BotToken"];
            WebhookDataVm vm = new WebhookDataVm(_serverRepository, _channelRepository);
            string messageData = vm.DataBeingSentBackForWebhook(webhook);
            await _discordServicesForChannels.SendMessageThroughWebhook(botToken, webhook.Id, webhook.Token, messageData);
            return View();
        }

        public async Task<IActionResult> DeleteWebhook(WebhookUsageVM webhook)
        {
            string botToken = _configuration["API:BotToken"];
            IEnumerable<Channel> channels = _channelRepository.GetAll();
            Channel channel = channels.Where(i => i.Id == webhook.channel_id).FirstOrDefault();
            await _discordServicesForChannels.DeleteWebhook(botToken, webhook.Id);
            return RedirectToAction("ChannelWebhooks", channel);
        }

    }
}