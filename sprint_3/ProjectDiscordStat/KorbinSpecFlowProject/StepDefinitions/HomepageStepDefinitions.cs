using KorbinSpecFlowProject.PageObjects;

namespace KorbinSpecFlowProject.StepDefinitions
{
    [Binding]
    public sealed class HomepageStepDefinitions
    {
        private readonly HomePage _homePage;
        private readonly AccountPage _accountPage;
        

        public HomepageStepDefinitions(HomePage Homepage, AccountPage accountPage)
        {
            _homePage = Homepage;
            _accountPage = accountPage;

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
            var welcomeMessage = _homePage.GetTitle;
            welcomeMessage.Should().Equals("WELCOME");
        }

        [When(@"I login")]
        
        public void DiscordUserLoggedIn()
        {
          
            _homePage.LoadAllCookies();
        }
        [Given(@"I login")]
        public void DiscordUserLoggedInGiven()
        {
            
            _accountPage.Goto(Common.HomePageName);
            _accountPage.LoadAllCookies();
        }

        [Then(@"I should be logged in")]
        public void CheckForNavBar()
        {
            var ServersNav = _homePage.GetServerNav;
            ServersNav.Should().Equals("My Servers");
        }

        [When(@"I go to my account page")]
        public void GotToAccountPage()
        {
            _homePage.Goto(Common.HomePageName);
            _accountPage.LoadAllCookies();
            _accountPage.Goto(Common.AccountPageName);
        }
        [Then(@"I will see the servers I am in")]
        public void CheckToSeeIfOnAccountPage()
        {
            _accountPage.GetServers.Should().BeTrue();
        }

        //[Given(@"I am on the home page")]
        //public void GivenIAmOnTheHomePage()
        //{
        //    _homePage.Goto();
        //    _homePage.SaveAllCookies();
        //}





        //[Then(@"I can save all cookies")]
        //public void DiscordUserLoggeded()
        //{
        //    var check = true;
        //    check.Should().BeTrue();
        //}

        //[Then(@"I can see the graph")]
        //    public void ThenTheResultShouldBe()
        //    {
        //       int a = 1;

        //    }

    }
    }