using KorbinSpecFlowProject.PageObjects;
namespace KorbinSpecFlowProject.StepDefinitions
{
    [Binding]
    public sealed class AD284StepDefinitions
    {
        private readonly HomePage _homePage;
        private readonly AccountPage _accountPage;
        private readonly ContactPage _contactPage;
        private readonly PrivacyPage _privacyPage;
        private readonly AllServersPage _allServersPage;
        private readonly DetailsPage _detailPage;
        private readonly ServerGrowthPage _serverGrowthPage;
        private readonly ForumPage _forumPage;

        public AD284StepDefinitions(HomePage Homepage, AccountPage accountPage, ContactPage contactPage, PrivacyPage privacyPage, AllServersPage allServersPage, DetailsPage detailsPage, ServerGrowthPage serverGrowthPage, ForumPage forumPage)
        {
            _homePage = Homepage;
            _accountPage = accountPage;
            _contactPage = contactPage;
            _privacyPage = privacyPage;
            _allServersPage = allServersPage;
            _detailPage = detailsPage;
            _serverGrowthPage = serverGrowthPage;
            _forumPage = forumPage;

        }
        [When(@"I go to the Forum page")]
        public void ClickOnServerDetailsButton()
        {
            _forumPage.Goto(Common.ForumPageName);
        }
        
        [When(@"I enter a message")]
        public void EnterMessage()
        {
            for(int i = 0; i< 15; ++i)
            {
                _forumPage.Goto(Common.ForumPageName);
            }
            
            _forumPage.typeIntoMessage();
            _forumPage.clickSubmit();
        }
    }
}
