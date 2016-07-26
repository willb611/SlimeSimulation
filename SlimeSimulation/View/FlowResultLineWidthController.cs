using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;
using SlimeSimulation.FlowCalculation;

namespace SlimeSimulation.View {
    public class FlowResultLineWidthController : LineWidthController {
        private readonly FlowResult flowResult;
        private double maxmimumFlowFound;

        public FlowResultLineWidthController(FlowResult flowResult) {
            this.flowResult = flowResult;
            this.maxmimumFlowFound = flowResult.GetMaximumFlowOnEdge();
        }

        public override double GetLineWidthForEdge(Edge edge) {
            return flowResult.FlowOnEdge(edge);
        }

        public override double GetMaximumLineWidth() {
            return maxmimumFlowFound;
        }
    }
}
