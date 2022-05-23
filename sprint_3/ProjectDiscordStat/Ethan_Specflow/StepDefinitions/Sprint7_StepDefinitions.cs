using Ethan_Specflow.PageObjects;

namespace Ethan_Specflow.StepDefinitions
{
    [Binding]
    public sealed class Sprint7_StepDefinitions
    {
        private readonly HomePage _homePage;
        private readonly AccountPage _accountPage;
        private readonly ContactPage _contactPage;
        private readonly PrivacyPage _privacyPage;
        private readonly AllServersPage _allServersPage;
        private readonly DetailsPage _detailPage;

        public Sprint7_StepDefinitions(HomePage Homepage, AccountPage accountPage, ContactPage contactPage, PrivacyPage privacyPage, AllServersPage allServersPage, DetailsPage detailsPage)
        {
            _homePage = Homepage;
            _accountPage = accountPage;
            _contactPage = contactPage;
            _privacyPage = privacyPage;
            _allServersPage = allServersPage;
            _detailPage = detailsPage;

        }

        [Then(@"I will see the reactions graph")]
        public void ThenIWillSeeTheReactionsGraph()
        {
            _detailPage.GetReactionGraph.Should().BeTrue();
        }

        [Then(@"I will see the emojis graph")]
        public void ThenIWillSeeTheEmojisGraph()
        {
            _detailPage.GetEmojiGraph.Should().BeTrue();
        }

        [Then(@"I will see the role column")]
        public void ThenIWillSeeTheRoleColumn()
        {
            _detailPage.GetRoleColumn.Should().BeTrue();
        }

        [Then(@"I will see the activity status graph")]
        public void ThenIWillSeeTheActivityStatusGraph()
        {
            _detailPage.GetActivityStatusGraph.Should().BeTrue();
        }
    }
}