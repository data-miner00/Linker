namespace Linker.Common.Extensions;

using System;
using System.Threading.Tasks;

/// <summary>
/// Extension class for <see cref="Task"/> related code.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Safely execute the <see cref="Task"/> without waiting for it to complete before moving to the next instruction.
    /// </summary>
    /// <param name="task">The task itself.</param>
    /// <param name="continueOnCapturedContext">Whether continue on the calling thread.</param>
    /// <param name="onException">Action to execute during exception.</param>
#pragma warning disable S3168 // "async" methods should not return "void"
    public static async void SafeFireAndForget(
        this Task task,
        bool continueOnCapturedContext = true,
        Action<Exception> onException = null)
    {
        try
        {
            await task.ConfigureAwait(continueOnCapturedContext);
        }
        catch (Exception ex) when (onException is not null)
        {
            onException(ex);
        }
    }
#pragma warning restore S3168 // "async" methods should not return "void"
}
