using DiscordStats.DAL.Abstract;
using DiscordStats.Models;

namespace DiscordStats.DAL.Concrete
{
    public class ServerMemberRepository : Repository<ServerMembers>, IServerMemberRepository
    {
        public ServerMemberRepository(DiscordDataDbContext ctx) : base(ctx)
        {
        }
    }
}
