using AbrahamSpecFlowProject.PageObjects;
using System;
using TechTalk.SpecFlow;

namespace AbrahamSpecFlowProject.StepDefinitions
{
    [Binding]
    public class SprintSevenUserStories_StepDefinitions
    {

        private readonly AccountPage _accountPage;

        public SprintSevenUserStories_StepDefinitions(AccountPage accountPage)
        {

            _accountPage = accountPage;

        }

        [Then(@"I will know file has been downloaded")]
        public void ThenIWillKnowFileHasBeenDownloaded()
        {
          _accountPage.VerifyFileActiveVoiceChannelTimeExists();
          
        }
    }
}
