Feature: As a user, I want to see the roles that are assigned to each member so that I know the most popular way people spend their time

Scenario: View role column on server details page
	Given I login
	When I go to my account page
	When I click on the server details page
	Then I will see the role column