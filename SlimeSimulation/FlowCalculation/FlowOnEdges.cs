using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;

namespace SlimeSimulation.FlowCalculation {
    public class FlowOnEdges {
        private readonly double maxErrorFoundOnCalculatingHeadLoss;
        private readonly Dictionary<Edge, double> flowOnEdgeMapping;
        
        public FlowOnEdges(FlowOnEdges flowOnEdges) {
            maxErrorFoundOnCalculatingHeadLoss = 0;
            flowOnEdgeMapping = new Dictionary<Edge, double>(flowOnEdges.flowOnEdgeMapping);
        }

        internal FlowOnEdges(List<Edge> edges) {
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

        internal double GetFlowOnEdge(Edge edge) {
            double result;
            bool found = flowOnEdgeMapping.TryGetValue(edge, out result);
            if (!found) {
                throw new ArgumentException("Given edge not contained in this flowResult");
            }
            return result;
        }

        internal void IncreaseFlowOnEdgeBy(Edge edge, double amount) {
            double current = GetFlowOnEdge(edge);
            flowOnEdgeMapping.Remove(edge);
            flowOnEdgeMapping.Add(edge, current + amount);
        }
    }
}
