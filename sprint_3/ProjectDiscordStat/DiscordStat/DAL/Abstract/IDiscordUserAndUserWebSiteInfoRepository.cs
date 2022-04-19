using System.Collections.Generic;
using DiscordStats.Models;

namespace DiscordStats.DAL.Abstract
{
    public interface IDiscordUserAndUserWebSiteInfoRepository : IRepository<DiscordUserAndUserWebSiteInfo>
    {
        IEnumerable<DiscordUserAndUserWebSiteInfo> GetDiscordUsers();

    }
}
