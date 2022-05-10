using System.Collections.Generic;
using System.Linq;
using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.DAL.Concrete
{
    public class StatusRepository : Repository<Status>, IStatusRepository
    {
        public StatusRepository(DiscordDataDbContext ctx) : base(ctx)
        {
        }
    }
}