using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linker.ConsoleUI.Extensions
{
    public static class Enumerable
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
            => self.Select((item, index) => (item, index));
    }
}
