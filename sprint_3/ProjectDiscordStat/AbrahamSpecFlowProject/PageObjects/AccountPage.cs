using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;
using System.Collections.ObjectModel;

namespace AbrahamSpecFlowProject.PageObjects
{
    public class AccountPage : Page
    {
        private IWebElement Title => _browserInteractions.WaitAndReturnElement(By.Id("displayMessage"));
        
        private IWebElement WelcomeText => _browserInteractions.WaitAndReturnElement(By.Id("loggedin-welcome"));
        private IEnumerable<IWebElement> AppleButtons => _browserInteractions.WaitAndReturnElements(By.CssSelector("#listOfApples button"));
        private IWebElement logoutButton => _browserInteractions.WaitAndReturnElement(By.Id("logoutButton"));
        private IWebElement UserServers => _browserInteractions.WaitAndReturnElement(By.Id("ServersBlock"));
        private IWebElement ServerInfoButton => _browserInteractions.WaitAndReturnElement(By.Id("Advanced Innovations"));
        private IWebElement ServerInfoButtonNoBot => _browserInteractions.WaitAndReturnElement(By.Id("Pizza Pals"));
        private IWebElement noBotMessage => _browserInteractions.WaitAndReturnElement(By.Id("noBotMessage"));
        private IWebElement NextButton => _browserInteractions.WaitAndReturnElement(By.Id("serverTable_next"));

        private IWebElement ProfileInfo => _browserInteractions.WaitAndReturnElement(By.Id("websiteProfileInfoLocation"));

        private IWebElement profileInfo => _browserInteractions.WaitAndReturnElement(By.Id("profileInfo"));

        private IWebElement ProfileForm => _browserInteractions.WaitAndReturnElement(By.Id("profileForm"));

        private IWebElement SubmitProfileForm => _browserInteractions.WaitAndReturnElement(By.Id("submitProfileForm"));

        public AccountPage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.AccountPageName;
        }

        public bool GetnoBotMessage => noBotMessage.Displayed;
        public string GetlogoutButton => logoutButton.Text;
        public bool GetServers => UserServers.Enabled;
        public string GetTitle => Title.Text;
        public string GetWelcomeText => WelcomeText.Text;

        public bool GetProfileInfo => ProfileInfo.Displayed;

        public bool GetProfileForm => ProfileForm.Displayed;
        public bool GetTitleBool => Title.Displayed;

        public void ClickSubmitProfileFormButton()
        {
            SubmitProfileForm.ClickWithRetry(30);
        }

        public void ClickProfileButton()
        {
            profileInfo.ClickWithRetry(30);
        }

        public void clickNextButton()
        {
            NextButton.ClickWithRetry(30);
        }
        public void clickServerInfoNoBot()
        {
            ServerInfoButtonNoBot.ClickWithRetry(30);
        }
        public void clickServerInfo()
        {
           
            ServerInfoButton.ClickWithRetry(30);
        }
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