using System.Collections.Generic;
using DiscordStats.Models;
using DiscordStats.ViewModel;

namespace DiscordStats.DAL.Abstract
{
    public interface IDiscordUserAndUserWebSiteInfoRepository : IRepository<DiscordUserAndUserWebSiteInfo>
    {
        IEnumerable<DiscordUserAndUserWebSiteInfo> GetDiscordUsers();

        void UpdateWebsiteProfileInfo(ServerAndDiscordUserInfoAndWebsiteProfileVM profileInfo);

    }
}
