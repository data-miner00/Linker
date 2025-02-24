namespace Linker.Cli.Integrations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Linker.Cli.Core;
using Linker.Common.Helpers;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// The repository that stores <see cref="Visit"/>.
/// </summary>
public sealed class VisitRepository : IRepository<Visit>
{
    private readonly AppDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="VisitRepository"/> class.
    /// </summary>
    /// <param name="context">The db context.</param>
    public VisitRepository(AppDbContext context)
    {
        this.context = Guard.ThrowIfNull(context);
    }

    /// <inheritdoc/>
    public async Task<int> AddAsync(Visit item)
    {
        var entry = await this.context.Visits.AddAsync(item);
        await this.context.SaveChangesAsync();
        return entry.Entity.Id;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Visit>> GetAllAsync()
    {
        var visits = await this.context.Visits.ToListAsync();
        return visits;
    }

    /// <inheritdoc/>
    public Task<Visit?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task RemoveAsync(int id)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task UpdateAsync(Visit item)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets all visits of a <see cref="Link"/>.
    /// </summary>
    /// <param name="linkId">The link Id.</param>
    /// <returns>The visits of the link.</returns>
    public async Task<IEnumerable<Visit>> GetByLinkId(int linkId)
    {
        var visits = await this.context.Visits.Where(x => x.LinkId == linkId).ToListAsync();
        return visits;
    }

    /// <inheritdoc/>
    public Task<IEnumerable<Visit>> SearchAsync(string keyword)
    {
        throw new NotImplementedException();
    }
}
