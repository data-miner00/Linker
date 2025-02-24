namespace Linker.Cli.Integrations;

using System.Collections.Generic;

/// <summary>
/// The base interface for repositories.
/// </summary>
/// <typeparam name="T">The object type.</typeparam>
public interface IRepository<T>
{
    /// <summary>
    /// Add a new item into the database.
    /// </summary>
    /// <param name="item">The item to be added.</param>
    /// <returns>The added Id.</returns>
    Task<int> AddAsync(T item);

    /// <summary>
    /// Retrieves all the available data from the database.
    /// </summary>
    /// <returns>The entire list of data.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Retrieves the item by ID.
    /// </summary>
    /// <param name="id">The id of the item.</param>
    /// <returns>The item itself. <c>null</c> if not found.</returns>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Deletes an item from the database with the given id.
    /// </summary>
    /// <param name="id">The id of the item to be removed.</param>
    /// <returns>The task.</returns>
    Task RemoveAsync(int id);

    /// <summary>
    /// Updates an item with the new details provided.
    /// </summary>
    /// <param name="item">The item with updated details.</param>
    /// <returns>The task.</returns>
    Task UpdateAsync(T item);

    /// <summary>
    /// Searches the <typeparamref name="T"/> entity with against a keyword.
    /// </summary>
    /// <param name="keyword">The keyword query.</param>
    /// <returns>The list of found <typeparamref name="T"/>.</returns>
    Task<IEnumerable<T>> SearchAsync(string keyword);
}
