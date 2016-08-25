using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using SlimeSimulation.Model.Bfs;

namespace SlimeSimulation.Model
{
    public class SlimeNetwork : GraphWithFoodSources
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ISet<SlimeEdge> SlimeEdges { get; }

        public SlimeNetwork(ISet<SlimeEdge> edges) : base(new HashSet<Edge>(RemoveDisconnectedEdges(edges)))
        {
            SlimeEdges = Cast(base.Edges);
        }
        public SlimeNetwork(ISet<Node> nodes, ISet<FoodSourceNode> foodSources,
            ISet<SlimeEdge> edges) : base(new HashSet<Edge>(RemoveDisconnectedEdges(edges)), nodes, foodSources)
        {
            if (nodes.Count > 1) // If it's not the case that the slime only consists of a single node.
            {
                Nodes = GetNodesContainedIn(base.Edges);
                FoodSources = GetFoodSourceNodes(Nodes);
            }
            SlimeEdges = Cast(base.Edges);
        }

        private ISet<SlimeEdge> Cast(ISet<Edge> edges)
        {
            var result = new HashSet<SlimeEdge>();
            foreach (var edge in edges)
            {
                var slimeEdge = edge as SlimeEdge;
                if (slimeEdge != null)
                {
                    result.Add(slimeEdge);
                }
            }
            return result;
        }

        internal double GetEdgeConnectivityOrZero(Node a, Node b)
        {
            if (EdgeExistsBetween(a, b))
            {
                return GetEdgeBetween(a, b).Connectivity;
            }
            return 0;
        }

        internal new SlimeEdge GetEdgeBetween(Node a, Node b)
        {
            return (SlimeEdge) base.GetEdgeBetween(a, b);
        }

        public bool CoversGraph(GraphWithFoodSources graphWithFoodSources)
        {
            if (graphWithFoodSources == null)
            {
                return false;
            }
            if (!Nodes.SetEquals(graphWithFoodSources.Nodes))
            {
                return false;
            }
            if (!FoodSources.SetEquals(graphWithFoodSources.FoodSources))
            {
                return false;
            }
            if (SlimeEdges.Count != graphWithFoodSources.Edges.Count)
            {
                return false;
            }
            return SlimeEdges.All(slimEdge => graphWithFoodSources.Edges.Contains(slimEdge.Edge));
        }

        public ISet<Edge> GetAllEdgesInGraphReplacingThoseWhichAreSlimed(GraphWithFoodSources graph)
        {
            var edges = new HashSet<Edge>(graph.Edges);
            foreach (var slimeEdge in SlimeEdges)
            {
                edges.Remove(slimeEdge.Edge);
                edges.Add(slimeEdge);
            }
            if (Logger.IsTraceEnabled)
            {
                Logger.Trace($"[GetAllEdgesInGraphReplacingThoseWhichAreSlimed] Returning number {edges.Count}");
            }
            return edges;
        }

        internal static HashSet<SlimeEdge> RemoveDisconnectedEdges(IEnumerable<SlimeEdge> edges)
        {
            HashSet<SlimeEdge> connected = new HashSet<SlimeEdge>();
            foreach (var slimeEdge in edges)
            {
                if (!slimeEdge.IsDisconnected())
                {
                    connected.Add(slimeEdge);
                }
            }
            return connected;
        }
    }
}
