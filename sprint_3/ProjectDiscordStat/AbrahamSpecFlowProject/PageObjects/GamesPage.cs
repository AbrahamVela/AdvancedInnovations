using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbrahamSpecFlowProject.PageObjects
{
    public class GamesPage : Page
    {
        private IWebElement RuneLite => _browserInteractions.WaitAndReturnElement(By.Id("RuneLite"));


        public GamesPage(IBrowserInteractions browserInteractions)
          : base(browserInteractions)
        {
            PageName = Common.AccountPageName;
        }

        public void ClickRuneLiteButton()
        {
            RuneLite.ClickWithRetry(30);
        }

    }
}
