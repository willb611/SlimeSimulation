using System.Collections.Generic;

namespace SlimeSimulation.Model
{
    public class SlimeNetwork
    {
        public ISet<Node> Nodes { get; }
        public ISet<FoodSourceNode> FoodSources { get; }
        public ISet<SlimeEdge> Edges { get; }

        public SlimeNetwork(ISet<Node> nodes, ISet<FoodSourceNode> foodSources,
          ISet<SlimeEdge> edges)
        {
            this.Nodes = nodes;
            this.Edges = edges;
            this.FoodSources = foodSources;
        }

    }
}
