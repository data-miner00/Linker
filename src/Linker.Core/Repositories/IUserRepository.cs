namespace Linker.Core.Repositories
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Linker.Core.Models;

    /// <summary>
    /// The abstraction for the user repository.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Add a new user into the database.
        /// </summary>
        /// <param name="user">The user to be created.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task.</returns>
        Task AddAsync(User user, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves all the users from the database.
        /// </summary>
        /// <returns>The collection of users.</returns>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves the user by ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The found user.</returns>
        Task<User> GetByIdAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves the user by username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The found user.</returns>
        Task<User> GetByUsernameAsync(string username, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves the user by username and password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The found user.</returns>
        Task<User> GetByUsernameAndPasswordAsync(string username, string password, CancellationToken cancellationToken);

        /// <summary>
        /// Removes a user from the database by ID.
        /// </summary>
        /// <param name="id">The ID of the user to be removed.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task.</returns>
        Task RemoveAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Updates a user with the new details provided.
        /// </summary>
        /// <param name="user">The user with updated details.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task.</returns>
        Task UpdateAsync(User user, CancellationToken cancellationToken);
    }
}
