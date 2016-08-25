using System.Collections.Generic;

namespace SlimeSimulation.Model.Bfs
{
    internal class BfsResult
    {
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
            return nodes;
        }
    }
}
