using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using DiscordStats.ViewModel;
using DiscordStats.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;


namespace DiscordStats.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IDiscordService _discord;
        private readonly IConfiguration _configuration;
        private readonly IServerRepository _serverRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IPresenceRepository _presenceRepository;
        private readonly IVoiceChannelRepository _voiceChannelRepository;
        private readonly IMessageInfoRepository _messageIngoChannelRepository;
        private readonly IDiscordUserAndUserWebSiteInfoRepository _userRepository;
        public AccountController(ILogger<HomeController> logger, IDiscordService discord, IConfiguration config, IServerRepository serverRepository, IChannelRepository channelRepository, IPresenceRepository presenceRepository, IVoiceChannelRepository voiceChannelRepository, IMessageInfoRepository messageInfoRepository, IDiscordUserAndUserWebSiteInfoRepository userRepository)
        {
            _logger = logger;
            _discord = discord;
            _configuration = config;
            _serverRepository = serverRepository;
            _channelRepository = channelRepository;
            _presenceRepository = presenceRepository;
            _voiceChannelRepository = voiceChannelRepository;
            _messageIngoChannelRepository = messageInfoRepository;
            _userRepository = userRepository;    

        }

        [Authorize (AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> Account()
        {
            // Don't use the ViewBag!  Use a viewmodel instead.
            // The data in ClaimTypes can be mocked.  Will have to wait though for how to do that.
            ViewBag.name  = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            ViewBag.id = userId;
            string bearerToken = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
            string botToken = _configuration["API:BotToken"];


            //var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            //var updatedOwner = await _discord.UpdateOwner(_configuration["API:BotToken"], "952358862059614218", userId);

            IEnumerable<Server>? servers = await _discord.GetCurrentUserGuilds(bearerToken);
            if (servers == null)
            {
                return RedirectToAction("Account", "Account");
            }
            foreach (Server server in servers)
            {              
                string hasBot = await _discord.CheckForBot(botToken, server.Id);
                if (hasBot == "true")
                {
                     var serverWithMemCount = await _discord.GetFullGuild(botToken, server.Id);

                     _discord.ServerEntryDbCheck(serverWithMemCount, hasBot, server.Owner);
                }
            }

            var userInfo = await _discord.GetCurrentUserInfo(bearerToken);
            ViewBag.hash = userInfo.Avatar;
            var websiteProfileInfo = _userRepository.GetAll().ToList();
            var vm = new ServerAndDiscordUserInfoAndWebsiteProfileVM();
            vm.Servers = servers.ToList();
            var user = websiteProfileInfo.Where(n => n.Id == userId).FirstOrDefault();
            if (user != null)
            {
                vm.id = user.Id;
                vm.ProfileFirstName = user.FirstName;
                vm.ProfileLastName = user.LastName;
                vm.ProfileBirthDate = user.BirthDate;
                vm.ProfileEmail = user.Email;
            }

            // Now we can inject a mock IDiscordService that fakes this method.  That will allow us to test
            // anything __after__ getting this list of servers, i.e. any logic that we perform with this data from
            // here on.  There's nothing here now but there presumably will be.  If this method used a viewmodel
            // then we could test this action method a little more, but it doesn't.

            // Unfortunately it doesn't allow us to test the actual code within the GetCurrentUserGuilds method.
            // For that we must take the next step in refactoring.
            //var test = await _discord.UpdateOwner(botToken);
            return View(vm);
        }


        [Authorize]
        public async Task<IActionResult> WebsiteProfileForm(string userId)
        {
            var vm = new ServerAndDiscordUserInfoAndWebsiteProfileVM();
            vm.id = userId;
            return View(vm);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> ProfileFormSubmit([Bind("id, ProfileFirstName, ProfileLastName, ProfileBirthDate, ProfileEmail")] ServerAndDiscordUserInfoAndWebsiteProfileVM websiteProfileInfo)
        {
            _userRepository.UpdateWebsiteProfileInfo(websiteProfileInfo);
            return RedirectToAction("Account");
        }

        [Authorize(AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> Servers()
        {
            ViewBag.id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            ViewBag.name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            string bearerToken = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;

            IEnumerable<Server>? servers = await _discord.GetCurrentUserGuilds(bearerToken);
            if (servers == null)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var s in servers.Where(m => m.Owner == "true"))
            {
                s.HasBot = await _discord.CheckForBot(_configuration["API:BotToken"], s.Id);
                foreach (var server in _serverRepository.GetAll().ToList())
                {

                    if (s.Id == server.Id)                        
                            if(server.InLottery == "true")                    
                            s.InLottery = "true";
                    if (server.InLottery == "false")
                        s.InLottery = "false";
                    
                }

            }
            //836721845570895872
            var test = servers.Where(m => m.Owner == "true").ToList();
            foreach(var server in test)
            {
                if (server.HasBot == "false")
                    server.InLottery = null;
            }
            //var servers2 = _serverRepository.GetAll().ToList();
            return View(test);
            //return View(servers2.Where(m => m.Owner == "true").ToList());
        }

        [Authorize(AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> AddServerLottery(string serverId)
        {
            Server selectedServer = _serverRepository.GetAll().Where(m => m.Id == serverId).FirstOrDefault();
            ServerLotteryFunctionality lottoFunction = new(_serverRepository);
            if(selectedServer.InLottery == "false")
            {
                _serverRepository.AddingServerToLottery(serverId);
            }
            return RedirectToAction("Servers");
        }

        [Authorize(AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> RemoveServerLottery(string serverId)
        {
            _serverRepository.RemoveServerFromLottery(serverId);

            return RedirectToAction("Servers");
        }

        [HttpPost]
        public IActionResult ChangePrivacy(string privacyString)
        {
            var listPrivacyChanges = privacyString.Split(' ');
            string privacy = listPrivacyChanges[0];
            string serverId = listPrivacyChanges[1];
            _serverRepository.UpdatePrivacy(serverId, privacy);
            return RedirectToAction("Servers");
        }


        [AllowAnonymous]
        public IActionResult Logout()
        {
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Details(string? name)
        {

            string bearerToken = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
            string botToken = _configuration["API:BotToken"];
            IEnumerable<Server>? servers = await _discord.GetCurrentUserGuilds(bearerToken);
            if(servers == null)
            {
                return RedirectToAction("Account", "Account");
            }
            var SelectedServer = servers.Where(m => m.Name == name).FirstOrDefault();
            if(SelectedServer == null)
            {
                return RedirectToAction("Account", "Account");
            }
            SelectedServer.HasBot = await _discord.CheckForBot(_configuration["API:BotToken"], SelectedServer.Id);
            var vm = new ServerOwnerViewModel();
            
            if (SelectedServer.HasBot == "true")
            {
                vm = await _discord.GetFullGuild(_configuration["API:BotToken"], SelectedServer.Id);
                var ServerOwner = await _discord.GetUserInfoById(_configuration["API:BotToken"], vm.Owner_Id);
                vm.HasBot = SelectedServer.HasBot;
                vm.Owner = ServerOwner.Username;

                vm.users = await _discord.GetCurrentGuildUsers(_configuration["API:BotToken"], vm.Id);

            }
            else
            {
                vm.Icon = SelectedServer.Icon;
                vm.Name = SelectedServer.Name;
                vm.Id = SelectedServer.Id;
                vm.HasBot = "false";
            }
            var ServerMessages = _messageIngoChannelRepository.GetAll().Where(m => m.ServerId == vm.Id).ToList();
            var UsersMessagesFiltered = ServerMessages.GroupBy(x => x.UserId).Select(x => x.ToList()).Take(3).ToList();
            var users = new List<UserMessageVM>();
            foreach (var u in UsersMessagesFiltered)
            {
                var user = await _discord.GetUserInfoById(botToken, u[0].UserId);
                UserMessageVM MessageUser = new();
                MessageUser.Id = user.Id;
                MessageUser.Username = user.Username;
                MessageUser.Avatar = user.Avatar;
                MessageUser.MessageCount = u.Count();
                users.Add(MessageUser);
            }
            vm.userMessageVMs = users.OrderByDescending(m => m.MessageCount).ToList();
            return View(vm);
        }
        [Authorize(AuthenticationSchemes = "Discord")]
        public void LeaveServer(string ServerId)
        {
            string userid = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            string bearerToken = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
            _discord.RemoveUserServer(_configuration["API:BotToken"], ServerId, userid);
        }
        
        public async void KickUser(string ServerId,string user)
        {
            var users = await _discord.GetCurrentGuildUsers(_configuration["API:BotToken"], ServerId);
            
            string UserId = "";
            foreach (var u in users)
            {
                if (u.user.UserName == user)
                    UserId = u.user.Id;
            }

            await _discord.RemoveUserServer(_configuration["API:BotToken"], ServerId, UserId);
        }
        [Authorize(AuthenticationSchemes = "Discord")]

        public async Task<IActionResult> ServerForm()
        {

            return View();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> ServerForm(CreateServerVM vm)
        {
            var newServer = await _discord.CreateServer(_configuration["API:BotToken"], vm);
            vm = JsonConvert.DeserializeObject<CreateServerVM>(newServer);

            vm.accessCode = await addUsertoGuild(vm.Id);

            return RedirectToAction("ServerCreateUpdateOwner", vm );
        }

        [Authorize(AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> ServerCreateUpdateOwner(CreateServerVM vm)
        {
            return View(vm);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Discord")]
        public async void ServerCreateUpdateOwner(string ServerId)
        {
            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var updatedOwner = await _discord.UpdateOwner(_configuration["API:BotToken"], ServerId, userId);
        }

        public async Task<string> addUsertoGuild(string serverId)
        {
            var getResponse = await _discord.FindChannels(_configuration["API:BotToken"], serverId);
            var channelList = getResponse.Split("},")[2];
            var channelId = channelList.Split(",")[0].Split(":")[1];
            channelId = channelId.Remove(0, 2);
            channelId = channelId.Remove(channelId.Length - 1, 1);


            var postResponse = await _discord.AddMemberToGuild(_configuration["API:BotToken"], channelId);
            string codeValue = "";

            codeValue = postResponse.Split(",")[0].Split(":")[1];
            codeValue = codeValue.Remove(0, 2);
            codeValue = codeValue.Remove(codeValue.Length - 1, 1);
            return codeValue;
        }
        [Authorize(AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> Games(string ServerId)
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
                if(u.user.UserName == name)
                    authenticated = true;
            }
            if (authenticated)
            {
                List<GamesVM> games = new List<GamesVM>();

                var presence_list = _discord.GetPresencesForServer(ServerId).Result;

                foreach (var presence in presence_list)

                {
                    var duplicate = false;
                    foreach (var game in games)
                    {
                        if (game.name == presence.Name)
                        {
                            game.UserCount++;
                            duplicate = true;
                        }
                    }
                    if (duplicate == false)
                    {
                        GamesVM newGame = new GamesVM();
                        newGame.ServerId = ServerId;
                        newGame.name = presence.Name;
                        newGame.UserCount = 1;

                        if (presence.Image == null)
                            newGame.GameImage = "https://e7.pngegg.com/pngimages/672/63/png-clipart-discord-computer-icons-online-chat-cool-discord-icon-logo-smiley.png";
                        else
                            newGame.GameImage = presence.Image;
                        newGame.smallImageId = presence.SmallImageId;
                        if (newGame.smallImageId != null)
                            if (newGame.smallImageId.Contains("playstation"))
                                newGame.GameImage = "https://blog.playstation.com/tachyon/2021/03/Playstation-logo.jpg";
                        if (newGame.GameImage == null)

                        {
                            var game = await _discord.GetJsonStringFromEndpointGames(newGame.name);
                            if (game == null)
                            {
                                newGame.icon = "https://e7.pngegg.com/pngimages/672/63/png-clipart-discord-computer-icons-online-chat-cool-discord-icon-logo-smiley.png";
                                newGame.id = "1";
                            }
                            else
                            {
                                if (game.icon == null)
                                    newGame.icon = "https://e7.pngegg.com/pngimages/672/63/png-clipart-discord-computer-icons-online-chat-cool-discord-icon-logo-smiley.png";
                                else
                                    newGame.icon = game.icon;
                                newGame.id = game.id;
                            }

                        }
                        games.Add(newGame);
                    }
                }
                return View(games);
            }
            else
            {
                return RedirectToAction("Account", "Account");
            }
        }
        

        [Authorize(AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> GameDetails(string gameName, string ServerId )
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
                var ps = new ServerIdAndGameNameVM()
                {
                    ServerId = ServerId,
                    GameName = gameName
                };
                return View(ps);
            }
            else
            {
                return RedirectToAction("Account", "Account");
            }
        }
        [HttpGet]
        public IActionResult GetVoiceChannelInfoFromDatabase(string ServerId)
        {
            var voiceChannels = _voiceChannelRepository.GetAll().Where(v => v.GuildId == ServerId).ToList();
            var distinctTimes = voiceChannels.DistinctBy(p => p.Time.Value.Hour).ToList();
            var graphData = new List<VoiceChannelGraph>();
            foreach (var dt in distinctTimes)
            {
                VoiceChannelGraph voiceChannelGraph = new VoiceChannelGraph();
                voiceChannelGraph.hour = dt.Time.Value.Hour;
                voiceChannelGraph.TotalmemberCount = 0;
                voiceChannelGraph.divider = 0;
                graphData.Add(voiceChannelGraph);
                foreach (var vc in voiceChannels)
                {
                    if (vc.Time.Value.Hour == dt.Time.Value.Hour)
                    {
                        graphData.Where(g => g.hour == vc.Time.Value.Hour).First().TotalmemberCount += vc.Count;
                        graphData.Where(g => g.hour == vc.Time.Value.Hour).First().divider++;


                    }
                }
            }
            foreach (var data in graphData)
            {
                data.avgMemberCount = (double)data.TotalmemberCount / data.divider;
            }
                return Json(graphData);
        }
        public async Task<PartialViewResult> UpdateMessagesByDate(DateTime StartDate, DateTime EndDate, string serverId)
        {
            string botToken = _configuration["API:BotToken"];
            var users = await _discord.UpdatedMessagesByDates(StartDate, EndDate, serverId);

            var model = new ServerOwnerViewModel();
            model.userMessageVMs = users;
            return PartialView("_UpdateMessagePartial", model);
        }
    }
}

