Feature: SprintFive User Stories.


Scenario: Login a user with cookie
	Given I am on the Homepage
	When I login
	Then I should be logged in

Scenario: Logged in User can see option for Lottery
	Given I login
	When I go to my servers page
	Then I will see the drawing column

Scenario: When on home page can see the featured server
	Given I am on the Homepage
	Then I will see the featured server area

Scenario: When in the account page can see the website profile info location
	Given I login
	When I go to my account page
	Then I will see the profile info location

Scenario: After I click on profile edit button I'll see the profile info form
	Given I login
	When I go to my account page
	When I click on edit profile button
	Then I will see the profile edit form
	
Scenario: After I click on submit profile form button I'll see the Account page Welcome
	Given I login
	When I go to my account page
	When I click on edit profile button
	When I will see the profile edit form
	When I click on submit profile info form
	Then I will see the account page welcome


Scenario: When I go to My servers page I'll see the server channels column
	Given I login
	When I go to my servers page
	Then I will see the server channels column

