namespace Linker.GraphQL.Options;

/// <summary>
/// The options required for SQLite.
/// </summary>
public sealed record SQLiteOption
{
    /// <summary>
    /// Gets the connection string for SQLite database.
    /// </summary>
    required public string ConnectionString { get; init; }
}
