namespace Linker.Cli.Core.Serializers;

using System.Collections.Generic;

/// <summary>
/// The custom serializer interface.
/// </summary>
/// <typeparam name="T">The type of the object.</typeparam>
public interface ISerializer<in T>
{
    /// <summary>
    /// Serializes a collection of objects into string.
    /// </summary>
    /// <param name="items">The collection of objects.</param>
    /// <returns>The serialized string.</returns>
    string Serialize(IEnumerable<T> items);

    /// <summary>
    /// Serializes an object into string.
    /// </summary>
    /// <param name="item">The object.</param>
    /// <returns>The serialized string.</returns>
    string Serialize(T item);
}
