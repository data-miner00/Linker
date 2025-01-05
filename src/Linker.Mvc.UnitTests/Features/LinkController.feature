Feature: LinkController

The link controller have a lot of behaviour that needs to be tested out.

Background:
	Given controller context is properly setup

@nulls
Scenario: Unable to initialize controller with "Null" parameters
	When I initialize with "Null" for repo "<IsRepoNull>" and "Null" for mapper "<IsMapperNull>" and "Null" for logger "<IsLoggerNull>" and "Null" for client "<IsClientNull>"
	Then I expect "ArgumentNullException" should be thrown.
Examples: 
| IsRepoNull | IsMapperNull | IsLoggerNull | IsClientNull |
| true       | false        | false        | false        |
| false      | true         | false        | false        |
| false      | false        | true         | false        |
| false      | false        | false        | true         |


@get
Scenario: Return links when "Index" is called
	Given the repository is able to fetch all links that contains name "My link"
	When I invoke "Index" method
	Then I expect the response to contain a list of links that contains "My link"
	And the repository's "GetAllAsync" method should be called 1 times
	And no exception was thrown
