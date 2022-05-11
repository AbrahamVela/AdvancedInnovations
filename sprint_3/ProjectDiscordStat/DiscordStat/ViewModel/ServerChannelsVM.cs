using DiscordStats.DAL.Abstract;
using DiscordStats.Models;

namespace DiscordStats.ViewModel
{
    public class ChannelsFromDatabase
    {
        private readonly IChannelRepository _channelRepository;

        public ChannelsFromDatabase(IChannelRepository channelRepository)
        {
            _channelRepository = channelRepository;
        }


        public List<ServerChannelsVM> ServersWithCount(List<ServerChannelsVM> channels)
        {
            
            foreach (var channel in channels)
            {
                foreach (var cha in _channelRepository.GetAll().ToList())
                {
                    if (channel.id == cha.Id)
                    {
                        channel.count = cha.Count;
                    }
                }

            }
            return channels;
        }
 
    }

    public class ServerChannelsVM
    {
        public string id { get; set; }

        public string type { get; set; }
        public bool type_text { get; set; }
        public bool type_voice { get; set; }
        public string name { get; set; }

        public string parent_id { get; set; }

        public string guild_id { get; set; }

        public int? count { get; set; }

        public List<ServerChannelsVM> serverChannels { get; set; }

    }

}
