namespace Linker.Core.Repositories
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Linker.Core.Models;

    /// <summary>
    /// The base interface for link repositories.
    /// </summary>
    /// <typeparam name="T">The type that extends <see cref="Link"/>.</typeparam>
    public interface ILinkRepository<T>
        where T : Link
    {
        /// <summary>
        /// Add a new item into the database.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task.</returns>
        Task AddAsync(T item, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves all the available data from the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entire list of data.</returns>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves the item by ID.
        /// </summary>
        /// <param name="id">The id of the item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The item itself.</returns>
        Task<T> GetByIdAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes an item from the database with the given id.
        /// </summary>
        /// <param name="id">The id of the item to be removed.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The awaitable task.</returns>
        Task RemoveAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an item with the new details provided.
        /// </summary>
        /// <param name="item">The item with updated details.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The awaitable task.</returns>
        Task UpdateAsync(T item, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves all items that is owned by the user.
        /// </summary>
        /// <param name="userId">Ths user ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of items that the user owns.</returns>
        Task<IEnumerable<T>> GetAllByUserAsync(string userId, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a particular item that is owned by the user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="linkId">The link ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The found item.</returns>
        Task<T> GetByUserAsync(string userId, string linkId, CancellationToken cancellationToken);
    }
}
