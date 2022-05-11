using KorbinSpecFlowProject.PageObjects;

namespace KorbinSpecFlowProject.StepDefinitions
{
    [Binding]
    public sealed class AD232StepDefinitions
    {
        private readonly HomePage _homePage;
        private readonly AccountPage _accountPage;
        private readonly ContactPage _contactPage;
        private readonly PrivacyPage _privacyPage;
        private readonly AllServersPage _allServersPage;
        private readonly DetailsPage _detailPage;
        private readonly ServerGrowthPage _serverGrowthPage;

        public AD232StepDefinitions(HomePage Homepage, AccountPage accountPage, ContactPage contactPage, PrivacyPage privacyPage, AllServersPage allServersPage, DetailsPage detailsPage, ServerGrowthPage serverGrowthPage)
        {
            _homePage = Homepage;
            _accountPage = accountPage;
            _contactPage = contactPage;
            _privacyPage = privacyPage;
            _allServersPage = allServersPage;
            _detailPage = detailsPage;
            _serverGrowthPage = serverGrowthPage;

        }
        [When(@"I click on the server Growth button")]
        public void ClickOnServerDetailsButton()
        {
            _detailPage.ClickGrowthButton();
        }
        [Then(@"I will see the graph")]
        public void CheckForGraph()
        {
            var graph = _serverGrowthPage.getUsersGraph;
            graph.Should().BeTrue();
        }
        [Then(@"I will not see the growth button")]
        public void CheckForGrowthbutton()
        {
            var alertMessage = _accountPage.GetnoBotMessage;
            alertMessage.Should().BeTrue();
        }

    }
}
