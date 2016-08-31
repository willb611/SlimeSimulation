using System.Collections.Generic;

namespace SlimeSimulation.StdLibHelpers
{
    public static class EnumeratorExtension
    {
        public static List<T> AsList<T>(this IEnumerator<T> source)
        {
            var list = new List<T>();
            while (source.MoveNext())
            {
                list.Add(source.Current);
            }
            return list;
        }
    }
}
