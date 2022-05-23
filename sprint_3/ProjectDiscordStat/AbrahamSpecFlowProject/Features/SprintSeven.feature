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

Scenario: When in the details page can see Download Container, when click on JSON button for Active Voice Channel Time button, then download will be verified by file path comparison
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	When I will see the download container
	Then I click on JSON button for Active Voice Channel time and I'll know it has been downloaded with right file type comparison

Scenario: When in the details page can see Download Container, when click on CSV button for Active Voice Channel Time button, then download will be verified by file path comparison
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	When I will see the download container
	Then I click on CSV button for Active Voice Channel time and I'll know it has been downloaded with right file type comparison

Scenario: When in the details page can see Download Container, when click on JSON button for Active Messaging Time button, then download will be verified by file path comparison
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	When I will see the download container
	Then I click on JSON button for Active Messaging time and I'll know it has been downloaded with right file type comparison

Scenario: When in the details page can see Download Container, when click on CSV button for Active Messaging Time button, then download will be verified by file path comparison
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	When I will see the download container
	Then I click on CSV button for Active Messaging time and I'll know it has been downloaded with right file type comparison

Scenario: When in the details page can see Download Container, when click on JSON button for Active Gaming Time button, then download will be verified by file path comparison
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	When I will see the download container
	Then I click on JSON button for Active Gaming time and I'll know it has been downloaded with right file type comparison

Scenario: When in the details page can see Download Container, when click on CSV button for Active Gaming Time button, then download will be verified by file path comparison
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	When I will see the download container
	Then I click on CSV button for Active Gaming time and I'll know it has been downloaded with right file type comparison

Scenario: When in the details page can see Download Container, when click on JSON button for Hours Per Game button, then download will be verified by file path comparison
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	When I will see the download container
	Then I click on JSON button for Hours Per Game and I'll know it has been downloaded with right file type comparison

Scenario: When in the details page can see Download Container, when click on CSV button for Hours Per Game button, then download will be verified by file path comparison
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	When I will see the download container
	Then I click on CSV button for Hours Per Game and I'll know it has been downloaded with right file type comparison

Scenario: When in the details page can see Download Container, when click on JSON button for Statuses button, then download will be verified by file path comparison
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	When I will see the download container
	Then I click on JSON button for Statuses and I'll know it has been downloaded with right file type comparison

Scenario: When in the details page can see Download Container, when click on CSV button for Statuses button, then download will be verified by file path comparison
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	When I will see the download container
	Then I click on CSV button for Statuses and I'll know it has been downloaded with right file type comparison


Scenario: When in the details page I'll click on View Games button, then click on RuneLite, and be on GamesDetails page where Most Popular Play Time DownLoadOptions are
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	When I click on View Games button
	When I click on RuneLite icon
	Then I will scroll down and see Download area

Scenario: When in the GamesDetails page at the Most Popular Play Time DownLoadOptions Json file option will be clicked and download will be verified by file path on system
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	When I click on View Games button
	When I click on RuneLite icon
	Then I will click json file type and I'll know it has been downloaded with right file type by filepath

Scenario: When in the GamesDetails page at the Most Popular Play Time DownLoadOptions Csv file option will be clicked and download will be verified by file path on system
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	When I click on View Games button
	When I click on RuneLite icon
	Then I will click csv file type and I'll know it has been downloaded with right file type by filepath

Scenario: When in the ServerGrowth page at the Active Members in Server DownLoadOptions Json file option will be clicked and download will be verified by file path on system
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	When I click on Server Growth button
	Then I will click Active Members in Server json file type and I'll know it has been downloaded with right file type by filepath

Scenario: When in the ServerGrowth page at the Active Members in Server DownLoadOptions Csv file option will be clicked and download will be verified by file path on system
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	When I click on Server Growth button
	Then I will click Active Members in Server csv file type and I'll know it has been downloaded with right file type by filepath


Scenario: When in the account page can see Download Container click the Active Voice Channel Time Button I'll remain on the same page
	Given I login
	When I go to my account page
	When I click on Advance Innovations Details icon
	When I will see the download container
	When I click on Active Voice Channel Time Button
	Then I will know file has been downloaded