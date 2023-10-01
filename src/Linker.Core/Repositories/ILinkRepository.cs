namespace Linker.Core.Repositories
{
    using System.Collections.Generic;
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
        /// <returns>The task.</returns>
        Task AddAsync(T item);

        /// <summary>
        /// Retrieves all the available data from the database.
        /// </summary>
        /// <returns>The entire list of data.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Retrieves the item by ID.
        /// </summary>
        /// <param name="id">The id of the item.</param>
        /// <returns>The item itself.</returns>
        Task<T> GetByIdAsync(string id);

        /// <summary>
        /// Deletes an item from the database with the given id.
        /// </summary>
        /// <param name="id">The id of the item to be removed.</param>
        /// <returns>The awaitable task.</returns>
        Task RemoveAsync(string id);

        /// <summary>
        /// Updates an item with the new details provided.
        /// </summary>
        /// <param name="item">The item with updated details.</param>
        /// <returns>The awaitable task.</returns>
        Task UpdateAsync(T item);
    }
}
