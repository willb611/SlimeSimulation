using System.Collections.Generic;
using System.Linq;

namespace SlimeSimulation.Model
{
    public class SlimeNetwork : GraphWithFoodSources
    {
        public new ISet<SlimeEdge> Edges { get; }

        public SlimeNetwork(IEnumerable<Node> nodes, ISet<FoodSourceNode> foodSources, ISet<SlimeEdge> edges) : this(new HashSet<Node>(nodes), foodSources, edges)
        {
        }

        public SlimeNetwork(ISet<Node> nodes, ISet<FoodSourceNode> foodSources,
            ISet<SlimeEdge> edges) : base(new HashSet<Edge>(edges), nodes, foodSources)
        {
            Edges = edges;
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
            if (!Nodes.SequenceEqual(graphWithFoodSources.Nodes))
            {
                return false;
            }
            if (!FoodSources.SetEquals(graphWithFoodSources.FoodSources))
            {
                return false;
            }
            if (Edges.Count != graphWithFoodSources.Edges.Count)
            {
                return false;
            }
            return Edges.All(slimEdge => graphWithFoodSources.Edges.Contains(slimEdge.Edge));
        }
    }
}
