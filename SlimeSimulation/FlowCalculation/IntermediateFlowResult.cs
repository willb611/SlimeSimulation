using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.FlowCalculation {
    public class IntermediateFlowResult {
        internal double MaxErrorFoundOnCalculatingHeadLoss;
        private readonly List<LoopWithDirectionOfFlow> loops;
        private FlowOnEdges flowOnEdges;

        public IntermediateFlowResult(double maxError, List<LoopWithDirectionOfFlow> loops, FlowOnEdges flowFound) {
            MaxErrorFoundOnCalculatingHeadLoss = maxError;
            this.loops = loops;
            this.flowOnEdges = flowFound;
        }

        public List<LoopWithDirectionOfFlow> Loops {
            get {
                return loops;
            }
        }

        public FlowOnEdges FlowOnEdges {
            get {
                return flowOnEdges;
            }
        }

        internal void IncreaseFlowOnEdgeBy(Edge edge, double deltaToBeApplied) {
            flowOnEdges.IncreaseFlowOnEdgeBy(edge, deltaToBeApplied);
        }
    }
}
