using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NLog;

namespace SlimeSimulation.Model.Generation
{
    public abstract class GraphWithFoodSourcesGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public abstract GraphWithFoodSources Generate();

        public ISet<Edge> CreateEdgesBetweenNodesInOrder(List<Node> row)
        {
            if (row == null)
            {
                Logger.Warn("[JoinCellsInRow] Given null row");
                return new HashSet<Edge>();
            }
            var result = new HashSet<Edge>();
            for (int i = 1; i < row.Count; i++)
            {
                Edge e = new Edge(row[i-1], row[i]);
                result.Add(e);
            }
            return result;
        }

        public ISet<Edge> CreateEdgesBetweenRowsAtSameIndex(List<Node> row, List<Node> otherRow)
        {
            if (row == null || otherRow == null)
            {
                Logger.Warn("[CreateEdgesConnectingRowsVertically] Given null row");
                return new HashSet<Edge>();
            }
            var result = new HashSet<Edge>();
            for (int i = 0; i < row.Count && i < otherRow.Count; i++)
            {
                Edge e = new Edge(row[i], otherRow[i]);
                result.Add(e);
            }
            return result;
        }
    }
}
