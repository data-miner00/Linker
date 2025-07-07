namespace Linker.WebApi.HealthChecks;

using Linker.Common.Helpers;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Checks for database availability.
/// </summary>
public sealed class DatabaseHealthCheck : IHealthCheck
{
    private readonly IDbConnection dbConnection;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseHealthCheck"/> class.
    /// </summary>
    /// <param name="dbConnection">The database connection.</param>
    public DatabaseHealthCheck(IDbConnection dbConnection)
    {
        this.dbConnection = Guard.ThrowIfNull(dbConnection);
    }

    /// <inheritdoc/>
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        this.dbConnection.Open();

        HealthCheckResult result;

        if (this.dbConnection.State == ConnectionState.Open)
        {
            result = HealthCheckResult.Healthy();
        }
        else
        {
            result = HealthCheckResult.Unhealthy();
        }

        this.dbConnection.Close();

        return Task.FromResult(result);
    }
}
