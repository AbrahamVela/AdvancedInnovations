using KorbinSpecFlowProject.PageObjects;

namespace KorbinSpecFlowProject.StepDefinitions
{
    [Binding]
    public sealed class UIStepDefinitions
    {
        private readonly HomePage _homePage;
        private readonly AccountPage _accountPage;
        private readonly ContactPage _contactPage;
        private readonly PrivacyPage _privacyPage;
        private readonly AllServersPage _allServersPage;
        private readonly DetailsPage _detailPage;

        public UIStepDefinitions(HomePage Homepage, AccountPage accountPage, ContactPage contactPage, PrivacyPage privacyPage, AllServersPage allServersPage, DetailsPage detailsPage)
        {
            _homePage = Homepage;
            _accountPage = accountPage;
            _contactPage = contactPage;
            _privacyPage = privacyPage;
            _allServersPage = allServersPage;
            _detailPage = detailsPage;

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
            _accountPage.Goto(Common.AccountPageName);            
        }

        [Then(@"I will see the servers I am in")]
        public void CheckToSeeIfOnAccountPage()
        {
            _accountPage.GetServers.Should().BeTrue();
        }
        [When(@"I go to the contact page")]

        public void GoToContactPage()
        {
            _contactPage.Goto(Common.ContactUsPageName);
        }
        [Then(@"I will see contact information")]
        public void CheckForContactInfo()
        {
            var contactDiv = _contactPage.getContactDiv;
            contactDiv.Should().BeTrue();
        }
        [When(@"I go to the privacy page")]
        public void GoToPrivacyPage()
        {
            _privacyPage.Goto(Common.PrivacyPageName);
        }
        [Then(@"I will see privacy information")]
        public void CheckForPrivacyInfo()
        {
            var privacyDiv = _privacyPage.getPrivacyDiv;
            privacyDiv.Should().BeTrue();
        }
        [When(@"I go to the AllServers page")]
        public void GoToAllServersPage()
        {
            _allServersPage.Goto(Common.AllServersPageName);
        }
        [Then(@"I should see all the servers on the site")]
        public void CheckForAllServersList()
        {
            var AllServersDiv = _allServersPage.getAllServersDiv;
            AllServersDiv.Should().BeTrue();
        }
        [When(@"I click on the server details page")]
        public void ClickOnDetailsButton()
        {
            _accountPage.clickNextButton();
            _accountPage.clickNextButton();
            _accountPage.clickServerInfo();
        }
        [Then(@"I will see the information for that server")]
        public void CheckServerInfo()
        {
            var VoiceGraph = _detailPage.GetVoiceGraph;
            var MessageGraph = _detailPage.GetMessageGraph;
            var MessagingList = _detailPage.GetMessagingTable;
            var DetailsContainer = _detailPage.GetDetailsContainer;

            DetailsContainer.Should().Equals("Details");
            MessagingList.Should().BeTrue();
            VoiceGraph.Should().BeTrue();
            MessageGraph.Should().BeTrue();
        }
        [When(@"I click on the view games button")]
        public void ClickOnGamesButton()
        {
            _detailPage.ClickGames();
        }
        [Then(@"I will see the games played on the server")]
        public void CheckForGamesBeingDisplayed()
        {
            var runeliteImage = _detailPage.GetRunelite_Image;
            runeliteImage.Should().BeTrue();
            _detailPage.ClickOnRunelite();
            var gameGraph = _detailPage.GetGameGraph;
            gameGraph.Should().BeTrue();

        }
        [When(@"I click on the server details page without a bot")]
        public void ClickOnDetailsButtonNoBot()
        {

            _accountPage.clickServerInfoNoBot();
        }
        [Then(@"I will see the an alert message")]
        public void CheckForAlertMessage()
        {
            var alertMessage = _accountPage.GetnoBotMessage;
            alertMessage.Should().BeTrue();
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