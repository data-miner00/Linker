namespace Linker.WebApi.Filters;

using Microsoft.AspNetCore.Mvc.Filters;

/// <summary>
/// A global filter that logs when and after an action has been executed.
/// </summary>
public sealed partial class GlobalInformerFilter : IActionFilter
{
    private readonly ILogger<GlobalInformerFilter> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalInformerFilter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public GlobalInformerFilter(ILogger<GlobalInformerFilter> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        this.logger = logger;
    }

    /// <inheritdoc/>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        this.LogExecuting(context.ActionDescriptor.DisplayName ?? "Unknown Action");
    }

    /// <inheritdoc/>
    public void OnActionExecuted(ActionExecutedContext context)
    {
        this.LogExecuted(context.ActionDescriptor.DisplayName ?? "Unknown Action");
    }

    /// <summary>
    /// Implementation following <see href="https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1873">CA1873: Avoid potentially expensive logging</see>.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    [LoggerMessage(Level = LogLevel.Information, Message = "Global executing action: {ActionName}")]
    private partial void LogExecuting(string actionName);

    [LoggerMessage(Level = LogLevel.Trace, Message = "Global executed action: {ActionName}")]
    private partial void LogExecuted(string actionName);
}
