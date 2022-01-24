using System.Collections.Generic;

namespace DolDoc.HolyC
{
    internal static class Extensions
    {
        internal static void AddRange<T>(this IList<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
                collection.Add(item);
        }
    }
}
