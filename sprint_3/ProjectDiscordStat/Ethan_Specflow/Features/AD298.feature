Feature: As a user, I want to see a graph that shows the activity status of each member so that I know the most popular ways members spend their time

Scenario: View activity status graph on server details page
	Given I login
	When I go to my account page
	When I click on the server details page
	Then I will see the activity status graph