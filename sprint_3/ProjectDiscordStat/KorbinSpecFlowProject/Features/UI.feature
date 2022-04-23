Feature: As a user I would like to have a clean and simple looking user interface that is easy to follow and looks professional, so that I feel welcome to the site and can follow along with all the features your site offers.


Scenario: Login a user with cookie
	Given I am on the Homepage
	When I login
	Then I should be logged in
	
Scenario: Logged in User can see servers they are in
	Given I login
	When I go to my account page
	Then I will see the servers I am in

Scenario: Users can view a contact page if they need help
	Given I am on the Homepage
	When I go to the contact page
	Then I will see contact information

Scenario: Users can view a privacy page for more info
	Given I am on the Homepage
	When I go to the privacy page
	Then I will see privacy information

Scenario: View all servers registered to the site
	Given I am on the Homepage
	When I login
	When I go to the AllServers page
	Then I should see all the servers on the site

Scenario: View Details Page of a server
	Given I login
	When I go to my account page
	When I click on the server details page
	Then I will see the information for that server

Scenario: View Games played on a server
	Given I login
	When I go to my account page
	When I click on the server details page
	When I click on the view games button
	Then I will see the games played on the server

Scenario: View Details Page for server without bot
	Given I login
	When I go to my account page
	When I click on the server details page without a bot
	Then I will see the an alert message