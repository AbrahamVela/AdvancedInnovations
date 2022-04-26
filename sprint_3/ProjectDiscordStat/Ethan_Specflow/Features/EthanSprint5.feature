Feature: As a user, I would like to choose dates to go between different days on a calendar for the presence frequency graph, message frequency graph, and voice activity graph so that I can see specific times that people on my server are active


Scenario: Login a user with cookie
	Given I am on the Homepage
	When I login
	Then I should be logged in

Scenario: View Games graph played on a server
	Given I login
	When I go to my account page
	When I click on the server details page
	When I click on the view games button
	When I click on game
	Then I will see the presence graph

Scenario: View User Dropdown on game page
	Given I login
	When I go to my account page
	When I click on the server details page
	When I click on the view games button
	When I click on game
	Then I will see the users games dropdown box

Scenario: View users dropdown on server details page
	Given I login
	When I go to my account page
	When I click on the server details page
	Then I will see the users dropdown box

Scenario: View message activity graph on server details page
	Given I login
	When I go to my account page
	When I click on the server details page
	Then I will see the message activity graph

Scenario: View voice activity graph on server details page
	Given I login
	When I go to my account page
	When I click on the server details page
	Then I will see the voice activity graph


Scenario: View all games graph on server details page
	Given I login
	When I go to my account page
	When I click on the server details page
	Then I will see the all games graph


Scenario: View presence activity graph on server details page
	Given I login
	When I go to my account page
	When I click on the server details page
	Then I will see the game activity graph
