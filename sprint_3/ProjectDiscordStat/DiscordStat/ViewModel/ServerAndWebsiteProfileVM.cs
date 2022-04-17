using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using DiscordStats.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DiscordStats.ViewModel
{
    public class ServerAndDiscordUserInfoAndWebsiteProfileVM
    {
        public string id { get; set; }
        public List<Server> Servers { get; set; }

        public string ProfileFirstName { get; set; }

        public string ProfileLastName { get; set; }

        [DataType(DataType.Date)]
        [RegularExpression(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$", ErrorMessage = "Not a valid birthdate")]
        public string ProfileBirthDate { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"(@)(.+)$", ErrorMessage = "Not a valid Email")]
        public string ProfileEmail { get; set; }

    }

    public class WebsiteProfileUpdate
    {

        private readonly IDiscordUserRepository _discordUserRepository;

        public WebsiteProfileUpdate(IDiscordUserRepository discordUserRepository)
        {

            _discordUserRepository = discordUserRepository;
        }

        public void UpdateWebsiteProfileInfoInDiscordUserAndSiteInfoDb(DiscordUserAndUserWebSiteInfo userInputProfileForm)
        {
            var discordUserAndWebsite = _discordUserRepository.GetAll();
            foreach (var websiteUser in discordUserAndWebsite)
            {
                var duplicate = false;

                if (websiteUser.Id == userInputProfileForm.Id)
                {
                    duplicate = true;
                }

                if (!duplicate)
                {
                    _discordUserRepository.AddOrUpdate(websiteUser);
                }

            }
        }
    }

}

