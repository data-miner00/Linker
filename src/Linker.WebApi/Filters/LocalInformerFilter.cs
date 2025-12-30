namespace Linker.WebApi.Filters;

using Microsoft.AspNetCore.Mvc.Filters;

/// <summary>
/// A local filter that logs when and after an action has been executed.
/// </summary>
public sealed class LocalInformerFilter : IActionFilter
{
    private readonly ILogger<LocalInformerFilter> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalInformerFilter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public LocalInformerFilter(ILogger<LocalInformerFilter> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        this.logger = logger;
    }

    /// <inheritdoc/>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (this.logger.IsEnabled(LogLevel.Information))
        {
            this.logger.LogInformation("Local executing action: {ActionName}", context.ActionDescriptor.DisplayName ?? "Unknown Action");
        }
    }

    /// <inheritdoc/>
    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (this.logger.IsEnabled(LogLevel.Information))
        {
            this.logger.LogInformation("Local executed action: {ActionName}", context.ActionDescriptor.DisplayName ?? "Unknown Action");
        }
    }
}
