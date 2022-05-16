using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;
using System.Collections.ObjectModel;
using System;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Threading;
using OpenQA.Selenium.Support.UI;

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

        private IWebElement DownloadContainer => _browserInteractions.WaitAndReturnElement(By.Id("dowwnloadContainer"));

        private IWebElement ExportJsonVoiceChannel => _browserInteractions.WaitAndReturnElement(By.Id("exportJsonVoiceChannel"));

        private IWebElement ExportJsonMessaging => _browserInteractions.WaitAndReturnElement(By.Id("exportJsonMessaging"));

        private IWebElement ExportJsonActiveGaming => _browserInteractions.WaitAndReturnElement(By.Id("exportJsonActiveGaming"));

        private IWebElement ExportJsonHoursPerGame => _browserInteractions.WaitAndReturnElement(By.Id("exportJsonHoursPerGame"));

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

        public bool GetDownloadContainer => DownloadContainer.Displayed;

        public void ClickExportJsonVoiceChannelButton()
        {
            ExportJsonVoiceChannel.ClickWithRetry(30);
        }

        public void ClickExportJsonMessagingButton()
        {
            ExportJsonMessaging.ClickWithRetry(30);
        }
        public void ClickExportJsonActiveGamingButton()
        {
            ExportJsonActiveGaming.ClickWithRetry(30);
        }
        public void ClickExportJsonHoursPerGameButton()
        {
            ExportJsonHoursPerGame.ClickWithRetry(30);
        }
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

        public void VerifyFileActiveVoiceChannelTimeExists()
        {
            string expectedFilePath = @"C:\Users\Abraham\Downloads\ActiveVoicChannelTime.json";
            bool fileExists = false;

            ClickExportJsonVoiceChannelButton();
            var something = File.Exists(expectedFilePath);

            //Until<bool>(c => fileExists = File.Exists(expectedFilePath));

        }

        //public void VerifyFileActiveVoiceChannelTimeExists()
        //{
        //    IWebDriver driver;
        //    ChromeOptions capability = new ChromeOptions();
        //    capability.AddAdditionalCapability("browser", "Chrome", true);
        //    capability.AddAdditionalCapability("browser_version", "latest", true);
        //    capability.AddAdditionalCapability("os", "Windows", true);
        //    capability.AddAdditionalCapability("os_version", "10", true);
        //    capability.AddAdditionalCapability("browserstack.user", "YOUR_USERNAME", true);
        //    capability.AddAdditionalCapability("browserstack.key", "YOUR_ACCESS_KEY", true);
        //    capability.AddAdditionalCapability("name", "Bstack-[C_sharp] Sample file download", true);

        //    driver = new RemoteWebDriver(
        //        new Uri("http://hub-cloud.browserstack.com/wd/hub/"), capability
        //    );
        //    IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;

        //    driver.Navigate().GoToUrl("https://www.browserstack.com/test-on-the-right-mobile-devices");
        //    Thread.Sleep(2000);

        //    driver.FindElement(By.Id("accept-cookie-notification")).Click();

        //    // Find element by link text and store in variable "Element"             
        //    IWebElement Element = driver.FindElement(By.ClassName("exportJsonVoiceChannel"));

        //    // This will scroll the page till the element is found       
        //    jse.ExecuteScript("arguments[0].scrollIntoView();", Element);
        //    jse.ExecuteScript("window.scrollBy(0,-100)");
        //    Thread.Sleep(2000);

        //    Element.Click();
        //    Thread.Sleep(2000);

        //    driver.Quit();

        //}

    }
}