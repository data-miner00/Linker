namespace Linker.Core.Models
{
    /// <summary>
    /// The role of a user.
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// The owner of the application. Have full access to the system
        /// including managing administrators.
        /// </summary>
        Owner,

        /// <summary>
        /// The administrator that has full access to the system.
        /// </summary>
        Administrator,

        /// <summary>
        /// A delegated user that has access to edit or delete content,
        /// but not creation.
        /// </summary>
        Moderator,

        /// <summary>
        /// A regular user that uses the application.
        /// </summary>
        User,

        /// <summary>
        /// A restricted user that only has read only access to certain parts
        /// of the resource.
        /// </summary>
        ReadOnlyUser,

        /// <summary>
        /// A non-registered user that only have read access to the application.
        /// </summary>
        Guest,
    }
}
