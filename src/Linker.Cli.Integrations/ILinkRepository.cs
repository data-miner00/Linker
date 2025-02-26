namespace Linker.Cli.Integrations;

using Linker.Cli.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// The repository contract for <see cref="Link"/>.
/// </summary>
public interface ILinkRepository : IRepository<Link>
{
    /// <summary>
    /// Get all links and optionally filter out watch later.
    /// </summary>
    /// <param name="watchLater">A flag to take only watch later links.</param>
    /// <returns>The list of links.</returns>
    Task<IEnumerable<Link>> GetAllAsync(bool watchLater);
}
