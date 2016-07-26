using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.FlowCalculation {
    public class Graph {
        private readonly List<Edge> edges;
        private readonly List<Node> nodes;
        private Dictionary<Node, List<Node>> neighbourMapping;

        public IEnumerable<Node> Nodes {
            get {
                return nodes;
            }
        }

        public List<Edge> Edges {
            get {
                return edges;
            }
        }

        public Graph(List<Node> nodes, List<Edge> edges) {
            this.nodes = nodes;
            this.edges = edges;
            neighbourMapping = MakeNeighbourMapping(edges, nodes);
        }

        public Graph(List<Edge> edges) {
            this.edges = edges;
            List<Node> nodes = new List<Node>();
            foreach (Edge edge in Edges) {
                AddIfNotContained(edge.A, ref nodes);
                AddIfNotContained(edge.B, ref nodes);
            }
            this.nodes = nodes;
            neighbourMapping = MakeNeighbourMapping(edges, nodes);
        }

        private Dictionary<Node, List<Node>> MakeNeighbourMapping(List<Edge> edges, List<Node> nodes) {
            Dictionary<Node, List<Node>> mapping = new Dictionary<Node, List<Node>>();
            foreach (Node node in nodes) {
                mapping[node] = new List<Node>();
            }
            foreach (Edge edge in edges) {
                if (!mapping[edge.A].Contains(edge.B)) {
                    mapping[edge.A].Add(edge.B);
                }
                if (!mapping[edge.B].Contains(edge.A)) {
                    mapping[edge.B].Add(edge.A);
                }
            }
            return mapping;
        }

        private void AddNodesInEdgeNotContained(Edge edge, ref List<Node> nodes) {
            AddIfNotContained(edge.A, ref nodes);
            AddIfNotContained(edge.B, ref nodes);
        }

        private void AddIfNotContained(Node node, ref List<Node> nodes) {
            if (!nodes.Contains(node)) {
                nodes.Add(node);
            }
        }

        internal List<Node> Neighbours(Node node) {
            return neighbourMapping[node];
        }
    }
}
