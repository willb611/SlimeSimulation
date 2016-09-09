using System.Collections.Generic;
using NLog;
using SlimeSimulation.Model;

namespace SlimeSimulation.Algorithms.Bfs
{
    public class Subgraph : Graph
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        protected internal Subgraph(ISet<Edge> edges, ISet<Node> nodes) : base(edges, nodes)
        {
        }

        public bool IsNodeConnected(Node node)
        {
            return NodesInGraph.Contains(node);
        }

        public ISet<Node> ConnectedNodes()
        {
            return NodesInGraph;
        }
    }
}
