using AbrahamSpecFlowProject.PageObjects;
using System;
using TechTalk.SpecFlow;

namespace AbrahamSpecFlowProject.StepDefinitions
{
    [Binding]
    public class AD166UserStoryStepDefinitions
    {
        private readonly HomePage _homePage;

        public AD166UserStoryStepDefinitions(HomePage page)
        {
            _homePage = page;
        }

        [Given(@"I am on the ServerChannels page")]
        public void GivenIAmOnTheServerChannelsPage()
        {
            _homePage.Goto();
        }

        [When(@"I click on a channel")]
        public void WhenIClickOnAChannel()
        {
            throw new PendingStepException();
        }

        [Then(@"I'll see a list of webhooks")]
        public void ThenIllSeeAListOfWebhooks()
        {
            throw new PendingStepException();
        }

        [Given(@"I am on the ChannelWebhooks page")]
        public void GivenIAmOnTheChannelWebhooksPage()
        {
            throw new PendingStepException();
        }

        [When(@"I click on a webhook")]
        public void WhenIClickOnAWebhook()
        {
            throw new PendingStepException();
        }

        [Then(@"I'll see a send message form")]
        public void ThenIllSeeASendMessageForm()
        {
            throw new PendingStepException();
        }

        [When(@"there are no webhooks")]
        public void WhenThereAreNoWebhooks()
        {
            throw new PendingStepException();
        }

        [Then(@"I'll see an option to create webhook")]
        public void ThenIllSeeAnOptionToCreateWebhook()
        {
            throw new PendingStepException();
        }

        [When(@"I click on Create Webhook")]
        public void WhenIClickOnCreateWebhook()
        {
            throw new PendingStepException();
        }

        [Then(@"I'll see make webhook form")]
        public void ThenIllSeeMakeWebhookForm()
        {
            throw new PendingStepException();
        }
    }
}
