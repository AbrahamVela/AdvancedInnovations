using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;
using System.Collections.ObjectModel;

namespace KorbinSpecFlowProject.PageObjects
{

    public class ForumPage : Page
    {
        
        private IWebElement MessageBox => _browserInteractions.WaitAndReturnElement(By.Id("MessageBox"));
        private IWebElement SumbitButton => _browserInteractions.WaitAndReturnElement(By.Id("addToForum"));
        public ForumPage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.ForumPageName;
        }

       
      
        public void typeIntoMessage()
        {
            MessageBox.SendKeys("hello");
        }
        public void clickSubmit()
        {
            SumbitButton.ClickWithRetry(30);
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