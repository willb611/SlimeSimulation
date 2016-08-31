using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
