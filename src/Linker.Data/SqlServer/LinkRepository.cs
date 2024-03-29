namespace Linker.Data.SqlServer;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;
using Dapper;
using System.Data;
using EnsureThat;

/// <summary>
/// The repository for working with <see cref="Link"/>.
/// </summary>
public sealed class LinkRepository : ILinkRepository
{
    private readonly IDbConnection connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="LinkRepository"/> class.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    public LinkRepository(IDbConnection connection)
    {
        this.connection = EnsureArg.IsNotNull(connection, nameof(connection));
    }

    /// <inheritdoc/>
    public Task AddAsync(Link link, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<IEnumerable<Link>> GetAllAsync(CancellationToken cancellationToken)
    {
        var query = @"SELECT * FROM Links;";

        return this.connection.QueryAsync<Link>(query, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<IEnumerable<Link>> GetAllByUserAsync(string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<Link> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<Link> GetByUserAsync(string userId, string linkId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task RemoveAsync(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task UpdateAsync(Link item, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
