using System.Collections.Generic;
using SlimeSimulation.Model;
using NLog;
using System;

namespace SlimeSimulation.FlowCalculation
{
    public class FlowOnEdges
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const double SmallAmountToPreventNodesFromGettingDisconnected = 0.0000001;
        private readonly Dictionary<SlimeEdge, double> _flowOnEdgeMapping;

        public FlowOnEdges(FlowOnEdges flowOnEdges)
        {
            _flowOnEdgeMapping = new Dictionary<SlimeEdge, double>(flowOnEdges._flowOnEdgeMapping);
        }

        internal FlowOnEdges(ICollection<SlimeEdge> edges)
        {
            _flowOnEdgeMapping = new Dictionary<SlimeEdge, double>();
            foreach (SlimeEdge edge in edges)
            {
                _flowOnEdgeMapping.Add(edge, SmallAmountToPreventNodesFromGettingDisconnected);
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

        internal double GetFlowOnEdge(SlimeEdge edge)
        {
            return _flowOnEdgeMapping[edge];
        }

        internal void IncreaseFlowOnEdgeBy(SlimeEdge edge, double amount)
        {
            double current = GetFlowOnEdge(edge);
            _flowOnEdgeMapping[edge] = current + amount;
            Logger.Trace("[IncreaseFlowOnEdgeBy] For edge: {0}, flow is now: {1}", edge, GetFlowOnEdge(edge));
        }
    }
}
