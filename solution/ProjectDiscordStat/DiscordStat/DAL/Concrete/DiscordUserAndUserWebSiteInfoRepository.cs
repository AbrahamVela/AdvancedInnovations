using System.Collections.Generic;
using System.Linq;
using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using DiscordStats.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.DAL.Concrete
{
    public class DiscordUserAndUserWebSiteInfoRepository : Repository<DiscordUserAndUserWebSiteInfo>, IDiscordUserAndUserWebSiteInfoRepository
    {
        public DiscordUserAndUserWebSiteInfoRepository(DiscordDataDbContext ctx) : base(ctx)
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

        public void UpdateWebsiteProfileInfo(ServerAndDiscordUserInfoAndWebsiteProfileVM profileInfo)
        {

            var userNotIn = false;

            DiscordUserAndUserWebSiteInfo user = new();

            foreach (var websiteUser in _dbSet.ToList())
            {
                if (websiteUser.Id != profileInfo.id)
                {
                    userNotIn = true;
                }
                if (websiteUser.Id == profileInfo.id)
                {
                    userNotIn = false;
                    if (profileInfo.ProfileFirstName != null)
                    {
                        websiteUser.FirstName = profileInfo.ProfileFirstName;
                        AddOrUpdate(websiteUser);
                    }
                    if (profileInfo.ProfileLastName != null)
                    {
                        websiteUser.LastName = profileInfo.ProfileLastName;
                        AddOrUpdate(websiteUser);
                    }
                    if (profileInfo.ProfileBirthDate != null)
                    {
                        websiteUser.BirthDate = profileInfo.ProfileBirthDate;
                        AddOrUpdate(websiteUser);
                    }
                    if(profileInfo.ProfileEmail != null)
                    {
                        websiteUser.Email = profileInfo.ProfileEmail;
                        AddOrUpdate(websiteUser);
                    }
                }

            }

            if (userNotIn == true || _dbSet.ToList().Count() == 0)
            {
                user.Id = profileInfo.id;
                user.FirstName = profileInfo.ProfileFirstName;
                user.LastName = profileInfo.ProfileLastName;
                user.BirthDate = profileInfo.ProfileBirthDate;
                user.Email = profileInfo.ProfileEmail;
                AddOrUpdate(user);
            }
        }
    }
}
