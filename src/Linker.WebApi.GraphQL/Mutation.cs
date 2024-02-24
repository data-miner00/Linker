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
}
