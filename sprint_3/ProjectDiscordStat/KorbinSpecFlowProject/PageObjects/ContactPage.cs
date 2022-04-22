using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;
using System.Collections.ObjectModel;

namespace KorbinSpecFlowProject.PageObjects
{
    public class ContactPage : Page
    {
        private IWebElement Title => _browserInteractions.WaitAndReturnElement(By.Id("displayMessage"));

        private IWebElement WelcomeText => _browserInteractions.WaitAndReturnElement(By.Id("loggedin-welcome"));
        private IWebElement ContactDiv => _browserInteractions.WaitAndReturnElement(By.Id("contactInfo"));


        public ContactPage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.AccountPageName;
        }

        public string GetTitle => Title.Text;
        public string GetWelcomeText => WelcomeText.Text;
        public bool getContactDiv => ContactDiv.Displayed;


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