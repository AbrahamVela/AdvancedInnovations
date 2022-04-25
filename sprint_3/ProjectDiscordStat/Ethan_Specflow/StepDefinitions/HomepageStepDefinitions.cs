using Ethan_Specflow.PageObjects;

namespace Ethan_Specflow.StepDefinitions
{
 [Binding]
        public sealed class HomepageStepDefinitions
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
        public void CheckHomePage()
        {
            _homePage.Goto(Common.HomePageName);
            _homePage.LoadAllCookies();
            var welcomeMessage = _homePage.GetTitle;
            welcomeMessage.Should().Equals("WELCOME");
        }

        [When(@"I login")]

        public void DiscordUserLoggedIn()
        {

            _homePage.LoadAllCookies();
        }
        [Then(@"I should be logged in")]
        public void CheckForNavBar()
        {
            var ServersNav = _homePage.GetServerNav;
            ServersNav.Should().Equals("My Servers");
        }
    }
}