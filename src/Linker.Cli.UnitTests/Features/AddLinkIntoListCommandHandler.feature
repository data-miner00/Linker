Feature: AddLinkIntoListCommandHandler

The behaviour for the AddLinkIntoListCommandHandler.

@constructor
Scenario: When the link repository is null, it should throw an exception
	Given the link repository is null
	And the list repository is not null
	And the app context is not null
	When I instantiate the AddLinkIntoListCommandHandler
	Then I should expect an exception to be thrown

@constructor
Scenario: When the list repository is null, it should throw an exception
	Given the link repository is not null
	And the list repository is null
	And the app context is not null
	When I instantiate the AddLinkIntoListCommandHandler
	Then I should expect an exception to be thrown

@constructor
Scenario: When the app context is null, it should throw an exception
	Given the link repository is not null
	And the list repository is not null
	And the app context is null
	When I instantiate the AddLinkIntoListCommandHandler
	Then I should expect an exception to be thrown

@handle @arguments
Scenario: Handling a null command argument, it should throw an exception
    Given the command argument is null
	When I handle the command
	Then I should expect an exception to be thrown

@handle @arguments
Scenario: Handling an invalid type argument, it should throw an exception
    Given the command argument is incorrect type
	When I handle the command
	Then I should expect ArgumentException to be thrown

@handle @normal
Scenario: Handling valid scenario, should add the link into list
    Given the command argument is valid
	And the link exists
	And the list exists
	When I handle the command
	Then I should expect the link added to the list
	And I should expect no exception is thrown

@handle @notfound
Scenario: Link is found but List is not found
    Given the command argument is valid
	And the link exists
	And the list does not exist
	When I handle the command
	Then I should expect InvalidOperationException to be thrown

@handle @notfound
Scenario: Link is not found but List is found
    Given the command argument is valid
	And the link does not exist
	And the list exists
	When I handle the command
	Then I should expect InvalidOperationException to be thrown

