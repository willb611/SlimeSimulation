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
                UpdateEnumeratorsForDifferentSlime(slimeNetwork);
            }
            return SelectRouteUsingEnumerators();
        }

        private Route SelectRouteUsingEnumerators()
        {
            while (true)
            {
                var subgraph = NextElement(_enumeratorOfAllSubgraphs);
                if (subgraph.ConnectedNodes().Count < 2)
                {
                    continue;
                }
                var enumeratorForNodesInSubgraph = GetEnumeratorForSubgraph(subgraph);
                var source = NextElement(enumeratorForNodesInSubgraph);
                var sink = subgraph.ConnectedNodes().Except(source).PickRandom();
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
            ResetEnumeratorIfNextElementIsNull(enumerator);
            return enumerator;
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

        private void UpdateEnumeratorsForDifferentSlime(SlimeNetwork slimeNetwork)
        {
            Logger.Debug("[UpdateEnumeratorsForDifferentSlime] Entered");
            _graph = slimeNetwork;
            _graphSplitIntoSubgraphs = _bfsSolver.SplitIntoSubgraphs(slimeNetwork);
            _enumeratorOfAllSubgraphs = _graphSplitIntoSubgraphs.Subgraphs.GetEnumerator();
            _enumeratorOfAllSubgraphs.MoveNext();
            _enumeratorForNodesInSubgraphForEachSubgraph = new Dictionary<Subgraph, IEnumerator<Node>>();
            foreach (var subgraph in _graphSplitIntoSubgraphs.Subgraphs)
            {
                _enumeratorForNodesInSubgraphForEachSubgraph[subgraph] = subgraph.ConnectedNodes().GetEnumerator();
            }
        }

        private bool SlimeNetworkHasChanged(SlimeNetwork slimeNetwork)
        {
            return _graph == null || !_graph.Equals(slimeNetwork);
        }
    }
}
