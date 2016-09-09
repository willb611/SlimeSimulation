using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
