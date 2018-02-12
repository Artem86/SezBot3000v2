using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SezBot3000v2.Extensions
{
    public static class EnumerableExtension
    {
        public static T GetRandomElement<T>(this IEnumerable<T> collection)
        {
            var r = new Random(DateTime.Now.Millisecond);
            var replyPosition = r.Next(0, collection.Count());
            return collection.ElementAt(replyPosition);
        }
    }
}
