using AbrahamSpecFlowProject.PageObjects;
using System;
using TechTalk.SpecFlow;

namespace AbrahamSpecFlowProject.StepDefinitions
{
    [Binding]
    public class SprintSevenUserStories_StepDefinitions
    {

        private readonly AccountPage _accountPage;
        private readonly DetailsPage _detailPage;
        private readonly GamesPage _gamesPage;
        private readonly GameDetailsPage _gameDetailsPage;
        private readonly ServerGrowthPage _serverGrowthPage;

        public SprintSevenUserStories_StepDefinitions(AccountPage accountPage, DetailsPage detailsPage, GamesPage gamesPage, GameDetailsPage gameDetailsPage, ServerGrowthPage serverGrowthPage)
        {
            _accountPage = accountPage;
            _detailPage = detailsPage;
            _gamesPage = gamesPage;
            _gameDetailsPage = gameDetailsPage;
            _serverGrowthPage = serverGrowthPage;
        }

        [When(@"I click on View Games button")]
        public void WhenIClickOnViewGamesButton()
        {
            _detailPage.ClickGames();
        }

        [When(@"I click on RuneLite icon")]
        public void WhenIClickOnRuneLiteIcon()
        {
            _gamesPage.ClickRuneLiteButton();
        }

        [Then(@"I will scroll down and see Download area")]
        public void ThenIWillScrollDownAndSeeDownloadOptions()
        {
            _gameDetailsPage.GetGameDetialsDownloadArea.Should().BeTrue();
        }

        [Then(@"I will click json file type and I'll know it has been downloaded with right file type by filepath")]
        public void ThenIWillKnowFileHasBeenDownloadedWithRightFileType()
        {
            _gameDetailsPage.VerifyFileJsonMostPopularPlayTimeExists();
        }

        [Then(@"I will click csv file type and I'll know it has been downloaded with right file type by filepath")]
        public void ThenIWillClickCsvFileTypeAndIllKnowItHasBeenDownloadedWithRightFileTypeByFilepath()
        {
            _gameDetailsPage.VerifyFileCsvMostPopularPlayTimeExists();
        }

        [When(@"I click on Server Growth button")]
        public void WhenIClickOnServerGrowthButton()
        {
            _detailPage.ClickViewServerGrowthButton();
        }

        [Then(@"I will click Active Members in Server json file type and I'll know it has been downloaded with right file type by filepath")]
        public void ThenIWillClickActiveMembersInServerJsonFileTypeAndIllKnowItHasBeenDownloadedWithRightFileTypeByFilepath()
        {
            _serverGrowthPage.VerifyFileJsonActiveMembersInServerExists();
        }

        [Then(@"I will click Active Members in Server csv file type and I'll know it has been downloaded with right file type by filepath")]
        public void ThenIWillClickActiveMembersInServerCsvFileTypeAndIllKnowItHasBeenDownloadedWithRightFileTypeByFilepath()
        {
            _serverGrowthPage.VerifyFileCsvActiveMembersInServerExists();
        }


        [Then(@"I click on JSON button for Active Voice Channel time and I'll know it has been downloaded with right file type comparison")]
        public void ThenIClickOnJSONButtonForActiveVoiceChannelTimeButtonAndIllKnowItHasBeenDownloadedWithRightFileTypeComparison()
        {
            _detailPage.VerifyFileJsonActiveVoiceChannelTimeExists();
        }

        [Then(@"I click on CSV button for Active Voice Channel time and I'll know it has been downloaded with right file type comparison")]
        public void ThenIClickOnCSVButtonForActiveVoiceChannelTimeButtonAndIllKnowItHasBeenDownloadedWithRightFileTypeComparison()
        {
            _detailPage.VerifyFileCsvActiveVoiceChannelTimeExists();
        }

        [Then(@"I click on JSON button for Active Messaging time and I'll know it has been downloaded with right file type comparison")]
        public void ThenIClickOnJSONButtonForActiveMessagingTimeButtonAndIllKnowItHasBeenDownloadedWithRightFileTypeComparison()
        {
            _detailPage.VerifyFileJsonActiveMessagingExists();
        }

        [Then(@"I click on CSV button for Active Messaging time and I'll know it has been downloaded with right file type comparison")]
        public void ThenIClickOnCSVButtonForActiveMessagingTimeButtonAndIllKnowItHasBeenDownloadedWithRightFileTypeComparison()
        {
            _detailPage.VerifyFileCsvActiveMessagingTimeExists();
        }

        [Then(@"I click on JSON button for Active Gaming time and I'll know it has been downloaded with right file type comparison")]
        public void ThenIClickOnJSONButtonForActiveGamingTimeButtonAndIllKnowItHasBeenDownloadedWithRightFileTypeComparison()
        {
            _detailPage.VerifyFileJsonActiveGamingTimeExists();
        }

        [Then(@"I click on CSV button for Active Gaming time and I'll know it has been downloaded with right file type comparison")]
        public void ThenIClickOnCSVButtonForActiveGamingTimeButtonAndIllKnowItHasBeenDownloadedWithRightFileTypeComparison()
        {
            _detailPage.VerifyFileCsvActiveGamingTimeExists();
        }

        [Then(@"I click on JSON button for Hours Per Game and I'll know it has been downloaded with right file type comparison")]
        public void ThenIClickOnJSONButtonForHoursPerGameButtonAndIllKnowItHasBeenDownloadedWithRightFileTypeComparison()
        {
            _detailPage.VerifyFileJsonHoursPerGameExists();
        }

        [Then(@"I click on CSV button for Hours Per Game and I'll know it has been downloaded with right file type comparison")]
        public void ThenIClickOnCSVButtonForHoursPerGameButtonAndIllKnowItHasBeenDownloadedWithRightFileTypeComparison()
        {
            _detailPage.VerifyFileCsvHoursPerGameExists();
        }

        [Then(@"I click on JSON button for Statuses and I'll know it has been downloaded with right file type comparison")]
        public void ThenIClickOnJSONButtonForStatusesAndIllKnowItHasBeenDownloadedWithRightFileTypeComparison()
        {
            _detailPage.VerifyFileJsonStatusesExists();
        }

        [Then(@"I click on CSV button for Statuses and I'll know it has been downloaded with right file type comparison")]
        public void ThenIClickOnCSVButtonForStatusesAndIllKnowItHasBeenDownloadedWithRightFileTypeComparison()
        {
            _detailPage.VerifyFileCsvStatusesExists();
        }

        [Then(@"I will know file has been downloaded")]
        public void ThenIWillKnowFileHasBeenDownloaded()
        {
            _accountPage.VerifyFileActiveVoiceChannelTimeExists();
        }
    }
}
