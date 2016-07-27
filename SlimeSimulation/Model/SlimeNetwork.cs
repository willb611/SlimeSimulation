using System.Collections.Generic;

namespace SlimeSimulation.Model {
    public class SlimeNetwork {
        ISet<Node> nodes;
        ISet<FoodSourceNode> foodSources;
        ISet<Edge> edges;
        ISet<Loop> loops;

        public SlimeNetwork(ISet<Node> nodes, ISet<FoodSourceNode> foodSources,
            ISet<Edge> edges, ISet<Loop> loops) {
            this.nodes = nodes;
            this.edges = edges;
            this.foodSources = foodSources;
            this.loops = loops;
        }

        public ISet<Node> Nodes {
            get {
                return nodes;
            }
        }

        internal ISet<FoodSourceNode> FoodSources {
            get {
                return foodSources;
            }
        }

        public ISet<Edge> Edges {
            get {
                return edges;
            }
        }

        public ISet<Loop> Loops {
            get {
                return loops;
            }
        }
    }
}
