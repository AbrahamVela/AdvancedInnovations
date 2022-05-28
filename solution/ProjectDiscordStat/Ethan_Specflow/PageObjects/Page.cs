﻿using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;

namespace Ethan_Specflow.PageObjects
{
    // Base class for all pages, provides common functionality
    public class Page
    {
        protected readonly IBrowserInteractions _browserInteractions;

        public Page(IBrowserInteractions browserInteractions)
        {
            _browserInteractions = browserInteractions;
        }

        protected string PageName { get; set; } = Common.HomePageName;

        // Goto this page.  Preferred way for derived classes
        public virtual void Goto()
        {
            //Common.UrlFor(PageName)
            _browserInteractions.GoToUrl("https://www.google.com/");
        }

        // If you need to just go to a named page
        public virtual void Goto(string pageName)
        {
            _browserInteractions.GoToUrl(Common.UrlFor(pageName));
        }

        // Not ideal to have the page itself return the URL but the page is currently our only
        // access to the browser, so for now put it here.  i.e. each page is a proxy for the browser
        public virtual string GetCurrentUrl()
        {
            return _browserInteractions.GetUrl();
        }
    }
}