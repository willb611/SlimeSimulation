using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NLog;
using SlimeSimulation.Model.Bfs;

namespace SlimeSimulation.Model
{
    public class Graph
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ISet<Node> NodesInGraph { get; protected set; }
        public ISet<Edge> EdgesInGraph { get; }
        private readonly Dictionary<Node, ISet<Edge>> _edgesConnectedToNodeMapping;
        private static readonly BfsSolver BfsSolver = new BfsSolver();

        public Graph(ISet<Edge> edgesInGraph) : this(edgesInGraph, Edges.GetNodesContainedIn(edgesInGraph))
        {
        }

        [JsonConstructor]
        public Graph(ISet<Edge> edgesInGraph, ISet<Node> nodesInGraph)
        {
            if (edgesInGraph == null)
            {
                throw new ArgumentNullException(nameof(edgesInGraph));
            } else if (nodesInGraph == null)
            {
                throw new ArgumentNullException(nameof(nodesInGraph));
            }
            NodesInGraph = nodesInGraph;
            EdgesInGraph = edgesInGraph;
            _edgesConnectedToNodeMapping = MakeEdgesConnectedToNodeMapping(edgesInGraph, nodesInGraph);
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
            if (_edgesConnectedToNodeMapping.ContainsKey(node))
            {
                return _edgesConnectedToNodeMapping[node];
            }
            {
                Logger.Warn("[EdgesConnectedToNode] Node not contained in this graph... returning empty set");
                return new HashSet<Edge>();
            }
        }

        internal Edge GetEdgeBetween(Node a, Node b)
        {
            var edge = GetEdgeOrNullBetween(a, b);
            if (edge == null)
            {
                throw new ArgumentException("Couldn't find an edge between nodes: " + a + ", and: " + b);
            }
            else
            {
                return edge;
            }
        }

        protected Edge GetEdgeOrNullBetween(Node a, Node b)
        {
            foreach (var edge in EdgesConnectedToNode(a))
            {
                if (edge.GetOtherNode(a).Equals(b))
                {
                    return edge;
                }
            }
            return null;
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

            return EdgesInGraph.Equals(other.EdgesInGraph) && NodesInGraph.Equals(other.NodesInGraph);
        }

        public override int GetHashCode()
        {
            return EdgesInGraph.GetHashCode() * 17 + NodesInGraph.GetHashCode();
        }
    }
}
