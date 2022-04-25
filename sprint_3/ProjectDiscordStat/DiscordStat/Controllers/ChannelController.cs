using Microsoft.AspNetCore.Mvc;
using DiscordStats.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;
using DiscordStats.DAL.Abstract;
using DiscordStats.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public ChannelController(ILogger<ChannelController> logger, IConfiguration configuration, IChannelRepository channelRepository, IDiscordServicesForChannels discordServicesForChannels, IServerRepository serverRepository, IDiscordService discord)
        {
            _logger = logger;
            _configuration = configuration;
            _channelRepository = channelRepository;
            _serverRepository = serverRepository;
            _discordServicesForChannels = discordServicesForChannels;
            _discord = discord;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ServerChannels(string? serverId)
        {
            bool authenticated = false;
            var usersInGuild = await _discord.GetCurrentGuildUsers(_configuration["API:BotToken"], serverId);
            if (usersInGuild == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
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

                ServerChannelsVM serverChannels = new();
                ChannelsFromDatabase channelsWithCount = new(_channelRepository);
                if (selectedServer != null)
                {
                    if (selectedServer.HasBot == "true")
                    {
                        serverChannels.serverChannels = await _discordServicesForChannels.GetServerChannels(botToken, selectedServer.Id);
                        serverChannels.serverChannels = channelsWithCount.ServersWithCount(serverChannels.serverChannels);
                        var selectList = new SelectList(
                         serverChannels.serverChannels.Where(m => m.type == "4").ToList().Select(s => new { Text = $"{s.name}", Value = s.id }),
                        "Value", "Text");
                        ViewData["Id"] = selectList;
                        serverChannels.guild_id = serverId;
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
                return View(serverChannels);
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
        public async Task<IActionResult> CreateChannel(ServerChannelsVM create)
        {
            string botToken = _configuration["API:BotToken"];
            if (create.type_text == true ) create.type = "0";
            if (create.type_text == false && create.type_voice == false) create.type = "0";
            if (create.type_voice == true) create.type = "2";
            await _discordServicesForChannels.CreateChannel(botToken, create.guild_id, create.name, create.type, create.parent_id);
            return RedirectToAction("ServerChannels", new {serverId = create.guild_id});
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
        public async Task<IActionResult> WebhookForm(WebhookUsageVM vm)
        {
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
            Channel channel = channels.Where( i => i.Id == webhook.channel_id).FirstOrDefault();
            await _discordServicesForChannels.DeleteWebhook(botToken, webhook.Id);
            return RedirectToAction("ChannelWebhooks", channel);
        }

    }
}
