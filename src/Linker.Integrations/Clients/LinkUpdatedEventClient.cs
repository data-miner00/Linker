﻿namespace Linker.Integrations.Clients;

using Dapper;
using Linker.Core.V2.Clients;
using Linker.Core.V2.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

public sealed class LinkUpdatedEventClient : ILinkUpdatedEventClient
{
    private readonly IDbConnection connection;

    public LinkUpdatedEventClient(IDbConnection connection)
    {
        this.connection = connection;
    }

    public Task CompleteAsync(long id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<LinkUpdatedEvent> PeekAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<LinkUpdatedEvent>> PeekBatchAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task PublishAsync(string linkId, CancellationToken cancellationToken)
    {
        var stmt = "INSERT INTO LinkUpdatedEvents ([LinkId]) VALUES (@LinkId);";

        return this.connection.ExecuteAsync(stmt, new { LinkId = linkId });
    }
}
