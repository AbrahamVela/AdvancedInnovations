Feature: AD232

Scenario: View the members growth chart
	Given I am on the Homepage
	When I login
	When I go to my account page
	When I click on the server details page
	When I click on the server Growth button
	Then I will see the graph

Scenario: Details Page without bot
	Given I am on the Homepage
	When I login
	When I go to my account page
	When I click on the server details page without a bot
	Then I will not see the growth button