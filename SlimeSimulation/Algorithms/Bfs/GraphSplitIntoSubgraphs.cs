using System;
using System.Collections.Generic;
using NLog;
using SlimeSimulation.Model;

namespace SlimeSimulation.Algorithms.Bfs
{
    public class GraphSplitIntoSubgraphs
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly List<Subgraph> _subgraphs;
        private readonly Dictionary<Node, Subgraph> _nodeToConnectionResults;

        public List<Subgraph> Subgraphs => _subgraphs;

        internal GraphSplitIntoSubgraphs(List<Subgraph> subgraphs)
        {
            _subgraphs = subgraphs;
            _nodeToConnectionResults = BuildNodeToSubgraphMapping(subgraphs);
        }

        private Dictionary<Node, Subgraph> BuildNodeToSubgraphMapping(List<Subgraph> subgraphs)
        {
            Dictionary<Node, Subgraph> result = new Dictionary<Node, Subgraph>();
            foreach (var subgraph in subgraphs)
            {
                foreach (var node in subgraph.NodesInGraph)
                {
                    if (result.ContainsKey(node))
                    {
                        Logger.Warn("[BuildNodeToConnectionResultMapping] Node {0} was found in multiple subgraphs.", node);
                    }
                    result[node] = subgraph;
                }
            }
            return result;
        }

        public Subgraph SubgraphContainingNode(Node source)
        {
            if (_nodeToConnectionResults.ContainsKey(source))
            {
                return _nodeToConnectionResults[source];
            }
            else
            {
                throw new ArgumentException(nameof(source));
            }
        }

    }
}
