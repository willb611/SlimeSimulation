using System.Collections.Generic;
using NLog;
using SlimeSimulation.Model;

namespace SlimeSimulation.Algorithms.Bfs
{
    public class Subgraph
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<Node, bool> _connected;
        private readonly ISet<Node> _connectedNodes;

        protected internal Subgraph(Dictionary<Node, bool> connected)
        {
            _connected = connected;
            _connectedNodes = GetConnectedNodes();
        }

        private ISet<Node> GetConnectedNodes()
        {
            HashSet<Node> nodes = new HashSet<Node>();
            foreach (Node node in _connected.Keys)
            {
                if (IsNodeConnected(node))
                {
                    nodes.Add(node);
                }
            }
            Logger.Debug("[ConnectedNodes] Out of {0} returning {1}", _connected.Count, nodes.Count);
            return nodes;
        }

        public bool IsNodeConnected(Node node)
        {
            return _connected[node];
        }

        public ISet<Node> ConnectedNodes()
        {
            return _connectedNodes;
        }
    }
}
