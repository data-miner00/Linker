namespace Linker.Cli.Integrations;

using Linker.Cli.Core;
using Linker.Common.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public sealed class UrlListRepository : IRepository<UrlList>
{
    private readonly AppDbContext context;

    public UrlListRepository(AppDbContext context)
    {
        this.context = Guard.ThrowIfNull(context);
    }

    public async Task<int> AddAsync(UrlList item)
    {
        var entry = await this.context.Lists.AddAsync(item);
        await this.context.SaveChangesAsync();
        return entry.Entity.Id;
    }

    public async Task<IEnumerable<UrlList>> GetAllAsync()
    {
        var lists = await this.context.Lists.ToListAsync();
        return lists;
    }

    public Task<UrlList> GetByIdAsync(int id)
    {
        return this.context.Lists.Include(x => x.Links).FirstAsync(x => x.Id == id);
    }

    public async Task RemoveAsync(int id)
    {
        var listToBeDeleted = await this.context.Lists.FirstAsync(x => x.Id == id);
        this.context.Lists.Remove(listToBeDeleted);
        await this.context.SaveChangesAsync();
    }

    public Task UpdateAsync(UrlList item)
    {
        this.context.Lists.Update(item);
        return this.context.SaveChangesAsync();
    }
}
