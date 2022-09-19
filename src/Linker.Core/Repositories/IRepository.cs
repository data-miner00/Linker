namespace Linker.Core.Repositories
{
    using System.Collections.Generic;
    using Linker.Core.Models;

    /// <summary>
    /// The base interface for link repositories.
    /// </summary>
    /// <typeparam name="T">The object that extends <see cref="Link"/>.</typeparam>
    public interface IRepository<T> : ITransactionalRepository
        where T : Link
    {
        /// <summary>
        /// Add a new item into the database.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        public void Add(T item);

        /// <summary>
        /// Retrieves all the available data from the database.
        /// </summary>
        /// <returns>The entire list of data.</returns>
        public IEnumerable<T> GetAll();

        /// <summary>
        /// Retrieves the item by ID.
        /// </summary>
        /// <param name="id">The id of the item.</param>
        /// <returns>The item itself.</returns>
        public T GetById(string id);

        /// <summary>
        /// Deletes an item from the database with the given id.
        /// </summary>
        /// <param name="id">The id of the item to be removed.</param>
        public void Remove(string id);

        /// <summary>
        /// Updates an item with the new details provided.
        /// </summary>
        /// <param name="item">The item with updated details.</param>
        public void Update(T item);
    }
}
