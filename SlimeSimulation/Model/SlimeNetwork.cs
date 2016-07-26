using System.Collections.Generic;

namespace SlimeSimulation.Model {
    public class SlimeNetwork {
        List<Node> nodes;
        List<FoodSourceNode> foodSources;
        List<Edge> edges;
        List<Loop> loops;

        public SlimeNetwork(List<Node> nodes, List<FoodSourceNode> foodSources,
            List<Edge> edges, List<Loop> loops) {
            this.nodes = nodes;
            this.edges = edges;
            this.foodSources = foodSources;
            this.loops = loops;
        }

        public List<Node> Nodes {
            get {
                return nodes;
            }
        }

        internal List<FoodSourceNode> FoodSources {
            get {
                return foodSources;
            }
        }

        public List<Edge> Edges {
            get {
                return edges;
            }
        }

        public List<Loop> Loops {
            get {
                return loops;
            }
        }
    }
}
