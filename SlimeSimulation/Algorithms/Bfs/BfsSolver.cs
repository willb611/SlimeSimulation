using System.Collections.Generic;
using System.Linq;
using NLog;
using SlimeSimulation.Model;
using SlimeSimulation.StdLibHelpers;

namespace SlimeSimulation.Algorithms.Bfs
{
    internal class BfsSolver
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Subgraph From(Node source, Graph graph)
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
            return new Subgraph(connected);
        }

        public GraphSplitIntoSubgraphs SplitIntoSubgraphs(Graph graph)
        {
            List<Subgraph> subgraphs = new List<Subgraph>();
            var unconnected = graph.NodesInGraph;
            while (unconnected.Any())
            {
                var nextSource = unconnected.PickRandom();
                var connectedToSource = From(nextSource, graph);
                subgraphs.Add(connectedToSource);
                unconnected.ExceptWith(connectedToSource.ConnectedNodes());
            }
            return new GraphSplitIntoSubgraphs(subgraphs);
        }
    }
}
