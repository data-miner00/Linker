namespace Linker.Core.V2.Repositories;

using System.Collections.Generic;
using System.Threading.Tasks;
using Linker.Core.V2.Models;

/// <summary>
/// The abstraction for the <see cref="Tag"/> repository.
/// </summary>
public interface ITagRepository
{
    /// <summary>
    /// Get all existing tags.
    /// </summary>
    /// <returns>The list of tags.</returns>
    public Task<IEnumerable<Tag>> GetAllAsync();

    /// <summary>
    /// Get by either name or Id.
    /// </summary>
    /// <param name="type">The type of the query.</param>
    /// <param name="value">The value for query.</param>
    /// <returns>The found tag.</returns>
    public Task<Tag> GetByAsync(string type, string value);

    /// <summary>
    /// Adds a new tag.
    /// </summary>
    /// <param name="name">The name of the tag.</param>
    /// <returns>Nothing.</returns>
    public Task AddAsync(string name);

    /// <summary>
    /// Add link tag pairs. Used when creating a new <see cref="Link"/>.
    /// </summary>
    /// <param name="linkId">The link Id.</param>
    /// <param name="tagId">The tag Id.</param>
    /// <returns>Nothing.</returns>
    public Task AddLinkTagAsync(string linkId, string tagId);

    /// <summary>
    /// Edit the name of a tag.
    /// </summary>
    /// <param name="id">The Id of the tag.</param>
    /// <param name="newName">The new name of the tag.</param>
    /// <returns>Nothing.</returns>
    public Task EditNameAsync(string id, string newName);

    /// <summary>
    /// Delete the tag from Tags and Link_Tags table.
    /// </summary>
    /// <param name="id">The Id of the tag to be deleted.</param>
    /// <returns>Nothing.</returns>
    public Task DeleteAsync(string id);

    /// <summary>
    /// Delete link tag. Used when deleting a <see cref="Link"/>.
    /// </summary>
    /// <param name="linkId">The link Id.</param>
    /// <param name="tagId">The tag Id.</param>
    /// <returns>Nothing.</returns>
    public Task DeleteLinkTagAsync(string linkId, string tagId);
}
