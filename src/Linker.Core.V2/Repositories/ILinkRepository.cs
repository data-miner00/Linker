namespace Linker.Core.V2.Repositories;

using Linker.Core.V2.Models;
using Linker.Core.V2.QueryParams;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// The abstraction for the <see cref="Link"/> repository.
/// </summary>
public interface ILinkRepository
{
    /// <summary>
    /// Add a new link into the database.
    /// </summary>
    /// <param name="link">The link to be added.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The id of the new link.</returns>
    Task<string> AddAsync(Link link, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all the available data from the database.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The entire list of links.</returns>
    Task<IEnumerable<Link>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all the available data from the database.
    /// </summary>
    /// <param name="query">The query parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The entire list of filtered links.</returns>
    Task<IEnumerable<Link>> GetAllAsync(GetLinksQueryParams query, CancellationToken cancellationToken);

    /// <summary>
    /// Searches the links that has the keyword provided.
    /// </summary>
    /// <param name="keyword">The keyword to be searched.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of search results.</returns>
    Task<IEnumerable<Link>> SearchAsync(string keyword, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the link by ID.
    /// </summary>
    /// <param name="id">The id of the link.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The link itself.</returns>
    Task<Link> GetByIdAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes an link from the database with the given id.
    /// </summary>
    /// <param name="id">The id of the link to be removed.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The awaitable task.</returns>
    Task RemoveAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an link with the new details provided.
    /// </summary>
    /// <param name="link">The link with updated details.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The awaitable task.</returns>
    Task UpdateAsync(Link link, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all items that is owned by the user.
    /// </summary>
    /// <param name="userId">Ths user ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of items that the user owns.</returns>
    Task<IEnumerable<Link>> GetAllByUserAsync(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a particular link that is owned by the user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="linkId">The link ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The found link.</returns>
    Task<Link> GetByUserAsync(string userId, string linkId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a list of links of a category.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of links.</returns>
    Task<IEnumerable<Link>> GetAllByCategoryAsync(Category category, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a list of links by link type.
    /// </summary>
    /// <param name="linkType">The type of link.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of links filtered by link type.</returns>
    Task<IEnumerable<Link>> GetAllByLinkTypeAsync(LinkType linkType, CancellationToken cancellationToken);
}
