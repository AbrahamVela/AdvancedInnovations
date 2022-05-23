using Ethan_Specflow.PageObjects;

namespace Ethan_Specflow.StepDefinitions
{
    [Binding]
    public sealed class AD242_StepDefinitions
    {
        private readonly HomePage _homePage;
        private readonly AccountPage _accountPage;
        private readonly ContactPage _contactPage;
        private readonly PrivacyPage _privacyPage;
        private readonly AllServersPage _allServersPage;
        private readonly DetailsPage _detailPage;

        public AD242_StepDefinitions(HomePage Homepage, AccountPage accountPage, ContactPage contactPage, PrivacyPage privacyPage, AllServersPage allServersPage, DetailsPage detailsPage)
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
            _homePage.GetTitle.Should().Be("WELCOME");
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
            ServersNav.Should().Be("My Servers");
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
            _accountPage.clickServerInfo();
        }
        [Then(@"I will see the information for that server")]
        public void CheckServerInfo()
        {
            var VoiceGraph = _detailPage.GetVoiceGraph;
            var MessageGraph = _detailPage.GetMessageGraph;
            var MessagingList = _detailPage.GetMessagingTable;
            var DetailsContainer = _detailPage.GetDetailsContainer;

            DetailsContainer.Should().Be("Details");
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
        [When(@"I click on game")]
        public void WhenIClickOnGame()
        {
            _detailPage.ClickOnRunelite();
        }


        [Then(@"I will see the presence graph")]
        public void ThenIWillSeeThePresenceGraph()
        {
            var gameGraph = _detailPage.GetGameGraph;
            gameGraph.Should().BeTrue();
        }

        [Then(@"I will see the users games dropdown box")]
        public void ThenIWillSeeTheUsersGamesDropdownBox()
        {
            _detailPage.GetUsersGameDropdownBox.Should().BeTrue();
        }

        [Then(@"I will see the users dropdown box")]
        public void ThenIWillSeeTheUsersDropdownBox()
        {
            _detailPage.GetUsersDropdownBox.Should().BeTrue();
        }

        [Then(@"I will see the message activity graph")]
        public void ThenIWillSeeTheMessageActivityGraph()
        {
            _detailPage.GetMessageGraph.Should().BeTrue();
        }
        [Then(@"I will see the voice activity graph")]
        public void ThenIWillSeeTheVoiceActivityGraph()
        {
            _detailPage.GetVoiceGraph.Should().BeTrue();
        }
        [Then(@"I will see the all games graph")]
        public void ThenIWillSeeTheAllGamesGraph()
        {
            _detailPage.GetAllGameGraph.Should().BeTrue();
        }
        [Then(@"I will see the game activity graph")]
        public void ThenIWillSeeTheGameActivityGraph()
        {
            _detailPage.GetGameActivityGraph.Should().BeTrue();
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

    }
}