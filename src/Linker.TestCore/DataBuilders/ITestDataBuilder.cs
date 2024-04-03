namespace Linker.TestCore.DataBuilders;

internal interface ITestDataBuilder<out T>
    where T : class, new()
{
    T Build();
}
