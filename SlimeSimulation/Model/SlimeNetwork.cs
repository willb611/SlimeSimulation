using System.Collections.Generic;

namespace SlimeSimulation.Model
{
    public class SlimeNetwork
    {
        ISet<Node> nodes;
        ISet<FoodSourceNode> foodSources;
        ISet<Edge> edges;

        public SlimeNetwork(ISet<Node> nodes, ISet<FoodSourceNode> foodSources,
          ISet<Edge> edges)
        {
            this.nodes = nodes;
            this.edges = edges;
            this.foodSources = foodSources;
        }

        public ISet<Node> Nodes {
            get { return nodes; }
        }

        internal ISet<FoodSourceNode> FoodSources {
            get { return foodSources; }
        }

        public ISet<Edge> Edges {
            get { return edges; }
        }
    }
}
