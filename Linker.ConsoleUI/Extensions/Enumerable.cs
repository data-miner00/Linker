namespace Linker.ConsoleUI.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class Enumerable
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
            => self.Select((item, index) => (item, index));
    }
}
