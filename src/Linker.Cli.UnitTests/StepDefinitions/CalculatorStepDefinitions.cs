namespace Linker.Cli.UnitTests.StepDefinitions;
[Binding]
public sealed class CalculatorStepDefinitions
{
    private int firstNumber;
    private int secondNumber;
    private int result;

    [Given("the first number is (.*)")]
    public void GivenTheFirstNumberIs(int number)
    {
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
        this.result = this.firstNumber + this.secondNumber;
    }

    [Then("the result should be (.*)")]
    public void ThenTheResultShouldBe(int result)
    {
        Assert.Equal(this.result, result);
    }
}
