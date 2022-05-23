Feature: As a user, I want to see a graph that shows the reactions being used in a server so that I can see the most popular reactions to messages

Scenario: View reactions graph on server details page
	Given I login
	When I go to my account page
	When I click on the server details page
	Then I will see the reactions graph