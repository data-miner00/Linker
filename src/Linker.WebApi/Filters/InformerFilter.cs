namespace Linker.WebApi.Filters;

using Microsoft.AspNetCore.Mvc.Filters;

public sealed class InformerFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine("Before action executed");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        Console.WriteLine("After action executed");
    }
}
