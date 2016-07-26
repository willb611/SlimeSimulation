using SlimeSimulation.Model;
using System.Collections.Generic;

namespace SlimeSimulation.FlowCalculation {
    public class Graph {
        private readonly List<Edge> edges;
        private readonly List<Node> nodes;
        private Dictionary<Node, List<Edge>> edgesConnectedToNodeMapping;

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
            edgesConnectedToNodeMapping = MakeEdgesConnectedToNodeMapping(edges, nodes);
        }

        public Graph(List<Edge> edges) {
            this.edges = edges;
            List<Node> nodes = new List<Node>();
            foreach (Edge edge in Edges) {
                AddIfNotContained(edge.A, ref nodes);
                AddIfNotContained(edge.B, ref nodes);
            }
            this.nodes = nodes;
            edgesConnectedToNodeMapping = MakeEdgesConnectedToNodeMapping(edges, nodes);
        }

        private Dictionary<Node, List<Edge>> MakeEdgesConnectedToNodeMapping(List<Edge> edges, List<Node> nodes) {
            Dictionary<Node, List<Edge>> mapping = new Dictionary<Node, List<Edge>>();
            foreach (Node node in nodes) {
                mapping[node] = new List<Edge>();
            }
            foreach (Edge edge in edges) {
                mapping[edge.A].Add(edge);
                mapping[edge.B].Add(edge);
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

        internal IEnumerable<Node> Neighbours(Node node) {
            List<Node> connected = new List<Node>();
            foreach (Edge edge in EdgesConnectedToNode(node)) {
                connected.Add(edge.GetOtherNode(node));
            }
            return connected;
        }

        internal List<Edge> EdgesConnectedToNode(Node node) {
            return edgesConnectedToNodeMapping[node];
        }
    }
}
