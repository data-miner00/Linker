namespace Linker.Cli.Integrations;

using Linker.Cli.Core;
using Linker.Common.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// The repository layer for <see cref="Link"/>.
/// </summary>
public sealed class LinkRepository : IRepository<Link>
{
    private readonly AppDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="LinkRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public LinkRepository(AppDbContext context)
    {
        this.context = Guard.ThrowIfNull(context);
    }

    /// <inheritdoc/>
    public async Task<int> AddAsync(Link item)
    {
        var entry = await this.context.Links.AddAsync(item);
        await this.context.SaveChangesAsync();
        return entry.Entity.Id;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Link>> GetAllAsync()
    {
        var links = await this.context.Links.ToListAsync();
        return links;
    }

    /// <inheritdoc/>
    public Task<Link> GetByIdAsync(int id)
    {
        return this.context.Links.Where(x => x.Id == id).FirstAsync();
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(int id)
    {
        var itemToBeRemoved = await this.context.Links.FindAsync(id)
            ?? throw new InvalidOperationException("Item not found");

        this.context.Links.Remove(itemToBeRemoved);

        await this.context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task UpdateAsync(Link item)
    {
        this.context.Links.Update(item);
        return this.context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Link>> SearchAsync(string keyword)
    {
        var matched = await this.context.Links
            .Where(
                x => x.Url.Contains(keyword)
                || (x.Name != null && x.Name.Contains(keyword))
                || (x.Description != null && x.Description.Contains(keyword))
                || (x.Tags != null && x.Tags.Contains(keyword)))
            .ToListAsync();

        return matched;
    }
}
