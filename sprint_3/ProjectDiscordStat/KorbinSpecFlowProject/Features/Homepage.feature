Feature: As a user I want to be able to see all the games that have been played on the server I am a part of, so that I can find new games my friends are playing and see if anyone is playing my favorite game.


Scenario: Login a user with cookie
	Given I am on the Homepage
	When I login
	Then I should be logged in
	

Scenario: Logged in User can see servers they are in
	Given I login
	When I go to my account page
	Then I will see the servers I am in

Scenario: User can select a game
	Given I am on the AccountPage
	When I click on a game image
	Then I will be redirected to another page

Scenario: Load cookies for a given user
	Given I am a user with a Discord account
	And I am on the 'Account' page
	When I load a previous sessions cookies
		And I am on the 'Account' page
	Then I can see all the servers I am in