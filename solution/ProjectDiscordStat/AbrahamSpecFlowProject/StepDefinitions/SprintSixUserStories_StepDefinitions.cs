using System;
using TechTalk.SpecFlow;
using AbrahamSpecFlowProject.PageObjects;


namespace AbrahamSpecFlowProject.StepDefinitions
{
    [Binding]
    public class SprintSixUserStories_StepDefinitions
    {

        private readonly AccountPage _accountPage;

        public SprintSixUserStories_StepDefinitions( AccountPage accountPage)
        {

            _accountPage = accountPage;

        }

        [When(@"I click on Advance Innovations Details icon")]
        public void WhenIClickOnAlphaTestingServerDetailsIcon()
        {
            _accountPage.clickServerInfo();
        }


        [Then(@"I will see the download container")]
        public void ThenIWillSeeTheDownloadContainer()
        {
            _accountPage.GetDownloadContainer.Should().BeTrue();
        }

        [When(@"I will see the download container")]
        public void WhenIWillSeeTheDownloadContainer()
        {
            _accountPage.GetDownloadContainer.Should().BeTrue();
        }


        [When(@"I click on Active Voice Channel Time Button")]
        public void WhenIClickOnActiveVoiceChannelTimeButton()
        {
            _accountPage.ClickExportJsonVoiceChannelButton();
        }

        [When(@"I click on Active Messaging Time Button")]
        public void WhenIClickOnActiveMessagingTimeButton()
        {
            _accountPage.ClickExportJsonMessagingButton();
        }

        [When(@"I click on Active Gaming Time Button")]
        public void WhenIClickOnActiveGamingTimeButton()
        {
            _accountPage.ClickExportJsonActiveGamingButton();
        }

        [When(@"I click on Hours Per Game Button")]
        public void WhenIClickOnHoursPerGameButton()
        {
            _accountPage.ClickExportJsonHoursPerGameButton();
        }
    }
}
