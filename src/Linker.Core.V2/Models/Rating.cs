namespace Linker.Core.V2.Models;

/// <summary>
/// The <see href="https://www.motionpictures.org/film-ratings/">MPA Rating</see>.
/// See more at <see href="https://www.marshallcinema.com/mpaa">this link</see>.
/// </summary>
public enum Rating
{
    /// <summary>
    /// It is safe for audience of all ages.
    /// </summary>
    G,

    /// <summary>
    /// Some parental guidance is encouraged.
    /// </summary>
    PG,

    /// <summary>
    /// Material might not suite pre-teenager.
    /// </summary>
    PG13,

    /// <summary>
    /// Material may include adult-themed activities.
    /// </summary>
    R,

    /// <summary>
    /// Strictly no underaged audience.
    /// </summary>
    NC17,
}
