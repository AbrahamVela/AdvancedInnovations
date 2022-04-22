using AbrahamSpecFlowProject.PageObjects;
using System;
using TechTalk.SpecFlow;

namespace AbrahamSpecFlowProject.StepDefinitions
{
    [Binding]
    public class HomepageStepDefinitions
    {
        private readonly HomePage _homePage;

        public HomepageStepDefinitions(HomePage page)
        {
            _homePage = page;
        }

        //[Given(@"I am on the Homepage")]
        //public void CheckHomePage()
        //{
        //    _homePage.Goto(Common.HomePageName);
        //    var check = true;
        //    _homePage.SaveAllCookies();
        //}


        [Given(@"I am on the Homepage")]
        public void GivenIAmOnTheHomepage()
        {
            _homePage.Goto(Common.HomePageName);
            //_homePage.LoadAllCookies();
            var welcomeMessage = _homePage.GetTitle;
            welcomeMessage.Should().Equals("WELCOME");
        }

        [When(@"I login")]
        public void WhenILogin()
        {
            _homePage.LoadAllCookies();
        }

        [Then(@"I should be logged in")]
        public void ThenIShouldBeLoggedIn()
        {
            var ServersNav = _homePage.GetServerNav;
            ServersNav.Should().Equals("My Servers");
        }

        [Given(@"I login")]
        public void GivenILogin()
        {
            throw new PendingStepException();
        }

        [When(@"I go to my account page")]
        public void WhenIGoToMyAccountPage()
        {
            throw new PendingStepException();
        }

        [Then(@"I will see the servers I am in")]
        public void ThenIWillSeeTheServersIAmIn()
        {
            throw new PendingStepException();
        }
    }
}

