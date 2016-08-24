using SlimeSimulation.Model;
using System.Collections.Generic;
using System;
using NLog;
using NLog.Targets.Wrappers;
using System.Linq;

namespace SlimeSimulation.Model
{
    public class Graph
    {
        private readonly ISet<SlimeEdge> _edges;
        private readonly ISet<Node> _nodes;
        private readonly Dictionary<Node, ISet<SlimeEdge>> _edgesConnectedToNodeMapping;

        public Graph(ISet<SlimeEdge> edges)
        {
            this._edges = edges;
            ISet<Node> nodes = new HashSet<Node>();
            foreach (var edge in Edges)
            {
                AddNodesInEdgeNotContained(edge, ref nodes);
            }
            this._nodes = nodes;
            _edgesConnectedToNodeMapping = MakeEdgesConnectedToNodeMapping(edges, nodes);
        }

        public Graph(ISet<SlimeEdge> edges, ISet<Node> nodes)
        {
            this._nodes = nodes;
            this._edges = edges;
            _edgesConnectedToNodeMapping = MakeEdgesConnectedToNodeMapping(edges, nodes);
        }

        private void AddNodesInEdgeNotContained(SlimeEdge slimeEdge, ref ISet<Node> nodes)
        {
            nodes.Add(slimeEdge.A);
            nodes.Add(slimeEdge.B);
        }

        public IEnumerable<Node> Nodes {
            get { return _nodes; }
        }

        public ISet<SlimeEdge> Edges {
            get { return _edges; }
        }

        private Dictionary<Node, ISet<SlimeEdge>> MakeEdgesConnectedToNodeMapping(ISet<SlimeEdge> edges, ISet<Node> nodes)
        {
            var mapping = new Dictionary<Node, ISet<SlimeEdge>>();
            foreach (var node in nodes)
            {
                mapping[node] = new HashSet<SlimeEdge>();
            }
            foreach (var edge in edges)
            {
                mapping[edge.A].Add(edge);
                mapping[edge.B].Add(edge);
            }
            return mapping;
        }

        internal IEnumerable<Node> Neighbours(Node node)
        {
            ISet<Node> connected = new HashSet<Node>();
            foreach (var edge in EdgesConnectedToNode(node))
            {
                connected.Add(edge.GetOtherNode(node));
            }
            return connected;
        }

        internal ISet<SlimeEdge> EdgesConnectedToNode(Node node)
        {
            return _edgesConnectedToNodeMapping[node];
        }

        internal SlimeEdge GetEdgeBetween(Node a, Node b)
        {
            foreach (var edge in EdgesConnectedToNode(a))
            {
                if (edge.GetOtherNode(a).Equals(b))
                {
                    return edge;
                }
            }
            throw new ArgumentException("Couldn't find an SlimeEdge between nodes: " + a + ", and: " + b);
        }

        internal bool EdgeExistsBetween(Node a, Node b)
        {
            return EdgesConnectedToNode(a).Any(edge => edge.GetOtherNode(a).Equals(b));
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
    }
}
