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

        public SlimeNetwork(ISet<SlimeEdge> edges) : base(new HashSet<Edge>(edges))
        {
            SlimeEdges = Cast(base.EdgesInGraph);
        }
        public SlimeNetwork(ISet<Node> nodesInGraph, ISet<FoodSourceNode> foodSources,
            ISet<SlimeEdge> edges) : base(new HashSet<Edge>(edges), nodesInGraph, foodSources)
        {
            SlimeEdges = Cast(base.EdgesInGraph);
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
            var edge = GetEdgeOrNullBetween(a, b) as SlimeEdge;
            if (edge != null)
            {
                return edge.Connectivity;
            }
            return 0;
        }

        public bool CoversGraph(GraphWithFoodSources graphWithFoodSources)
        {
            if (graphWithFoodSources == null)
            {
                return false;
            }
            if (!NodesInGraph.SetEquals(graphWithFoodSources.NodesInGraph))
            {
                return false;
            }
            if (!FoodSources.SetEquals(graphWithFoodSources.FoodSources))
            {
                return false;
            }
            if (SlimeEdges.Count != graphWithFoodSources.EdgesInGraph.Count)
            {
                return false;
            }
            return SlimeEdges.All(slimEdge => graphWithFoodSources.EdgesInGraph.Contains(slimEdge.Edge));
        }

        public ISet<Edge> GetAllEdgesInGraphReplacingThoseWhichAreSlimed(GraphWithFoodSources graph)
        {
            var edges = new HashSet<Edge>(graph.EdgesInGraph);
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
    }
}
