using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model
{
    public class Edges
    {
        public static ISet<Node> GetNodesContainedIn(ISet<SlimeEdge> edges)
        {
            return GetNodesContainedIn(FromSlimeEdges(edges));
        }

        public static ISet<Node> GetNodesContainedIn(ISet<Edge> edges)
        {
            ISet<Node> nodes = new HashSet<Node>();
            foreach (var edge in edges)
            {
                AddNodesFromEdge(edge, ref nodes);
            }
            return nodes;
        }
        private static void AddNodesFromEdge(Edge slimeEdge, ref ISet<Node> nodes)
        {
            nodes.Add(slimeEdge.A);
            nodes.Add(slimeEdge.B);
        }

        internal static bool[,] GetConnectedNodes(ISet<Edge> edges)
        {
            var max = GetMaxNodeId(edges);
            var connected = new bool[max+1, max+1];
            foreach (var edge in edges)
            {
                connected[edge.A.Id, edge.B.Id] = true;
                connected[edge.B.Id, edge.A.Id] = true;
            }
            return connected;
        }

        private static int GetMaxNodeId(ISet<Edge> edges)
        {
            int max = 0;
            foreach (var edge in edges)
            {
                max = Math.Max(max, edge.A.Id);
                max = Math.Max(max, edge.B.Id);
            }
            return max;
        }

        public static HashSet<Edge> FromSlimeEdges(ISet<SlimeEdge> slimeEdges)
        {
            HashSet<Edge> edges = new HashSet<Edge>();
            foreach (var slimeEdge in slimeEdges)
            {
                edges.Add(slimeEdge.Edge);
            }
            return edges;
        }
    }
}
