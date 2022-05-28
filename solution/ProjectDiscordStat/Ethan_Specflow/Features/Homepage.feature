Feature: As a user I want to be able to see all the games that have been played on the server I am a part of, so that I can find new games my friends are playing and see if anyone is playing my favorite game.


Scenario: Login a user with cookie
    Given I am on the Homepage
    When I login
    Then I should be logged in
    

Scenario: Logged in User can see servers they are in
    Given I login
    When I go to my account page
    Then I will see the servers I am in