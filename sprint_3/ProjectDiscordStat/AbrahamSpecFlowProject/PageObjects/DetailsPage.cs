using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;
using System.Collections.ObjectModel;

namespace AbrahamSpecFlowProject.PageObjects
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
        private IWebElement GameGraph => _browserInteractions.WaitAndReturnElement(By.Id("presencesHourlyAllTimeChart"));
        
        public DetailsPage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.AccountPageName;
        }

        public bool GetGameGraph => GameGraph.Displayed;
        public bool GetRunelite_Image => Runelite_Image.Displayed;
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