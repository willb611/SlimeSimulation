using System.Collections.Generic;
using SlimeSimulation.Model;
using NLog;
using System;

namespace SlimeSimulation.FlowCalculation {
    public class FlowOnEdges {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly double maxErrorFoundOnCalculatingHeadLoss;
        private readonly Dictionary<Edge, double> flowOnEdgeMapping;
        
        public FlowOnEdges(FlowOnEdges flowOnEdges) {
            maxErrorFoundOnCalculatingHeadLoss = 0;
            flowOnEdgeMapping = new Dictionary<Edge, double>(flowOnEdges.flowOnEdgeMapping);
        }

        internal FlowOnEdges(ICollection<Edge> edges) {
            flowOnEdgeMapping = new Dictionary<Edge, double>();
            foreach (Edge edge in edges) {
                flowOnEdgeMapping.Add(edge, 0);
            }
        }

        public double MaxErrorFoundOnCalculatingHeadLoss {
            get {
                return maxErrorFoundOnCalculatingHeadLoss;
            }
        }

        internal double GetMaximumFlowOnAnyEdge() {
            double max = 0;
            foreach (double value in flowOnEdgeMapping.Values) {
                max = Math.Max(max, Math.Abs(value));
            }
            return max;
        }

        internal double GetFlowOnEdge(Edge edge) {
            return flowOnEdgeMapping[edge];
        }

        internal void IncreaseFlowOnEdgeBy(Edge edge, double amount) {
            double current = GetFlowOnEdge(edge);
            flowOnEdgeMapping[edge] = current + amount;
            logger.Debug("[IncreaseFlowOnEdgeBy] For edge: " + edge + ", flow is now: " + GetFlowOnEdge(edge));
        }

        internal void LogFlowOnEdges() {
            logger.Debug("[LogFlowOnEdges] Begin");
            foreach (Edge edge in flowOnEdgeMapping.Keys) {
                logger.Debug("Edge " + edge + ", now has flow: " + GetFlowOnEdge(edge));
            }
            logger.Debug("[LogFlowOnEdges] Finish");
        }
    }
}
