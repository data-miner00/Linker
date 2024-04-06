namespace Linker.Core.V2.Models;

/// <summary>
/// The nature/provider of the link.
/// </summary>
public enum LinkType : uint
{
    /// <summary>
    /// The default value that represents nothing.
    /// </summary>
    None,

    /// <summary>
    /// Represents an article, news or blog posts.
    /// </summary>
    Article,

    /// <summary>
    /// Represents a general webpage.
    /// For example, company's website, product homepage or any top-level domain.
    /// </summary>
    Website,

    /// <summary>
    /// Represents a Youtube channel. It can also be an individual video on Youtube.
    /// </summary>
    Youtube,

    /// <summary>
    /// Represents a link to repo, issues, pull requests etc on GitHub.
    /// </summary>
    GitHub,

    /// <summary>
    /// Represents a link to social media such as Facebook page, Instagram post or X profile.
    /// </summary>
    Socials,
}
