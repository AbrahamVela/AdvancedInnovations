Feature: As a user, I want to see a graph that shows emojis that are being messaged over time so that I can see the most popular emojis used in the server

Scenario: View emojis graph on server details page
	Given I login
	When I go to my account page
	When I click on the server details page
	Then I will see the emojis graph