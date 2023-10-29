namespace Linker.Core.Models
{
    /// <summary>
    /// The role of a user.
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// The administrator that has full access to the system.
        /// </summary>
        Administrator,

        /// <summary>
        /// A regular user that uses the application.
        /// </summary>
        User,

        /// <summary>
        /// A guest user that only have read access to the application.
        /// </summary>
        Guest,
    }
}
