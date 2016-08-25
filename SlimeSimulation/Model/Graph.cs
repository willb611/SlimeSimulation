using System;
using System.Collections.Generic;
using System.Linq;
using SlimeSimulation.Model.Bfs;

namespace SlimeSimulation.Model
{
    public class Graph
    {
        public ISet<Node> Nodes { get; protected set; }
        public ISet<Edge> Edges { get; }
        private readonly Dictionary<Node, ISet<Edge>> _edgesConnectedToNodeMapping;
        private static readonly BfsSolver BfsSolver = new BfsSolver();

        public Graph(ISet<Edge> edges)
        {
            Edges = edges;
            Nodes = GetNodesContainedIn(edges);
            _edgesConnectedToNodeMapping = MakeEdgesConnectedToNodeMapping(edges, Nodes);
        }


        public Graph(ISet<Edge> edges, ISet<Node> nodes)
        {
            Nodes = nodes;
            Edges = edges;
            _edgesConnectedToNodeMapping = MakeEdgesConnectedToNodeMapping(edges, nodes);
        }

        protected ISet<Node> GetNodesContainedIn(ISet<Edge> edges)
        {
            ISet<Node> nodes = new HashSet<Node>();
            foreach (var edge in Edges)
            {
                AddNodesInEdgeNotContained(edge, ref nodes);
            }
            return nodes;
        }

        private void AddNodesInEdgeNotContained(Edge slimeEdge, ref ISet<Node> nodes)
        {
            nodes.Add(slimeEdge.A);
            nodes.Add(slimeEdge.B);
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

        public bool RouteExistsBetween(Node a, Node b)
        {
            var bfsResult = BfsSolver.From(a, this);
            return bfsResult.Connected(b);
        }

        public ISet<Node> AllNodesConnectedTo(Node source)
        {
            var bfsResult = BfsSolver.From(source, this);
            return bfsResult.ConnectedNodes();
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
            if (ReferenceEquals(other, this))
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
