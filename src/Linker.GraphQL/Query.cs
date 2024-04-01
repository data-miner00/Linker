namespace Linker.GraphQL;

using Linker.Core.Models;
using Linker.Core.Repositories;

/// <summary>
/// The query schema for GraphQL.
/// </summary>
public sealed class Query
{
    private readonly IArticleRepository articleRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="Query"/> class.
    /// </summary>
    /// <param name="articleRepository">The article repository.</param>
    public Query(IArticleRepository articleRepository)
    {
        this.articleRepository = articleRepository;
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
    #region Article

    /// <summary>
    /// Gets all the existing articles.
    /// </summary>
    /// <returns>The list of articles.</returns>
    public Task<IEnumerable<Article>> GetArticlesAsync()
    {
        return this.articleRepository.GetAllAsync(default);
    }

    /// <summary>
    /// Retrieves an article by ID.
    /// </summary>
    /// <param name="id">The ID of the article.</param>
    /// <returns>The article if found else <c>null</c>.</returns>
    public async Task<Article?> GetByIdAsync(string id)
    {
        try
        {
            return await this.articleRepository.GetByIdAsync(id, default).ConfigureAwait(false);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    /// <summary>
    /// Retrieves the articles created by a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The list of user created articles.</returns>
    public async Task<IEnumerable<Article>?> GetArticlesByUser(string userId)
    {
        try
        {
            return await this.articleRepository.GetAllByUserAsync(userId, default).ConfigureAwait(false);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
    #endregion
}
