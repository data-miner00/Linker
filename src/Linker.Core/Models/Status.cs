namespace Linker.Core.Models
{
    /// <summary>
    /// The status of a user.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// The user has been created but not activated.
        /// </summary>
        Created,

        /// <summary>
        /// The user that is activated.
        /// </summary>
        Active,

        /// <summary>
        /// The user that is suspended due to violation to policy.
        /// </summary>
        Suspended,

        /// <summary>
        /// The user that is banned due to violation to policy.
        /// </summary>
        Banned,

        /// <summary>
        /// The user that is voluntarily deactivated.
        /// </summary>
        Deactivated,
    }
}
