using System.Collections.Generic;
using System.Linq;
using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.DAL.Concrete
{
    public class VoiceStateRepository : Repository<VoiceState>, IVoiceStateRepository
    {
        public VoiceStateRepository(DiscordDataDbContext ctx) : base(ctx)
        {
        }
    }
}
