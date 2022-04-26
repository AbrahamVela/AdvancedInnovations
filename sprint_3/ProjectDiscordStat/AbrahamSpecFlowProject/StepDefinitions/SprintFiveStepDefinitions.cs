using AbrahamSpecFlowProject.PageObjects;

namespace AbrahamSpecFlowProject.StepDefinitions
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
        private readonly MyServersPage _myServersPage;
        private readonly ServerChannelsPage _serverChannels;

        public UIStepDefinitions(HomePage Homepage, AccountPage accountPage, ContactPage contactPage, PrivacyPage privacyPage, AllServersPage allServersPage, DetailsPage detailsPage, MyServersPage serversPage, ServerChannelsPage serverChannels)
        {
            _homePage = Homepage;
            _accountPage = accountPage;
            _contactPage = contactPage;
            _privacyPage = privacyPage;
            _allServersPage = allServersPage;
            _detailPage = detailsPage;
            _myServersPage = serversPage;
            _serverChannels = serverChannels;
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


        //[Given(@"I am on the Homepage")]
        //public void CheckHomePage()
        //{
        //    _homePage.Goto(Common.HomePageName);
        //    var check = true;
        //    _homePage.SaveAllCookies();
        //}

        [When(@"I go to my servers page")]
        public void WhenIGoToMyServersPage()
        {
            _myServersPage.Goto(Common.MyServersPageName);
        }

        [Then(@"I will see the drawing column")]
        public void ThenIWillSeeTheDrawingColumn()
        {
            _myServersPage.GetDrawingSpot.Should().BeTrue();
        }

        [Then(@"I will see the featured server area")]
        public void ThenIWillSeeTheFeaturedServerArea()
        {
            _homePage.GetFeaturedServerLocation.Should().BeTrue();
        }

        [Given(@"I am on the Accountpage")]
        public void GivenIAmOnTheAccountpage()
        {
            _accountPage.Goto(Common.AccountPageName);
        }


        [Then(@"I will see the profile info location")]
        public void ThenIWillSeeTheProfileInfoLocation()
        {
            _accountPage.GetProfileInfo.Should().BeTrue();
        }

        [Given(@"I am on the Homepage")]
        public void CheckHomePage()
        {
            _homePage.Goto(Common.HomePageName);
            //var welcomeMessage = _homePage.GetTitle;
            //welcomeMessage.Should().Be("WELCOME");
        }

        [When(@"I click on edit profile button")]
        public void WhenIClickOnEditProfileButton()
        {
            _accountPage.ClickProfileButton();
        }

        [Then(@"I will see the profile edit form")]
        public void ThenIWillSeeTheProfileEditForm()
        {
            _accountPage.GetProfileForm.Should().BeTrue();
        }

        [When(@"I will see the profile edit form")]
        public void WhenIWillSeeTheProfileEditForm()
        {
            _accountPage.GetProfileForm.Should().BeTrue();
        }

        [When(@"I click on submit profile info form")]
        public void WhenIClickOnSubmitProfileInfoForm()
        {
            _accountPage.ClickSubmitProfileFormButton();
        }

        [Then(@"I will see the account page welcome")]
        public void ThenIWillSeeTheAccountPageWelcome()
        {
            _accountPage.GetTitleBool.Should().BeTrue();
        }

        [Then(@"I will see the server channels column")]
        public void ThenIWillSeeTheServerChannelsColumn()
        {
            _myServersPage.GetServerChannelsCol.Should().BeTrue();
        }

        [When(@"I click on the channelId")]
        public void WhenIClickOnTheChannelId()
        {
 

            var inputOutputId = _myServersPage.GetServerChannelsForInputOutput;
            inputOutputId.Should().BeTrue();
            _myServersPage.ClickOnInputOutput();
      
        }

        [Then(@"I will see the ServerChannel Container")]
        public void ThenIWillSeeTheServerChannelContainer()
        {
            _serverChannels.GetServerChannelsCon.Should().BeTrue();

        }
        

    }
}