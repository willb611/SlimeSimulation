using System.Collections.Generic;
using System.Linq;
using NLog;

namespace SlimeSimulation.Model.Bfs
{
    internal class BfsSolver
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public BfsResult From(Node source, Graph graph)
        {
            Logger.Debug($"[From] Runing from source {source}");
            Dictionary<Node, bool> connected = new Dictionary<Node, bool>();
            foreach (var node in graph.NodesInGraph)
            {
                connected[node] = false;
            }

            Queue<Node> nodesToVisit = new Queue<Node>();
            nodesToVisit.Enqueue(source);
            connected[source] = true;

            while (nodesToVisit.Any())
            {
                var current = nodesToVisit.Dequeue();
                foreach (var node in graph.Neighbours(current))
                {
                    if (!connected[node])
                    {
                        connected[node] = true;
                        nodesToVisit.Enqueue(node);
                    }
                }
            }
            return new BfsResult(connected);
        }
    }
}
