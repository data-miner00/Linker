namespace Linker.Cli.Integrations;

using System.Collections.Generic;
using System.Threading.Tasks;

using Linker.Cli.Core;
using Linker.Common.Helpers;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// The url list repository.
/// </summary>
public sealed class UrlListRepository : IRepository<UrlList>
{
    private readonly AppDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UrlListRepository"/> class.
    /// </summary>
    /// <param name="context">The db context.</param>
    public UrlListRepository(AppDbContext context)
    {
        this.context = Guard.ThrowIfNull(context);
    }

    /// <inheritdoc/>
    public async Task<int> AddAsync(UrlList item)
    {
        var entry = await this.context.Lists.AddAsync(item);
        await this.context.SaveChangesAsync();
        return entry.Entity.Id;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<UrlList>> GetAllAsync()
    {
        var lists = await this.context.Lists.ToListAsync();
        return lists;
    }

    /// <inheritdoc/>
    public Task<UrlList?> GetByIdAsync(int id)
    {
        return this.context.Lists.Include(x => x.Links).FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(int id)
    {
        var listToBeDeleted = await this.context.Lists.FirstAsync(x => x.Id == id);
        this.context.Lists.Remove(listToBeDeleted);
        await this.context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<UrlList>> SearchAsync(string keyword)
    {
        var keywordLower = keyword.ToLowerInvariant();

        var lists = await this.context.Lists
            .Where(x => x.Name.Contains(keywordLower)
                || string.IsNullOrEmpty(x.Description)
                || x.Description.Contains(keywordLower))
            .ToListAsync();

        return lists;
    }

    /// <inheritdoc/>
    public Task UpdateAsync(UrlList item)
    {
        this.context.Lists.Update(item);
        return this.context.SaveChangesAsync();
    }
}
