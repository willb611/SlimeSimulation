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
                AddNodesInEdgeNotContained(edge, ref nodes);
            }
            return nodes;
        }
        private static void AddNodesInEdgeNotContained(Edge slimeEdge, ref ISet<Node> nodes)
        {
            nodes.Add(slimeEdge.A);
            nodes.Add(slimeEdge.B);
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
