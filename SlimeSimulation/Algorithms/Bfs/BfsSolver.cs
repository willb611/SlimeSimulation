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
            ISet<Node> nodesInSubgraph = new HashSet<Node>();
            ISet<Edge> edgesInSubgraph = new HashSet<Edge>();

            Queue<Node> nodesToVisit = new Queue<Node>();
            nodesToVisit.Enqueue(source);
            nodesInSubgraph.Add(source);

            while (nodesToVisit.Any())
            {
                var current = nodesToVisit.Dequeue();
                foreach (var edge in graph.EdgesConnectedToNode(current))
                {
                    edgesInSubgraph.Add(edge);
                    if (!nodesInSubgraph.Contains(edge.A))
                    {
                        nodesToVisit.Enqueue(edge.A);
                        nodesInSubgraph.Add(edge.A);
                    }
                    if (!nodesInSubgraph.Contains(edge.B))
                    {
                        nodesToVisit.Enqueue(edge.B);
                        nodesInSubgraph.Add(edge.B);
                    }
                }
            }
            return new Subgraph(edgesInSubgraph, nodesInSubgraph);
        }

        public GraphSplitIntoSubgraphs SplitIntoSubgraphs(Graph graph)
        {
            List<Subgraph> subgraphs = new List<Subgraph>();
            var unconnected = new HashSet<Node>(graph.NodesInGraph);
            while (unconnected.Any())
            {
                var node = unconnected.PickRandom();
                var subgraphConnectedToNode = From(node, graph);
                subgraphs.Add(subgraphConnectedToNode);
                unconnected.ExceptWith(subgraphConnectedToNode.NodesInGraph);
            }
            return new GraphSplitIntoSubgraphs(subgraphs);
        }
    }
}
