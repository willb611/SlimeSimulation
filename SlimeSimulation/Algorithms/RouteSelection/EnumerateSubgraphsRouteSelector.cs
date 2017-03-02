using System;
using System.Collections.Generic;
using NLog;
using SlimeSimulation.Algorithms.Bfs;
using SlimeSimulation.Model;
using SlimeSimulation.StdLibHelpers;

namespace SlimeSimulation.Algorithms.RouteSelection
{
    public class EnumerateSubgraphsRouteSelector : IRouteSelector
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly BfsSolver _bfsSolver = new BfsSolver();
        
        private Graph _graph;
        private GraphSplitIntoSubgraphs _graphSplitIntoSubgraphs;

        private IEnumerator<Subgraph> _enumeratorOfAllSubgraphs;
        private Dictionary<Subgraph, IEnumerator<FoodSourceNode>> _enumeratorForNodesInSubgraphForEachSubgraph;

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
            for (int i = 0; i < 1000; i++)
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
                var sink = GetFoodSourcesInSubGraph(subgraph).Except(source).PickRandom();
                return new Route(source, sink);
            }
            throw new ApplicationException("Unable to find a valid route");
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

        private IEnumerator<FoodSourceNode> GetEnumeratorForSubgraph(Subgraph subgraph)
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
            DisposeOfAnyExistingIterators();
            Logger.Debug("[UpdateEnumeratorsForDifferentSlime] Entered");
            _graph = slimeNetwork;
            _graphSplitIntoSubgraphs = _bfsSolver.SplitIntoSubgraphs(slimeNetwork);
            _enumeratorOfAllSubgraphs = _graphSplitIntoSubgraphs.Subgraphs.GetEnumerator();
            _enumeratorOfAllSubgraphs.MoveNext();
            _enumeratorForNodesInSubgraphForEachSubgraph = new Dictionary<Subgraph, IEnumerator<FoodSourceNode>>();
            foreach (var subgraph in _graphSplitIntoSubgraphs.Subgraphs)
            {
                _enumeratorForNodesInSubgraphForEachSubgraph[subgraph] = GetFoodSourcesInSubGraph(subgraph).GetEnumerator();
            }
        }

        private void DisposeOfAnyExistingIterators()
        {
            if (_enumeratorOfAllSubgraphs != null)
            {
                _enumeratorOfAllSubgraphs?.Dispose();
                _enumeratorOfAllSubgraphs = null;
            }
            if (_enumeratorForNodesInSubgraphForEachSubgraph != null)
            {
                foreach (var element in _enumeratorForNodesInSubgraphForEachSubgraph.Values)
                {
                    element?.Dispose();
                }
                _enumeratorForNodesInSubgraphForEachSubgraph = null;
            }
        }

        private bool SlimeNetworkHasChanged(SlimeNetwork slimeNetwork)
        {
            return _graph == null || !_graph.Equals(slimeNetwork);
        }

        private ISet<FoodSourceNode> GetFoodSourcesInSubGraph(Subgraph subgraph)
        {
            var nodesInSubgraph = new HashSet<Node>(subgraph.NodesInGraph);
            var foodSourcesInSubgraph = new HashSet<FoodSourceNode>();
            foreach (var node in nodesInSubgraph)
            {
                if (node.IsFoodSource)
                {
                    foodSourcesInSubgraph.Add(node as FoodSourceNode);
                }
            }
            return foodSourcesInSubgraph;
        }
    }
}
