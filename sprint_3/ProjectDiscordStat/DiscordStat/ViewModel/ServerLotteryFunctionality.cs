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
     
        public void FunctionalityEquation(string serverId)
        {
            Random rnd = new Random();
            List<Server> servers = _serverRepository.GetAll().ToList();
            int countOfServer = servers.Count;

            int randomNumber = rnd.Next(0, countOfServer);
            Server server = servers[randomNumber];
            if (server.Id == serverId)
            {
                _serverRepository.ServerLotteryWinner(server);
            }
        }

        public void ResetFunctionalityEquation()
        {
            Random rnd = new Random();
            List<Server> servers = _serverRepository.GetAll().ToList();
            List<Server> serversWithInLotteryTrue = new();

            foreach (Server server in servers)
            {
                if (server.InLottery != "null" && server.InLottery != "trueWinner")
                {
                    serversWithInLotteryTrue.Add(server);
                }
            }

            int countOfServer = serversWithInLotteryTrue.Count;
            if (countOfServer > 0)
            {
                int randomNumber = rnd.Next(0, countOfServer);
                Server serv = serversWithInLotteryTrue[randomNumber];
                _serverRepository.ServerLotteryWinner(serv);
            }
        }
    }
}
