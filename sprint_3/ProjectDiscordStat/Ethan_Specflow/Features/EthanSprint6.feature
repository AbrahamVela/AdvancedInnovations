Feature: As a user, I want to see a graph that displays online members, offline members, idle members, and do not disturb members so that I can see how often users of my server are available and how active my server is.


Scenario: Login a user with cookie
	Given I am on the Homepage
	When I login
	Then I should be logged in

Scenario: View Statuses graph on server details page
	Given I login
	When I go to my account page
	When I click on the server details page
	Then I will see the statuses graph