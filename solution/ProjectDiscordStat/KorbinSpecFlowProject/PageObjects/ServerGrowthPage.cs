using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;
using System.Collections.ObjectModel;

namespace KorbinSpecFlowProject.PageObjects
{
    public class ServerGrowthPage : Page
    {
        private IWebElement Title => _browserInteractions.WaitAndReturnElement(By.Id("displayMessage"));

        private IWebElement WelcomeText => _browserInteractions.WaitAndReturnElement(By.Id("loggedin-welcome"));
        private IWebElement privacyDiv => _browserInteractions.WaitAndReturnElement(By.Id("privacyInfo"));

        private IWebElement UsersGraph => _browserInteractions.WaitAndReturnElement(By.Id("ServerGrowthAllTimeChart"));
        public ServerGrowthPage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.AccountPageName;
        }
        public bool getUsersGraph => UsersGraph.Displayed;
        public string GetTitle => Title.Text;
        public string GetWelcomeText => WelcomeText.Text;
        public bool getPrivacyDiv => privacyDiv.Displayed;


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