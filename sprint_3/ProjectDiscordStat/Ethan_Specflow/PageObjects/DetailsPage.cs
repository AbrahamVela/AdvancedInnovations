using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;
using System.Collections.ObjectModel;

namespace Ethan_Specflow.PageObjects
{
    public class DetailsPage : Page
    {
        private IWebElement Title => _browserInteractions.WaitAndReturnElement(By.Id("displayMessage"));
        private IWebElement VoiceGraph => _browserInteractions.WaitAndReturnElement(By.Id("usersVoiceHourlyAllTimeChart"));
        private IWebElement MessageGraph => _browserInteractions.WaitAndReturnElement(By.Id("usersHourlyAllTimeChart"));
        private IWebElement WelcomeText => _browserInteractions.WaitAndReturnElement(By.Id("loggedin-welcome"));
        private IWebElement MessagesTable => _browserInteractions.WaitAndReturnElement(By.Id("serverTable"));
        private IWebElement DetailsContainer => _browserInteractions.WaitAndReturnElement(By.Id("DetailsContainer"));
        private IWebElement GamesButton => _browserInteractions.WaitAndReturnElement(By.Id("ViewGamesButton"));
        private IWebElement Runelite_Image => _browserInteractions.WaitAndReturnElement(By.Id("RuneLite"));
        private IWebElement RuneliteActivityGraph => _browserInteractions.WaitAndReturnElement(By.Id("RuneLite"));
        private IWebElement UsersGameDropdownBox => _browserInteractions.WaitAndReturnElement(By.Id("allUsersPresences"));
        private IWebElement UsersDropdownBox => _browserInteractions.WaitAndReturnElement(By.Id("allUsers"));
        private IWebElement GameGraph => _browserInteractions.WaitAndReturnElement(By.Id("presencesHourlyAllTimeChart"));
        private IWebElement AllGameGraph => _browserInteractions.WaitAndReturnElement(By.Id("allPresenceHourlyAllTimeChart"));
        private IWebElement GameActivityGraph => _browserInteractions.WaitAndReturnElement(By.Id("userPresenceHourlyAllTimeChart"));
        private IWebElement StatusGraph => _browserInteractions.WaitAndReturnElement(By.Id("usersStatusHourlyChart"));
        private IWebElement EmojiGraph => _browserInteractions.WaitAndReturnElement(By.Id("emojiStats"));
        private IWebElement ReactionGraph => _browserInteractions.WaitAndReturnElement(By.Id("reactionStats"));
        private IWebElement ActivityStatusGraph => _browserInteractions.WaitAndReturnElement(By.Id("usersStatusHourlyChart"));
        private IWebElement RoleColumn => _browserInteractions.WaitAndReturnElement(By.Id("roleId"));

        public DetailsPage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.AccountPageName;
        }

        public bool GetEmojiGraph => EmojiGraph.Displayed;
        public bool GetReactionGraph => ReactionGraph.Displayed;
        public bool GetActivityStatusGraph => ActivityStatusGraph.Displayed;
        public bool GetRoleColumn => RoleColumn.Displayed;
        public bool GetGameGraph => GameGraph.Displayed;
        public bool GetAllGameGraph => AllGameGraph.Displayed;
        public bool GetStatusGraph => StatusGraph.Displayed;
        public bool GetGameActivityGraph => GameActivityGraph.Displayed;
        public bool GetUsersGameDropdownBox => UsersGameDropdownBox.Displayed;
        public bool GetUsersDropdownBox => UsersDropdownBox.Displayed;
        public bool GetRunelite_Image => Runelite_Image.Displayed;
        public bool GetRuneliteActivityGraph => RuneliteActivityGraph.Displayed;
        public string GetDetailsContainer => DetailsContainer.Text;
        public bool GetVoiceGraph => VoiceGraph.Displayed;
        public bool GetMessageGraph => MessageGraph.Displayed;
        public string GetTitle => Title.Text;
        public string GetWelcomeText => WelcomeText.Text;
        public bool GetMessagingTable => MessagesTable.Displayed;

        public void ClickOnRunelite()
        {
            Runelite_Image.ClickWithRetry(30);
        }
        public void ClickGames()
        {
            GamesButton.ClickWithRetry(30);
        }
        public void SaveAllCookies()
        {
            ReadOnlyCollection<Cookie> cookies = _browserInteractions.GetCookies();
            FileUtils.SerializeCookiesToFile(Common.CookieFile, cookies);
        }

        public void LoadAllCookies()
        {
            List<Cookie> cookies = FileUtils.DeserializeCookiesFromFile(Common.CookieFile);
            foreach (Cookie cookie in cookies)
            {
                _browserInteractions.AddCookie(cookie);
            }
            _browserInteractions.RefreshPage();
        }

        public void DeleteCookies()
        {
            _browserInteractions.DeleteAllCookies();
        }

    }
}