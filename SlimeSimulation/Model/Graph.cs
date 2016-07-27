using SlimeSimulation.Model;
using System.Collections.Generic;
using System;

namespace SlimeSimulation.FlowCalculation {
    public class Graph {
        private readonly ISet<Edge> edges;
        private readonly ISet<Node> nodes;
        private Dictionary<Node, ISet<Edge>> edgesConnectedToNodeMapping;

        public Graph(ISet<Edge> edges) {
            this.edges = edges;
            ISet<Node> nodes = new HashSet<Node>();
            foreach (Edge edge in Edges) {
                AddIfNotContained(edge.A, ref nodes);
                AddIfNotContained(edge.B, ref nodes);
            }
            this.nodes = nodes;
            edgesConnectedToNodeMapping = MakeEdgesConnectedToNodeMapping(edges, nodes);
        }

        public Graph(ISet<Edge> edges, ISet<Node> nodes) {
            this.nodes = nodes;
            this.edges = edges;
            edgesConnectedToNodeMapping = MakeEdgesConnectedToNodeMapping(edges, nodes);
        }


        public IEnumerable<Node> Nodes {
            get {
                return nodes;
            }
        }

        public ISet<Edge> Edges {
            get {
                return edges;
            }
        }
        

        private Dictionary<Node, ISet<Edge>> MakeEdgesConnectedToNodeMapping(ISet<Edge> edges, ISet<Node> nodes) {
            Dictionary<Node, ISet<Edge>> mapping = new Dictionary<Node, ISet<Edge>>();
            foreach (Node node in nodes) {
                mapping[node] = new HashSet<Edge>();
            }
            foreach (Edge edge in edges) {
                mapping[edge.A].Add(edge);
                mapping[edge.B].Add(edge);
            }
            return mapping;
        }

        private void AddNodesInEdgeNotContained(Edge edge, ref ISet<Node> nodes) {
            AddIfNotContained(edge.A, ref nodes);
            AddIfNotContained(edge.B, ref nodes);
        }

        private void AddIfNotContained(Node node, ref ISet<Node> nodes) {
            if (!nodes.Contains(node)) {
                nodes.Add(node);
            }
        }

        internal IEnumerable<Node> Neighbours(Node node) {
            ISet<Node> connected = new HashSet<Node>();
            foreach (Edge edge in EdgesConnectedToNode(node)) {
                connected.Add(edge.GetOtherNode(node));
            }
            return connected;
        }

        internal ISet<Edge> EdgesConnectedToNode(Node node) {
            return edgesConnectedToNodeMapping[node];
        }
    }
}
