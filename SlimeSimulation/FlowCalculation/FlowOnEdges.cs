using System;
using System.Collections.Generic;
using NLog;
using SlimeSimulation.Model;

namespace SlimeSimulation.FlowCalculation
{
    public class FlowOnEdges
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const double SmallAmountToPreventNodesFromGettingDisconnected = 0.0000001;
        private readonly Dictionary<Edge, double> _flowOnEdgeMapping;

        public FlowOnEdges(FlowOnEdges flowOnEdges)
        {
            _flowOnEdgeMapping = new Dictionary<Edge, double>(flowOnEdges._flowOnEdgeMapping);
        }

        internal FlowOnEdges(ICollection<Edge> edges)
        {
            _flowOnEdgeMapping = new Dictionary<Edge, double>();
            foreach (var edge in edges)
            {
                _flowOnEdgeMapping.Add(edge, 0);
            }
        }


        internal double GetMaximumFlowOnAnyEdge()
        {
            double max = 0;
            foreach (double value in _flowOnEdgeMapping.Values)
            {
                max = Math.Max(max, Math.Abs(value));
            }
            return max;
        }

        internal double GetFlowOnEdge(Edge edge)
        {
            return _flowOnEdgeMapping[edge];
        }

        internal void IncreaseFlowOnEdgeBy(Edge edge, double amount)
        {
            double current = GetFlowOnEdge(edge);
            _flowOnEdgeMapping[edge] = current + amount;
            Logger.Trace("[IncreaseFlowOnEdgeBy] For edge: {0}, flow is now: {1}", edge, GetFlowOnEdge(edge));
        }
    }
}
