namespace Linker.GraphQL;

using AutoMapper;
using Linker.Core.V2.ApiModels;
using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;

/// <summary>
/// The mutation schema for GraphQL.
/// </summary>
public sealed class Mutation
{
    private readonly ILinkRepository linkRepository;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="Mutation"/> class.
    /// </summary>
    /// <param name="linkRepository">The link repository.</param>
    /// <param name="mapper">The model mapper.</param>
    public Mutation(ILinkRepository linkRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(linkRepository);
        ArgumentNullException.ThrowIfNull(mapper);
        this.linkRepository = linkRepository;
        this.mapper = mapper;
    }

    /// <summary>
    /// Creates an link.
    /// </summary>
    /// <param name="request">The link creation request.</param>
    /// <returns>The operation status.</returns>
    public async Task<bool> CreateLinkAsync(CreateLinkRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var link = this.mapper.Map<Link>(request);

        try
        {
            await this.linkRepository
                .AddAsync(link, default)
                .ConfigureAwait(false);

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Updates an existing link.
    /// </summary>
    /// <param name="linkId">The link Id.</param>
    /// <param name="request">The update request.</param>
    /// <returns>True if success.</returns>
    /// <exception cref="GraphQLException">The GraphQL exception.</exception>
    public async Task<bool> UpdateLinkAsync(string linkId, UpdateLinkRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var link = this.mapper.Map<Link>(request);

        try
        {
            var existing = await this.linkRepository
                .GetByIdAsync(linkId, default)
                .ConfigureAwait(false);

            link.Id = existing.Id;

            await this.linkRepository
                .UpdateAsync(link, default)
                .ConfigureAwait(false);

            return true;
        }
        catch (Exception ex)
        {
            throw new GraphQLException(ex.Message);
        }
    }

    /// <summary>
    /// Deletes an existing link.
    /// </summary>
    /// <param name="linkId">The link ID.</param>
    /// <returns>True if success.</returns>
    /// <exception cref="GraphQLException">The GraphQL exception.</exception>
    public async Task<bool> DeleteLinkAsync(string linkId)
    {
        ArgumentException.ThrowIfNullOrEmpty(linkId);

        try
        {
            await this.linkRepository
                .RemoveAsync(linkId, default)
                .ConfigureAwait(false);

            return true;
        }
        catch (Exception ex)
        {
            throw new GraphQLException(ex.Message);
        }
    }
}
