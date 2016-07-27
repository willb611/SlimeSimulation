using SlimeSimulation.Model;
using System.Collections.Generic;
using NLog;

namespace SlimeSimulation.FlowCalculation {
    public class IntermediateFlowResult {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        internal double MaxErrorFoundOnCalculatingHeadLoss;
        private readonly ISet<LoopWithDirectionOfFlow> loops;
        private FlowOnEdges flowOnEdges;

        public IntermediateFlowResult(double maxError, ISet<LoopWithDirectionOfFlow> loops, FlowOnEdges flowFound) {
            MaxErrorFoundOnCalculatingHeadLoss = maxError;
            this.loops = loops;
            this.flowOnEdges = flowFound;
            logger.Debug("Given number of loops: " + loops.Count +", and maxError: " + maxError);
        }

        public ISet<LoopWithDirectionOfFlow> Loops {
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
