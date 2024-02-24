namespace Linker.WebApi.GraphQL;

using AutoMapper;
using Linker.Core.ApiModels;
using Linker.Core.Models;
using Linker.Core.Repositories;

/// <summary>
/// The mutation schema for GraphQL.
/// </summary>
public sealed class Mutation
{
    private readonly IArticleRepository articleRepository;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="Mutation"/> class.
    /// </summary>
    /// <param name="articleRepository">The article repository.</param>
    /// <param name="mapper">The model mapper.</param>
    public Mutation(IArticleRepository articleRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(articleRepository);
        ArgumentNullException.ThrowIfNull(mapper);
        this.articleRepository = articleRepository;
        this.mapper = mapper;
    }

    /// <summary>
    /// Creates an article.
    /// </summary>
    /// <param name="request">The article creation request.</param>
    /// <returns>The operation status.</returns>
    public async Task<bool> CreateArticleAsync(CreateArticleRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var article = this.mapper.Map<Article>(request);

        try
        {
            await this.articleRepository
                .AddAsync(article, default)
                .ConfigureAwait(false);

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Updates an existing article.
    /// </summary>
    /// <param name="articleId">The article Id.</param>
    /// <param name="request">The update request.</param>
    /// <returns>True if success.</returns>
    /// <exception cref="GraphQLException">The GraphQL exception.</exception>
    public async Task<bool> UpdateArticleAsync(string articleId, UpdateArticleRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var article = this.mapper.Map<Article>(request);

        try
        {
            var existing = await this.articleRepository
                .GetByIdAsync(articleId, default)
                .ConfigureAwait(false);

            article.Id = existing.Id;

            await this.articleRepository
                .UpdateAsync(article, default)
                .ConfigureAwait(false);

            return true;
        }
        catch (Exception ex)
        {
            throw new GraphQLException(ex.Message);
        }
    }

    /// <summary>
    /// Deletes an existing article.
    /// </summary>
    /// <param name="articleId">The article ID.</param>
    /// <returns>True if success.</returns>
    /// <exception cref="GraphQLException">The GraphQL exception.</exception>
    public async Task<bool> DeleteArticleAsync(string articleId)
    {
        ArgumentException.ThrowIfNullOrEmpty(articleId);

        try
        {
            await this.articleRepository
                .RemoveAsync(articleId, default)
                .ConfigureAwait(false);

            return true;
        }
        catch (Exception ex)
        {
            throw new GraphQLException(ex.Message);
        }
    }
}
