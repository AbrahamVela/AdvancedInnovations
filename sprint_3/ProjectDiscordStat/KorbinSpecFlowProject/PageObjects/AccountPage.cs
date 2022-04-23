using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;
using System.Collections.ObjectModel;

namespace KorbinSpecFlowProject.PageObjects
{
    public class AccountPage : Page
    {
        private IWebElement Title => _browserInteractions.WaitAndReturnElement(By.Id("displayMessage"));
        
        private IWebElement WelcomeText => _browserInteractions.WaitAndReturnElement(By.Id("loggedin-welcome"));
        private IEnumerable<IWebElement> AppleButtons => _browserInteractions.WaitAndReturnElements(By.CssSelector("#listOfApples button"));
        private IWebElement logoutButton => _browserInteractions.WaitAndReturnElement(By.Id("logoutButton"));
        private IWebElement UserServers => _browserInteractions.WaitAndReturnElement(By.Id("ServersBlock"));
        public AccountPage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.AccountPageName;
        }
        
        public string GetlogoutButton => logoutButton.Text;
        public bool GetServers => UserServers.Enabled;
        public string GetTitle => Title.Text;
        public string GetWelcomeText => WelcomeText.Text;

        public string GetAppleButtonText(int index) => AppleButtons.ElementAt(index).Text;

        public IEnumerable<string> GetAppleButtonTexts() => AppleButtons.Select(x => x.Text);

        public void ClickAppleButton(int index)
        {
            AppleButtons.ElementAt(index).Click();
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