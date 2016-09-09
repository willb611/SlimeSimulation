using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using SlimeSimulation.Algorithms.Bfs;
using SlimeSimulation.Model;
using SlimeSimulation.StdLibHelpers;

namespace SlimeSimulation.Algorithms.RouteSelection
{
    class EnumerateSubgraphsRouteSelector : IRouteSelector
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly BfsSolver _bfsSolver = new BfsSolver();
        
        private Graph _graph;
        private GraphSplitIntoSubgraphs _graphSplitIntoSubgraphs;

        private IEnumerator<Subgraph> _enumeratorOfAllSubgraphs;
        private Dictionary<Subgraph, IEnumerator<Node>> _enumeratorForNodesInSubgraphForEachSubgraph;

        public Route SelectRoute(SlimeNetwork slimeNetwork)
        {
            if (SlimeNetworkHasChanged(slimeNetwork))
            {
                UpdateEnumeratorsForGivenSlime(slimeNetwork);
            }
            return SelectRouteUsingEnumerators();
        }

        private Route SelectRouteUsingEnumerators()
        {
            for (int i = 0; ; i++)
            {
                if (i > _graph.NodesInGraph.Count)
                {
                    Logger.Warn("[SelectRouteUsingEnumerators] So far {0} attempts to find a route have failed, the graph only has {1} nodes. Graph seems to be disconnected ?",
                        i, _graph.NodesInGraph.Count);
                }
                var subgraph = NextElement(_enumeratorOfAllSubgraphs);
                if (subgraph.NodesInGraph.Count < 2)
                {
                    continue;
                }
                var enumeratorForNodesInSubgraph = GetEnumeratorForSubgraph(subgraph);
                var source = NextElement(enumeratorForNodesInSubgraph);
                var sink = subgraph.NodesInGraph.Except(source).PickRandom();
                return new Route(source, sink);
            }
        }

        private T NextElement<T>(IEnumerator<T> enumerator)
        {
            if (enumerator == null)
            {
                throw new NullReferenceException(nameof(enumerator));
            }
            ResetEnumeratorIfNextElementIsNull(enumerator);
            var element = enumerator.Current;
            enumerator.MoveNext();
            return element;
        }

        private IEnumerator<Node> GetEnumeratorForSubgraph(Subgraph subgraph)
        {
            if (!_enumeratorForNodesInSubgraphForEachSubgraph.ContainsKey(subgraph))
            {
                throw new ArgumentException(nameof(subgraph));
            }
            var enumerator = _enumeratorForNodesInSubgraphForEachSubgraph[subgraph];
            return ResetEnumeratorIfNextElementIsNull(enumerator);
        }

        private IEnumerator<T> ResetEnumeratorIfNextElementIsNull<T>(IEnumerator<T> enumerator)
        {
            if (enumerator == null)
            {
                throw new NullReferenceException(nameof(enumerator));
            }
            if (enumerator.Current == null) 
            {
                enumerator.Reset();
                enumerator.MoveNext();
            }
            return enumerator;
        }

        private void UpdateEnumeratorsForGivenSlime(SlimeNetwork slimeNetwork)
        {
            Logger.Debug("[UpdateEnumeratorsForDifferentSlime] Entered");
            _graph = slimeNetwork;
            _graphSplitIntoSubgraphs = _bfsSolver.SplitIntoSubgraphs(slimeNetwork);
            _enumeratorOfAllSubgraphs = _graphSplitIntoSubgraphs.Subgraphs.GetEnumerator();
            _enumeratorOfAllSubgraphs.MoveNext();
            _enumeratorForNodesInSubgraphForEachSubgraph = new Dictionary<Subgraph, IEnumerator<Node>>();
            foreach (var subgraph in _graphSplitIntoSubgraphs.Subgraphs)
            {
                _enumeratorForNodesInSubgraphForEachSubgraph[subgraph] = subgraph.NodesInGraph.GetEnumerator();
            }
        }

        private bool SlimeNetworkHasChanged(SlimeNetwork slimeNetwork)
        {
            return _graph == null || !_graph.Equals(slimeNetwork);
        }
    }
}
