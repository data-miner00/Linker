Feature: Calculator
![Calculator](https://specflow.org/wp-content/uploads/2020/09/calculator.png)
Simple calculator for adding **two** numbers

Link to a feature: [Calculator](Linker.Cli.UnitTests/Features/Calculator.feature)
***Further read***: **[Learn more about how to generate Living Documentation](https://docs.specflow.org/projects/specflow-livingdoc/en/latest/LivingDocGenerator/Generating-Documentation.html)**

@mytag
Scenario: Add two numbers
	Given the first number is <first>
	And the second number is <second>
	When the two numbers are added
	Then the result should be <total>

Examples:
	| first | second | total |
	| 2     | 3      | 5     |
	| 4     | 5      | 9     |
	| 23    | 49     | 72    |
