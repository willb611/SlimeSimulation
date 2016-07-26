using System;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;

namespace SlimeSimulation.View {
    internal class FlowResultLineWidthController : LineWidthController {
        private FlowResult flowResult;
        private readonly double maxLineWidth;

        public FlowResultLineWidthController(FlowResult flowResult) {
            this.flowResult = flowResult;
            maxLineWidth = flowResult.GetMaximumFlowOnEdge();
        }

        public override double GetLineWidthForEdge(Edge edge) {
            return flowResult.FlowOnEdge(edge);
        }

        public override double GetMaximumLineWidth() {
            return maxLineWidth;
        }
    }
}
