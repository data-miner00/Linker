namespace Linker.WebApi.Filters;

using Linker.Common.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;

/// <summary>
/// The scoped informer filter as an attribute. The order actually matters.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class ScopedInformerFilterAttribute : Attribute, IActionFilter
{
    private readonly string name;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScopedInformerFilterAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of the caller.</param>
    public ScopedInformerFilterAttribute(string name)
    {
        this.name = Guard.ThrowIfNullOrWhitespace(name);
    }

    /// <inheritdoc/>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine($"Before {this.name} executed");
    }

    /// <inheritdoc/>
    public void OnActionExecuted(ActionExecutedContext context)
    {
        Console.WriteLine($"After {this.name} executed");
    }
}
