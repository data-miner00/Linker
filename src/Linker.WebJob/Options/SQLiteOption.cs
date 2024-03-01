namespace Linker.WebJob.Options;

/// <summary>
/// The options required for SQLite.
/// </summary>
internal sealed class SQLiteOption
{
    /// <summary>
    /// Gets or sets the connection string for SQLite database.
    /// </summary>
    required public string ConnectionString { get; set; }
}
