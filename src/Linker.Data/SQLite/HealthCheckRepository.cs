namespace Linker.Data.SQLite;

using Dapper;
using Linker.Core.Models;
using Linker.Core.Repositories;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// The repository to access health check results.
/// </summary>
public sealed class HealthCheckRepository : IHealthCheckRepository
{
    private readonly IDbConnection connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="HealthCheckRepository"/> class.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    public HealthCheckRepository(IDbConnection connection)
    {
        this.connection = connection;
    }

    /// <inheritdoc/>
    public Task<HealthCheckResult> GetByUrlAsync(string url, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var query = "SELECT * FROM HealthCheckResults WHERE Url = @Url;";

        return this.connection.QueryFirstAsync<HealthCheckResult>(query, new { Url = url });
    }

    /// <inheritdoc/>
    public async Task UpsertAsync(HealthCheckResult result, CancellationToken cancellationToken)
    {
        try
        {
            var previousResult = await this.GetByUrlAsync(result.Url, cancellationToken).ConfigureAwait(false);

            await UpdateAsync(previousResult).ConfigureAwait(false);
        }
        catch (InvalidOperationException)
        {
            await InsertAsync().ConfigureAwait(false);
        }

        Task UpdateAsync(HealthCheckResult previousResult)
        {
            var operation = @"
                UPDATE HealthCheckResults
                SET
                    Status = @Status,
                    LastChecked = @LastChecked,
                    ErrorMessage = @ErrorMessage,
                    DeadAt = @DeadAt
                WHERE
                    Url = @Url;
            ";

            if (previousResult.Status == UrlStatus.Dead &&
                result.Status == UrlStatus.Dead)
            {
                result.DeadAt = previousResult.DeadAt;
            }

            return this.connection.ExecuteAsync(operation, new
            {
                result.Url,
                Status = result.Status.ToString(),
                result.LastChecked,
                result.ErrorMessage,
                result.DeadAt,
            });
        }

        Task InsertAsync()
        {
            var operation = @"
                INSERT INTO HealthCheckResults (
                    Url,
                    Status,
                    LastChecked,
                    ErrorMessage,
                    DeadAt
                ) VALUES (
                    @Url,
                    @Status,
                    @LastChecked,
                    @ErrorMessage,
                    @DeadAt
                );
            ";

            return this.connection.ExecuteAsync(operation, new
            {
                result.Url,
                Status = result.Status.ToString(),
                result.LastChecked,
                result.ErrorMessage,
                result.DeadAt,
            });
        }
    }
}
