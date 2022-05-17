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

        public SprintSevenUserStories_StepDefinitions(AccountPage accountPage, DetailsPage detailsPage, GamesPage gamesPage, GameDetailsPage gameDetailsPage)
        {
            _accountPage = accountPage;
            _detailPage = detailsPage;  
            _gamesPage = gamesPage;
            _gameDetailsPage = gameDetailsPage;
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




        [Then(@"I will know file has been downloaded")]
        public void ThenIWillKnowFileHasBeenDownloaded()
        {
          _accountPage.VerifyFileActiveVoiceChannelTimeExists();
          
        }
    }
}
