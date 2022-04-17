using System.Collections.Generic;
using System.Linq;
using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.DAL.Concrete
{
    public class DiscordUserRepository : Repository<DiscordUserAndUserWebSiteInfo>, IDiscordUserRepository
    {
        public DiscordUserRepository(DiscordDataDbContext ctx) : base(ctx)
        {
        }

        public IEnumerable<DiscordUserAndUserWebSiteInfo> GetDiscordUsers()
        {
            List<DiscordUserAndUserWebSiteInfo> discordusers = new List<DiscordUserAndUserWebSiteInfo>();
            foreach (var s in _dbSet)
            {
                discordusers.Add(s);
            }

            return discordusers;

        }
    }
}
