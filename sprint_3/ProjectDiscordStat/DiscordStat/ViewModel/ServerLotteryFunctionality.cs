using DiscordStats.DAL.Abstract;
using DiscordStats.Models;

namespace DiscordStats.ViewModel
{
    public class ServerLotteryFunctionality
    {
        private readonly IServerRepository _serverRepository;

        public ServerLotteryFunctionality(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
        }
     
        public Server FunctionalityEquation()
        {
            Random rnd = new Random();
            List<Server> servers = _serverRepository.GetAll().Where(p => p.Privacy == "public").Where(t => t.InLottery == "true").ToList();
            int countOfServer = servers.Count;

            int randomNumber = rnd.Next(0, countOfServer);
            Server server = servers[randomNumber];
            return server;
        }

    }
}
