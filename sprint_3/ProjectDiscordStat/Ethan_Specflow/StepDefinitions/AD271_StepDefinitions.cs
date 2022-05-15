using Ethan_Specflow.PageObjects;

namespace Ethan_Specflow.StepDefinitions
{
    [Binding]
    public sealed class AD271_StepDefinitions
    {
        private readonly HomePage _homePage;
        private readonly AccountPage _accountPage;
        private readonly ContactPage _contactPage;
        private readonly PrivacyPage _privacyPage;
        private readonly AllServersPage _allServersPage;
        private readonly DetailsPage _detailPage;

        public AD271_StepDefinitions(HomePage Homepage, AccountPage accountPage, ContactPage contactPage, PrivacyPage privacyPage, AllServersPage allServersPage, DetailsPage detailsPage)
        {
            _homePage = Homepage;
            _accountPage = accountPage;
            _contactPage = contactPage;
            _privacyPage = privacyPage;
            _allServersPage = allServersPage;
            _detailPage = detailsPage;

        }

        [Then(@"I will see the statuses graph")]
        public void ThenIWillSeeTheStatusesGraph()
        {
            _detailPage.GetStatusGraph.Should().BeTrue();
        }

    }
}
