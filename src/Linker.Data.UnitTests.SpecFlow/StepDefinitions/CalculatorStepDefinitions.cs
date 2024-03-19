namespace Linker.Data.UnitTests.SpecFlow.StepDefinitions;

[Binding]
public sealed class CalculatorStepDefinitions
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef
    private int firstNumber;
    private int secondNumber;
    private int answer;

    [Given("the first number is (.*)")]
    public void GivenTheFirstNumberIs(int number)
    {
        // For storing and retrieving scenario-specific data see https://go.specflow.org/doc-sharingdata
        // To use the multiline text or the table argument of the scenario,
        // additional string/Table parameters can be defined on the step definition
        // method.
        this.firstNumber = number;
    }

    [Given("the second number is (.*)")]
    public void GivenTheSecondNumberIs(int number)
    {
        this.secondNumber = number;
    }

    [When("the two numbers are added")]
    public void WhenTheTwoNumbersAreAdded()
    {
        this.answer = this.firstNumber + this.secondNumber;
    }

    [Then("the result should be (.*)")]
    public void ThenTheResultShouldBe(int result)
    {
        this.answer.Should().Be(result);
    }
}
