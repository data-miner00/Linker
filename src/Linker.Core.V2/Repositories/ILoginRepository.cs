namespace Linker.Core.V2.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// The repository to track login dates. Login is a resource, not an action.
/// </summary>
public interface ILoginRepository
{
    /// <summary>
    /// Tracks the login for the user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The task.</returns>
    Task TrackLoginAsync(string userId);

    /// <summary>
    /// Retrieves the last login date tracked.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The login date.</returns>
    Task<DateTime> GetLastLoginDateAsync(string userId);

    /// <summary>
    /// Retrieves a list of recent logins performed by the user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="limit">The number of records to retrieve.</param>
    /// <returns>The list of login dates.</returns>
    Task<IEnumerable<DateTime>> GetRecentLoginDatesAsync(string userId, int limit);
}
