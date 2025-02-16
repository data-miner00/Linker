namespace Linker.Cli.Integrations;

using Linker.Cli.Core;
using Linker.Common.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public sealed class VisitRepository : IRepository<Visit>
{
    private readonly AppDbContext context;

    public VisitRepository(AppDbContext context)
    {
        this.context = Guard.ThrowIfNull(context);
    }

    public async Task<int> AddAsync(Visit item)
    {
        var entry = await this.context.Visits.AddAsync(item);
        await this.context.SaveChangesAsync();
        return entry.Entity.Id;
    }

    public async Task<IEnumerable<Visit>> GetAllAsync()
    {
        var visits = await this.context.Visits.ToListAsync();
        return visits;
    }

    public Task<Visit> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Visit item)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Visit>> GetByLinkId(int linkId)
    {
        var visits = await this.context.Visits.Where(x => x.LinkId == linkId).ToListAsync();
        return visits;
    }
}
