namespace Linker.WebJob.Options;

/// <summary>
/// The options required for database.
/// </summary>
internal sealed class DatabaseOption
{
    /// <summary>
    /// Gets or sets the connection string for database.
    /// </summary>
    required public string ConnectionString { get; set; }
}
