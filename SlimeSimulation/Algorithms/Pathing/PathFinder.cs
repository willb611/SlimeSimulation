using System;
using SlimeSimulation.Model;
using SlimeSimulation.Algorithms.Bfs;
using System.Collections.Generic;
using NLog;

namespace SlimeSimulation.Algorithms.Pathing
{
    public class PathFinder
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private BfsSolver bfsSolver = new BfsSolver();

        public Path FindPath(Graph graph, Route route)
        {
            CheckRoute(route);
            Subgraph connected = bfsSolver.From(route.Source, graph);
            if (!connected.ContainsNode(route.Sink))
            {
                Logger.Warn("Route {0} not possible as sink not connected to source.", route);
                return null;
            }
            List<Node> nodes = new List<Node>(connected.NodesInGraph);
            int n = connected.NodesInGraph.Count;

            int[] distances = InitDistances(n);
            distances[nodes.IndexOf(route.Source)] = 0;

            Node[] prev = new Node[n];
            Queue queue = new Queue();
            for (int i = 0; i < n; i++)
            {
                queue.Add(distances[i], i);   
            }
            while (queue.Any())
            {
                int minDistance = queue.MinKey();
                int nodeIndex = queue.Get(minDistance);
                Node u = nodes[nodeIndex];
                if (u == route.Sink) // Found shortest path to sink from source
                {
                    break;
                }
                queue.Remove(minDistance, nodeIndex);

                foreach (var edge in graph.EdgesConnectedToNode(u))
                {
                    int tmp = distances[nodeIndex] + EdgeDistance(edge);
                    int v = nodes.IndexOf(edge.GetOtherNode(u));
                    if (tmp < distances[v])
                    {
                        distances[v] = tmp;
                        prev[v] = u;
                    }
                }
            }
            if (Logger.IsTraceEnabled) {
                for (int i = 0; i < prev.Length; i++)
                {
                    Logger.Trace("[FindPath] i: {0}, value: {1}", i, prev[i]);
                }
            }
            // Build result
            List<Node> path = buildPath(nodes, prev, route);
            return new Path(path, route);
        }

        private void CheckRoute(Route route)
        {
            if (route.Source.Equals(route.Sink))
            {
                throw new ArgumentException("Source matches sink: " + route);
            }
        }

        private List<Node> buildPath(List<Node> ordering, Node[] prev, Route route)
        {
            List<Node> path = new List<Node>();
            Node u = route.Sink;
            while (prev[ordering.IndexOf(u)] != null)
            {
                path.Add(u);
                u = prev[ordering.IndexOf(u)];
            }
            path.Add(u);
            path.Reverse();
            return path;
        }

        private int EdgeDistance(Edge edge)
        {
            return 1;
        }

        private int[] InitDistances(int n)
        {
            int[] distances = new int[n];
            for (int i = 0; i < n; i++)
            {
                distances[i] = Int32.MaxValue;
            }
            return distances;
        }
    }
}
