Feature: AddLinkCommandHandler

The behaviour for the AddLinkCommandHandler.

@constructor
Scenario: When the repository is null, it should throw an exception
	Given the repository is null
	When I instantiate the AddLinkCommandHandler
	Then I should expect an exception to be thrown

@handle @arguments
Scenario: Handling a null command argument, it should throw an exception
    Given the command argument is null
	When I handle the command arguments
	Then I should expect an exception to be thrown

@handle @arguments
Scenario: Handling an invalid type argument, it should throw an exception
    Given the command argument is incorrect type
	When I handle the command arguments
	Then I should expect ArgumentException to be thrown

@handle @arguments
Scenario: Given request for help, should do nothing
    Given the command argument is help
	When I handle the command arguments
	Then I should expect AddAsync to not be called
    And I should expect no exception is thrown

@handle @arguments
Scenario: Given a valid command argument, it should call the repository
    Given the command with url "https://google.com"
	When I handle the command arguments
	Then I should expect AddAsync to be called with command with url "https://google.com"
    And I should expect no exception is thrown

@handle @repository
Scenario: Does not allow duplicate URLs
    Given the url already exists
    And the command with url "https://google.com"
	When I handle the command arguments
	Then I should expect AddAsync to be called with command with url "https://google.com"
	And I should expect environment exit code to be 1
    And I should expect no exception is thrown

@handle @repository
Scenario: Uncaught exception occurred
    Given the command with url "https://google.com"
	And SqliteException occurs
	When I handle the command arguments
	Then I should expect AddAsync to be called with command with url "https://google.com"
	And I should expect SqliteException to be thrown

@handle @repository
Scenario: Valid command argument link added
    Given I have the command argument
	| Url | Name | Description | WatchLater | Tags | Language | ShowHelp |
	| https://google.com | Google | | true       | google,search | C#       | false    |
	When I handle the command arguments
	Then I should expect AddAsync to be called with command with url "https://google.com"
