using System.Collections.Generic;
using System.Linq;

namespace SlimeSimulation.Model
{
    public class SlimeNetwork : Graph
    {
        public ISet<FoodSourceNode> FoodSources { get; }
        public new ISet<SlimeEdge> Edges { get; }

        public SlimeNetwork(IEnumerable<Node> nodes, ISet<FoodSourceNode> foodSources, ISet<SlimeEdge> edges) : this(new HashSet<Node>(nodes), foodSources, edges)
        {
        }
        public SlimeNetwork(ISet<Node> nodes, ISet<FoodSourceNode> foodSources,
          ISet<SlimeEdge> edges) : base(new HashSet<Edge>(edges), nodes)
        {
            this.Edges = edges;
            this.FoodSources = foodSources;
        }


        internal double GetEdgeConnectivityOrZero(Node a, Node b)
        {
            if (EdgeExistsBetween(a, b))
            {
                return GetEdgeBetween(a, b).Connectivity;
            }
            else
            {
                return 0;
            }
        }

        internal new SlimeEdge GetEdgeBetween(Node a, Node b)
        {
            return (SlimeEdge) base.GetEdgeBetween(a, b);
        }
    }
}
