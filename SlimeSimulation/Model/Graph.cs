using SlimeSimulation.Model;
using System.Collections.Generic;
using System;
using NLog;
using NLog.Targets.Wrappers;
using System.Linq;

namespace SlimeSimulation.Model {
    public class Graph {
        private readonly ISet<Edge> edges;
        private readonly ISet<Node> nodes;
        private Dictionary<Node, ISet<Edge>> edgesConnectedToNodeMapping;

        public Graph(ISet<Edge> edges) {
            this.edges = edges;
            ISet<Node> nodes = new HashSet<Node>();
            foreach (Edge edge in Edges) {
                AddNodesInEdgeNotContained(edge, ref nodes);
            }
            this.nodes = nodes;
            edgesConnectedToNodeMapping = MakeEdgesConnectedToNodeMapping(edges, nodes);
        }
        public Graph(ISet<Edge> edges, ISet<Node> nodes) {
            this.nodes = nodes;
            this.edges = edges;
            edgesConnectedToNodeMapping = MakeEdgesConnectedToNodeMapping(edges, nodes);
        }
        private void AddNodesInEdgeNotContained(Edge edge, ref ISet<Node> nodes) {
            nodes.Add(edge.A);
            nodes.Add(edge.B);
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

        internal Edge GetEdgeBetween(Node a, Node b) {
            foreach (Edge edge in EdgesConnectedToNode(a)) {
                if (edge.GetOtherNode(a).Equals(b)) {
                    return edge;
                }
            }
            throw new ArgumentException("Couldn't find an edge between nodes: " + a + ", and: " + b);
        }

        internal bool EdgeExistsBetween(Node a, Node b) {
            foreach (Edge edge in EdgesConnectedToNode(a)) {
                if (edge.GetOtherNode(a).Equals(b)) {
                    return true;
                }
            }
            return false;
        }

        internal double GetEdgeConnectivityOrZero(Node a, Node b) {
            if (EdgeExistsBetween(a, b)) {
                return GetEdgeBetween(a, b).Connectivity;
            } else {
                return 0;
            }
        }
    }
}
