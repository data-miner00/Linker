namespace Linker.Mvc.Options;

/// <summary>
/// The settings for cookie.
/// </summary>
public sealed class CookieOption
{
    /// <summary>
    /// Gets or sets the login path for the application.
    /// </summary>
    public string LoginPath { get; set; } = null!;

    /// <summary>
    /// Gets or sets the timeout for a login session.
    /// </summary>
    public int TimeoutInMinutes { get; set; }
}
