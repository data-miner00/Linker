Feature: CreateListCommandHandler

The behaviours for the create list command handler.

@constructor
Scenario: Invalid constructor parameters: null repository
    Given the repository is null
    And the console is not null
    When I instantiate the CreateListCommandHandler
    Then I should expect argument null exception to be thrown

@constructor
Scenario: Invalid constructor parameters: null console
    Given the repository is not null
    And the console is null
    When I instantiate the CreateListCommandHandler
    Then I should expect argument null exception to be thrown

@handle @arguments
Scenario: Handling a null command argument, it should throw an exception
    Given the command argument is null
    When I handle the command arguments
    Then I should expect argument null exception to be thrown

@handle @arguments
Scenario: Handling an invalid type argument, it should throw an exception
    Given the command argument is incorrect type
    When I handle the command arguments
    Then I should expect argument exception to be thrown

@handle
Scenario: Getting help for command
    Given the command argument is help
    When I handle the command arguments
    Then I should expect console write being called 1 times
    Then I should expect no exception is thrown

@handle
Scenario: Normal flow, call add async
    Given the command argument has name "hello" and description "world"
    When I handle the command arguments
    Then I should expect console write being called 0 times
    And I should expect repository add async to be called with argument
    | Name   | Description |
    | hello  | world       |
    And I should expect no exception is thrown
