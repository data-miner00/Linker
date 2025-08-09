namespace Linker.WebApi.Options
{
    using System.ComponentModel;

    /// <summary>
    /// The options required for SQLite.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal sealed class SQLiteOption
    {
        /// <summary>
        /// Gets or sets the connection string for SQLite database.
        /// </summary>
        required public string ConnectionString { get; set; }
    }
}
