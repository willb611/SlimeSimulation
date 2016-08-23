using System.Collections.Generic;

namespace SlimeSimulation.Model
{
    public class SlimeNetwork
    {
        readonly ISet<Node> _nodes;
        readonly ISet<FoodSourceNode> _foodSources;
        readonly ISet<Edge> _edges;

        public SlimeNetwork(ISet<Node> nodes, ISet<FoodSourceNode> foodSources,
          ISet<Edge> edges)
        {
            this._nodes = nodes;
            this._edges = edges;
            this._foodSources = foodSources;
        }

        public ISet<Node> Nodes {
            get { return _nodes; }
        }

        internal ISet<FoodSourceNode> FoodSources {
            get { return _foodSources; }
        }

        public ISet<Edge> Edges {
            get { return _edges; }
        }
    }
}
