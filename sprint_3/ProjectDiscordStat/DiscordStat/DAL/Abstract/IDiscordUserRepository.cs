using System.Collections.Generic;
using DiscordStats.Models;

namespace DiscordStats.DAL.Abstract
{
    public interface IDiscordUserRepository : IRepository<DiscordUserAndUserWebSiteInfo>
    {
        IEnumerable<DiscordUserAndUserWebSiteInfo> GetDiscordUsers();

    }
}
