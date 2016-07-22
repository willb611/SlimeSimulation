using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.FlowCalculation {
    public class FlowCalculator {
        private readonly static int DEFAULT_FLOW_EXPONENT = 2;
        private readonly static double DEFAULT_MAX_ERROR = 0.01;
        private readonly int flowExponent;
        private readonly double maximumErrorExclusive;

        public FlowCalculator() : this(DEFAULT_FLOW_EXPONENT, DEFAULT_MAX_ERROR) {
        }

        public FlowCalculator(int flowExponent, double maximumErrorExclusiveAllowedDuringCalculation) {
            this.flowExponent = flowExponent;
            this.maximumErrorExclusive = maximumErrorExclusiveAllowedDuringCalculation;
        }

        public int FlowExponent {
            get {
                return flowExponent;
            }
        }

        public double MaximumErrorExclusive {
            get {
                return maximumErrorExclusive;
            }
        }

        public FlowResult calculateFlow(List<Edge> edges, List<Loop> loops,
                Node source, Node sink) {
            if (edges == null) {
                throw new ArgumentNullException("Given null edges");
            } else if (loops == null) {
                throw new ArgumentNullException("Given null loops");
            }
            throw new NotImplementedException("TODO");
        }

        internal FlowOnEdges getInitialFlow(List<Edge> edges, Node source, Node sink) {
            throw new NotImplementedException("TODO");
        }
        
    }
}
