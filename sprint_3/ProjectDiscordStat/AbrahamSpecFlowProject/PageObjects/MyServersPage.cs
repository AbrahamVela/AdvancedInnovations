using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;
using System.Collections.ObjectModel;

namespace AbrahamSpecFlowProject.PageObjects
{
    public class MyServersPage : Page
    {
        private IWebElement Title => _browserInteractions.WaitAndReturnElement(By.Id("displayMessage"));

        private IWebElement WelcomeText => _browserInteractions.WaitAndReturnElement(By.Id("loggedin-welcome"));
        private IWebElement AllServersDiv => _browserInteractions.WaitAndReturnElement(By.Id("AllServerOnSite"));
        private IWebElement DrawingSpot => _browserInteractions.WaitAndReturnElement(By.Id("drawingSpot"));
        private IWebElement ServerChannelsCol => _browserInteractions.WaitAndReturnElement(By.Id("serverChannels"));
        private IWebElement ServerChannelsForInputOutput => _browserInteractions.WaitAndReturnElement(By.Id("789317480325316640"));


        public MyServersPage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.AccountPageName;
        }

        public string GetTitle => Title.Text;
        public string GetWelcomeText => WelcomeText.Text;
        public bool getAllServersDiv => AllServersDiv.Displayed;
  
        public bool GetDrawingSpot => DrawingSpot.Displayed;
        public bool GetServerChannelsCol => ServerChannelsCol.Displayed;

        public bool GetServerChannelsForInputOutput => ServerChannelsForInputOutput.Displayed;

        public void ClickOnInputOutput()
        {
            ServerChannelsForInputOutput.ClickWithRetry(30);
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