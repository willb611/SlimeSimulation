using System.Collections.Generic;
using NLog;

namespace SlimeSimulation.Model.Bfs
{
    internal class BfsResult
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<Node, bool> _connected;

        protected internal BfsResult(Dictionary<Node, bool> connected)
        {
            _connected = connected;
        }

        public bool Connected(Node node)
        {
            return _connected[node];
        }

        public ISet<Node> ConnectedNodes()
        {
            HashSet<Node> nodes = new HashSet<Node>();
            foreach (Node node in _connected.Keys)
            {
                if (Connected(node))
                {
                    nodes.Add(node);
                }
            }
            Logger.Debug("[ConnectedNodes] Out of {0} returning {1}", _connected.Count, nodes.Count);
            return nodes;
        }
    }
}
