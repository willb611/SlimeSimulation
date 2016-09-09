using System;
using System.Collections.Generic;
using NLog;

namespace SlimeSimulation.Model
{
    public class Edges
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
        private static void AddNodesFromEdge(Edge edge, ref ISet<Node> nodes)
        {
            nodes.Add(edge.A);
            nodes.Add(edge.B);
        }
        
        public static int GetMaxNodeId(ISet<Edge> edges)
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
            return new HashSet<Edge>(slimeEdges);
        }

        public static ISet<SlimeEdge> CastToSlimeEdges(ISet<Edge> edges)
        {
            var result = new HashSet<SlimeEdge>();
            foreach (var edge in edges)
            {
                var slimeEdge = edge as SlimeEdge;
                if (slimeEdge != null)
                {
                    result.Add(slimeEdge);
                }
                else
                {
                    Logger.Warn("[CastToSlimeEdges] Given set containing non-slime edge: {0}", edge);
                }
            }
            return result;
        }
    }
}
