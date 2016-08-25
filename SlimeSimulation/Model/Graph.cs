using System.Collections.Generic;
using System;
using System.Linq;

namespace SlimeSimulation.Model
{
    public class Graph
    {
        private readonly ISet<Edge> _edges;
        private readonly ISet<Node> _nodes;
        private readonly Dictionary<Node, ISet<Edge>> _edgesConnectedToNodeMapping;

        public Graph(ISet<Edge> edges)
        {
            _edges = edges;
            ISet<Node> nodes = new HashSet<Node>();
            foreach (var edge in Edges)
            {
                AddNodesInEdgeNotContained(edge, ref nodes);
            }
            _nodes = nodes;
            _edgesConnectedToNodeMapping = MakeEdgesConnectedToNodeMapping(edges, nodes);
        }

        public Graph(ISet<Edge> edges, ISet<Node> nodes)
        {
            _nodes = nodes;
            _edges = edges;
            _edgesConnectedToNodeMapping = MakeEdgesConnectedToNodeMapping(edges, nodes);
        }

        private void AddNodesInEdgeNotContained(Edge slimeEdge, ref ISet<Node> nodes)
        {
            nodes.Add(slimeEdge.A);
            nodes.Add(slimeEdge.B);
        }

        public IEnumerable<Node> Nodes {
            get { return _nodes; }
        }

        public ISet<Edge> Edges {
            get { return _edges; }
        }

        private Dictionary<Node, ISet<Edge>> MakeEdgesConnectedToNodeMapping(ISet<Edge> edges, ISet<Node> nodes)
        {
            var mapping = new Dictionary<Node, ISet<Edge>>();
            foreach (var node in nodes)
            {
                mapping[node] = new HashSet<Edge>();
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

        internal ISet<Edge> EdgesConnectedToNode(Node node)
        {
            return _edgesConnectedToNodeMapping[node];
        }

        internal Edge GetEdgeBetween(Node a, Node b)
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

        public override bool Equals(object obj)
        {
            return Equals(obj as Graph);
        }

        public bool Equals(Graph other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            else if (ReferenceEquals(other, this))
            {
                return true;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }

            return Edges.Equals(other.Edges) && Nodes.Equals(other.Nodes);
        }

        public override int GetHashCode()
        {
            return Edges.GetHashCode() * 17 + Nodes.GetHashCode();
        }
    }
}
