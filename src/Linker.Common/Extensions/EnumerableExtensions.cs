namespace Linker.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The static class for custom <see cref="IEnumerable{T}"/> extension methods.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Generate index for any <see cref="IEnumerable{T}"/> collection.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="self">The reference to self.</param>
        /// <returns>An enumerable with tuple of the item and index.</returns>
        public static IEnumerable<(T Item, int Index)> WithIndex<T>(this IEnumerable<T> self)
            => self.Select((item, index) => (item, index));

        /// <summary>
        /// Skip items in the list or take all.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="src">The source.</param>
        /// <param name="skip">Number of items to skip.</param>
        /// <returns>The queried results.</returns>
        public static IEnumerable<T> SkipOrAll<T>(this IEnumerable<T> src, int? skip)
        {
            return skip.HasValue ? src.Skip(skip.Value) : src;
        }

        /// <summary>
        /// Take a few items in the list or take all.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="src">The source.</param>
        /// <param name="take">Number of items to take.</param>
        /// <returns>The queried results.</returns>
        public static IEnumerable<T> TakeOrAll<T>(this IEnumerable<T> src, int? take)
        {
            return take.HasValue ? src.Take(take.Value) : src;
        }

        /// <summary>
        /// Take a few items in the list from behind or take all.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="src">The source.</param>
        /// <param name="last">Number of items to take from behind.</param>
        /// <returns>The queried results.</returns>
        public static IEnumerable<T> LastOrAll<T>(this IEnumerable<T> src, int? last)
        {
            return last.HasValue ? src.Skip(Math.Max(0, src.Count() - last.Value)) : src;
        }
    }
}
