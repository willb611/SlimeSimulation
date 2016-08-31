using System.Collections.Generic;
using System.Linq;
using NLog;

namespace SlimeSimulation.Model
{
    public class SlimeNetwork : GraphWithFoodSources
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ISet<SlimeEdge> SlimeEdges { get; }

        public SlimeNetwork(ISet<SlimeEdge> edges) : base(new HashSet<Edge>(edges))
        {
            SlimeEdges = Edges.CastToSlimeEdges(base.EdgesInGraph);
        }
        public SlimeNetwork(ISet<Node> nodesInGraph, ISet<FoodSourceNode> foodSources,
            ISet<SlimeEdge> edges) : base(new HashSet<Edge>(edges), nodesInGraph, foodSources)
        {
            SlimeEdges = Edges.CastToSlimeEdges(base.EdgesInGraph);
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

        public override bool Equals(object obj)
        {
            return Equals(obj as SlimeNetwork);
        }
        public bool Equals(SlimeNetwork other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }

            return SlimeEdges.Equals(other.SlimeEdges)
                   && base.Equals(other);
        }
        public override int GetHashCode()
        {
            return SlimeEdges.GetHashCode() * 17 + base.GetHashCode();
        }

        public bool InvalidSourceSink(Node source, Node sink)
        {
            if (Equals(source, sink))
            {
                return true;
            }
            else
            {
                return !this.RouteExistsBetween(source, sink);
            }
        }
    }
}
