using System.Collections.Generic;

namespace SlimeSimulation.StdLibHelpers
{
    public static class CollectionExtension
    {
        public static ICollection<T> Except<T>(this ICollection<T> source, T element)
        {
            ICollection<T> clone = new List<T>(source);
            clone.Remove(element);
            return clone;
        }
    }
}
