using System;
using System.Collections.Generic;
using System.Linq;

namespace SlimeSimulation.StdLibHelpers
{
    public static class EnumerableExtension
    {
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T item)
        {
            var items = source.ToList();
            return items.Except(item);
        }
    }
}
