using DiscordStats.Models;
using Microsoft.AspNetCore.Mvc;
using JsonSerializer = System.Text.Json.JsonSerializer;
using DiscordStats.DAL.Abstract;
using DiscordStats.ViewModel;


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


        private readonly IDiscordUserAndUserWebSiteInfoRepository _userRepository;
        private readonly IPresenceRepository _presenceRepository;
        private readonly ILogger<ApiController> _logger;
        private readonly IDiscordService _discord;
        private readonly IDiscordServicesForChannels _discordServicesForChannels;
        private readonly IServerRepository _serverRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IMessageInfoRepository _messageInfoRepository;
        private readonly IVoiceChannelRepository _voiceChannelRepository;
        private readonly IVoiceStateRepository _voiceStateRepository;

        public ApiController(ILogger<ApiController> logger, IDiscordUserAndUserWebSiteInfoRepository discordUserRepo, IPresenceRepository presenceRepository, IDiscordService discord, IDiscordServicesForChannels discordServicesForChannels, IServerRepository serverRepository, IChannelRepository channelRepository, IVoiceChannelRepository voiceChannelRepository, IMessageInfoRepository messageInfoRepository, IVoiceStateRepository voiceStateRepository)

        {
            _logger = logger;
            _userRepository = discordUserRepo;
            _presenceRepository = presenceRepository;
            _discord = discord;
            _discordServicesForChannels = discordServicesForChannels;
            _serverRepository = serverRepository;
            _channelRepository = channelRepository;
            _messageInfoRepository = messageInfoRepository;
            _voiceChannelRepository = voiceChannelRepository;
            _voiceStateRepository = voiceStateRepository;
        }


        [HttpPost]
        public async Task<IActionResult> PostUsers(DiscordUserAndUserWebSiteInfo[] users)
        {
           
            foreach (var user in users)
            {

                var duplicate = false;

                Task.Delay(300).Wait();
                await Task.Run(() =>
                {
                    var allDiscordUsers = _userRepository.GetAll().ToList();

                    for (int i = 0; i < allDiscordUsers.Count(); i++)
                    {
                        if (user.Id == allDiscordUsers[i].Id && user.Servers == allDiscordUsers[i].Servers)
                        {
                            duplicate = true;
                        }
                    }
                    if (!duplicate)
                    {
                        _userRepository.AddOrUpdate(user);
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
                if(presence.Name.Contains("™"))
                {
                    var trademark = presence.Name.IndexOf("™");
                    presence.Name = presence.Name.Remove(trademark);
                }
            }
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

        [HttpPost]
        public IActionResult PostVoiceStates(VoiceState[] voiceStates)
        {
            var duplicate = false;
            foreach (var voiceState in voiceStates)
            {
                foreach (var voice in _voiceStateRepository.GetAll().ToList())
                {
                    if (voice.UserId == voiceState.UserId && voice.ServerId == voiceState.ServerId && voice.CreatedAt?.Hour == voiceState.CreatedAt?.Hour && voice.CreatedAt?.Date == voiceState.CreatedAt?.Date)
                    {
                        duplicate = true;
                    }
                }
                if (duplicate)
                {
                    if (voiceState.CreatedAt?.Hour != DateTime.UtcNow.Hour)
                    {
                        var newVoiceState = voiceState;
                        newVoiceState.CreatedAt = DateTime.UtcNow;
                        _voiceStateRepository.AddOrUpdate(voiceState);

                    }
                }
                if (!duplicate)
                {
                    _voiceStateRepository.AddOrUpdate(voiceState);
                }
            }

            return Json("It worked");
        }
    }
}
