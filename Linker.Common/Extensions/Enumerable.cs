namespace Linker.Common.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The static class for custom <see cref="IEnumerable{T}"/> extension methods.
    /// </summary>
    public static class Enumerable
    {
        /// <summary>
        /// Generate index for any <see cref="IEnumerable{T}"/> collection.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="self">The reference to self.</param>
        /// <returns>An enumerable with tuple of the item and index.</returns>
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
            => self.Select((item, index) => (item, index));
    }
}
