using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation {
    internal class LogHelper {
        public static String ListToString<T>(List<T> list) {
            StringBuilder sb = new StringBuilder();
            sb.Append("List{Count=").Append(list.Count).Append(",Elements=[");
            foreach (T element in list) {
                sb.Append(element.ToString());
            }
            sb.Append("]").Append("}");
            return sb.ToString();
        }
    }
}
