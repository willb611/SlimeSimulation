using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation
{
  internal class LogHelper
  {
    private static Logger logger = LogManager.GetCurrentClassLogger();

    public static String CollectionToString<T>(ICollection<T> collection)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append(collection.GetType());
      sb.Append("{Count=").Append(collection.Count).Append(",Elements=[");
      foreach (T element in collection)
      {
        sb.Append(element.ToString());
      }
      sb.Append("]").Append("}");
      return sb.ToString();
    }

    public static String PrintArr(double[] arr)
    {
      return AppendRowToBuilder(arr, new StringBuilder()).ToString();
    }

    public static String PrintArr(double[][] arr)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("[PrintArr] Start").Append(Environment.NewLine);
      for (int i = 0; i < arr.Length; i++)
      {
        AppendRowToBuilder(arr[i], sb).Append(Environment.NewLine);
      }
      sb.Append("[PrintArr] Finish ").Append(Environment.NewLine);
      return sb.ToString();
    }

    private static StringBuilder AppendRowToBuilder(double[] row, StringBuilder builder)
    {
      for (int j = 0; j < row.Length; j++)
      {
        if (row[j] < 0)
        {
          builder.Append(String.Format("{0, 5:f4}", row[j]));
        }
        else
        {
          builder.Append("+")
            .Append(String.Format("{0, 5:f4}", row[j]));
        }
        builder.Append(" ");
      }
      return builder;
    }
  }
}