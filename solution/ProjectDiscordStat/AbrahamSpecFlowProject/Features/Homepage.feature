Feature: Homepage
![Calculator](https://specflow.org/wp-content/uploads/2020/09/calculator.png)
Simple calculator for adding **two** numbers

Link to a feature: [Calculator](AbrahamSpecFlowProject/Features/Calculator.feature)
***Further read***: **[Learn more about how to generate Living Documentation](https://docs.specflow.org/projects/specflow-livingdoc/en/latest/LivingDocGenerator/Generating-Documentation.html)**

@mytag
Scenario: Login a user with cookie
    Given I am on the Homepage
    When I login
    Then I should be logged in
    

Scenario: Logged in User can see servers they are in
    Given I login
    When I go to my account page
    Then I will see the servers I am in