using System;
using System.Collections.Generic;
using NLog;
using SlimeSimulation.Model;

namespace SlimeSimulation.Algorithms.FlowCalculation
{
    public class FlowOnEdges
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const double SmallAmountToStopDisconnection = 0.0000001;
        public static bool ShouldAllowDisconnection { get; set; }

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
                if (ShouldAllowDisconnection)
                {
                    _flowOnEdgeMapping.Add(edge, 0);
                }
                else
                {
                    _flowOnEdgeMapping.Add(edge, SmallAmountToStopDisconnection);
                }
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
            if (_flowOnEdgeMapping.ContainsKey(edge))
            {
                return _flowOnEdgeMapping[edge];
            }
            else
            {
                Logger.Warn("[GetFlowOnEdge] Edge {0} wasn't found, returning 0", edge);
                return 0;
            }
        }

        internal void IncreaseFlowOnEdgeBy(Edge edge, double amount)
        {
            double current = GetFlowOnEdge(edge);
            _flowOnEdgeMapping[edge] = current + amount;
            Logger.Trace("[IncreaseFlowOnEdgeBy] For edge: {0}, flow is now: {1}", edge, GetFlowOnEdge(edge));
        }
    }
}
