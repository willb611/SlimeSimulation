using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NLog;

namespace SlimeSimulation.Model
{
    public class SlimeNetwork : GraphWithFoodSources
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ISet<SlimeEdge> _slimeEdges;
        public ISet<SlimeEdge> SlimeEdges => _slimeEdges;

        public SlimeNetwork(ISet<SlimeEdge> slimeEdges) : base(Edges.FromSlimeEdges(slimeEdges))
        {
            if (slimeEdges == null)
            {
                throw new ArgumentNullException(nameof(slimeEdges));
            }
            _slimeEdges = slimeEdges;
        }
        [JsonConstructor]
        public SlimeNetwork(ISet<Node> nodesInGraph, ISet<FoodSourceNode> foodSources,
            ISet<SlimeEdge> slimeEdges) : base(Edges.FromSlimeEdges(slimeEdges), nodesInGraph, foodSources)
        {
            if (nodesInGraph == null)
            {
                throw new ArgumentNullException(nameof(nodesInGraph));
            } else if (foodSources == null)
            {
                throw new ArgumentNullException(nameof(foodSources));
            } else if (slimeEdges == null)
            {
                throw new ArgumentNullException(nameof(slimeEdges));
            }
            _slimeEdges = slimeEdges;
            Logger.Trace("[Constructor : 3 params] Finished with slimeEdges.Count {3}, foodSources.Count {0}, nodesInGraph.Count {1}, edgesINGraph.Count {2}",
                FoodSources.Count, NodesInGraph.Count, EdgesInGraph.Count, SlimeEdges.Count);
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
                Logger.Debug("[CoversGraph] {0} is null", nameof(graphWithFoodSources));
                return false;
            }
            if (!NodesInGraph.SetEquals(graphWithFoodSources.NodesInGraph))
            {
                Logger.Debug("[CoversGraph] NodesInGraph doesnt equal graphWithFoodSources.NodesInGraph");
                return false;
            }
            if (!FoodSources.SetEquals(graphWithFoodSources.FoodSources))
            {
                Logger.Debug("[CoversGraph] FoodSources (count={0}) doesnt equal graphWithFoodSources.FoodSources (count={1})",
                    FoodSources.Count, graphWithFoodSources.FoodSources.Count);
                return false;
            }
            if (SlimeEdges.Count != graphWithFoodSources.EdgesInGraph.Count)
            {
                Logger.Debug("[CoversGraph] SlimeEdges.Count doesnt equal EdgesInGraph.Count");
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

        public new bool Equals(object obj)
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
        public new int GetHashCode()
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

        public override string ToString()
        {
            return base.ToString() + "{slimeEdges.Count=" + SlimeEdges.Count + ",edgesInGraph.Count=" + EdgesInGraph.Count
                + ",foodSources.Count=" + FoodSources.Count + ",nodesInGraph.Count=" + NodesInGraph.Count + "}";
        }
    }
}
