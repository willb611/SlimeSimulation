using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;

namespace SlimeSimulation.View {
    public abstract class LineWeightController {
        public abstract double GetLineWeightForEdge(Edge edge);
        public abstract double GetMaximumLineWeight();
    }
    internal class FlowResultLineWidthController : LineWeightController {
        private FlowResult flowResult;
        private readonly double maxLineWidth;

        public FlowResultLineWidthController(FlowResult flowResult) {
            this.flowResult = flowResult;
            maxLineWidth = flowResult.GetMaximumFlowOnEdge();
        }

        public override double GetLineWeightForEdge(Edge edge) {
            return flowResult.FlowOnEdge(edge);
        }

        public override double GetMaximumLineWeight() {
            return maxLineWidth;
        }
    }
    internal class ConnectivityLineWidthController : LineWeightController {
        private List<Edge> edges;
        private readonly double max;
        public ConnectivityLineWidthController(List<Edge> edges) {
            this.edges = edges;
            var max = 0.0;
            foreach (Edge edge in edges) {
                max = Math.Max(edge.Connectivity, max);
            }
            this.max = max;
        }

        public override double GetLineWeightForEdge(Edge edge) {
            return edge.Connectivity;
        }

        public override double GetMaximumLineWeight() {
            return max;
        }
    }
}
