namespace Linker.GraphQL;

using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;

/// <summary>
/// The query schema for GraphQL.
/// </summary>
public sealed class Query
{
    private readonly ILinkRepository linkRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="Query"/> class.
    /// </summary>
    /// <param name="linkRepository">The link repository.</param>
    public Query(ILinkRepository linkRepository)
    {
        ArgumentNullException.ThrowIfNull(linkRepository);
        this.linkRepository = linkRepository;
    }

    #region Test

    /// <summary>
    /// The dummy type for initial setup.
    /// </summary>
    /// <param name="Name">The name of the book.</param>
    /// <param name="Author">The author of the book.</param>
    public sealed record Book([GraphQLNonNullType]string Name, string Author);

    /// <summary>
    /// Retrieves the one and only book.
    /// </summary>
    /// <returns>The book.</returns>
    [GraphQLDeprecated("This is for testing")]
    public Book GetBook() => new("Learn you Haskell", "Milan");

    #endregion
    #region Link

    /// <summary>
    /// Gets all the existing links.
    /// </summary>
    /// <returns>The list of links.</returns>
    public Task<IEnumerable<Link>> GetLinksAsync()
    {
        return this.linkRepository.GetAllAsync(default);
    }

    /// <summary>
    /// Retrieves an article by ID.
    /// </summary>
    /// <param name="id">The ID of the article.</param>
    /// <returns>The article if found else <c>null</c>.</returns>
    public async Task<Link?> GetByIdAsync(string id)
    {
        try
        {
            return await this.linkRepository.GetByIdAsync(id, default).ConfigureAwait(false);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    /// <summary>
    /// Retrieves the links created by a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The list of user created links.</returns>
    public async Task<IEnumerable<Link>?> GetLinksByUser(string userId)
    {
        try
        {
            return await this.linkRepository.GetAllByUserAsync(userId, default).ConfigureAwait(false);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
    #endregion
}
