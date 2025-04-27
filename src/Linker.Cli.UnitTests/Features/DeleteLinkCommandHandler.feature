Feature: DeleteLinkCommandHandler

The behaviours for delete link command handler.

@constructor
Scenario: Constructor invalid parameters
	Given the link repository is null
	When I instantiate the DeleteLinkCommandHandler
	Then I should expect ArgumentNullException to be thrown

@handler
Scenario: Handle null arguments
	Given the command argument is null
	When I handle the command arguments
	Then I should expect ArgumentNullException to be thrown

@handle @arguments
Scenario: Handling an invalid type argument, it should throw an exception
    Given the command argument is incorrect type
	When I handle the command arguments
	Then I should expect ArgumentException to be thrown

@handle @arguments
Scenario: Request for help
    Given the command argument is help
	When I handle the command arguments
	Then I should expect GetByIdAsync to not be called
	And I should expect RemoveAsync to not be called
    And I should expect no exception is thrown

@handle @arguments
Scenario: Link does not exist for deletion
    Given the link with id 1 does not exist
	When I handle the command arguments
	Then I should expect GetByIdAsync to be called with id 1
	And I should expect RemoveAsync to not be called
    And I should expect no exception is thrown

@handle @arguments
Scenario: Link exist for deletion and confirm delete
    Given the link with id 1 exists
	When I handle the command arguments
	Then I should expect GetByIdAsync to be called with id 1
	And I should expect RemoveAsync to be called with id 1
    And I should expect no exception is thrown

