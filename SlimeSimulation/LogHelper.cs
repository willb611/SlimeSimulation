using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation {
    internal class LogHelper {
        public static String CollectionToString<T>(ICollection<T> collection) {
            StringBuilder sb = new StringBuilder();
            sb.Append(collection.GetType());
            sb.Append("{Count=").Append(collection.Count).Append(",Elements=[");
            foreach (T element in collection) {
                sb.Append(element.ToString());
            }
            sb.Append("]").Append("}");
            return sb.ToString();
        }
    }
}
