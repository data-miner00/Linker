namespace Linker.Core.V2.Models;

public class User
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the profile image URL of the user.
    /// </summary>
    public string PhotoUrl { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the role associated with the user.
    /// </summary>
    public Role Role { get; set; }

    /// <summary>
    /// Gets or sets the status of the user.
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// Gets or sets the age of the user.
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the date that the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date by which the user was last updated.
    /// </summary>
    public DateTime ModifiedAt { get; set; }
}
