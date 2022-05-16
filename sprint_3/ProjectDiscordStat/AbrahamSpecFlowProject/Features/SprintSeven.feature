Feature: SprintSeven User Stories.

Scenario: Login a user with cookie
	Given I am on the Homepage
	When I login
	Then I should be logged in

Scenario: Logged in User can see container for download button
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	Then I will see the download container

Scenario: When in the account page can see Download Container click the Active Voice Channel Time Button I'll remain on the same page
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	When I will see the download container
	When I click on Active Voice Channel Time Button
	Then I will know file has been downloaded