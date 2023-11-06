#nullable disable
namespace Linker.Core.Models
{
    using System;

    /// <summary>
    /// The user of the application.
    /// </summary>
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
        /// Gets or sets the password of the user.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the role associated with the user.
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// Gets or sets the status of the user.
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Gets or sets the date that the user was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date by which the user was last updated.
        /// </summary>
        public DateTime ModifiedAt { get; set; }
    }
}
