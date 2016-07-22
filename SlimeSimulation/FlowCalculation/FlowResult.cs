using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.FlowCalculation {
    public class FlowResult {
        private readonly List<Edge> edges;
        private readonly Node source, sink;
        private readonly double maximumErrorExclusive;
        private readonly double flowExponentAmount; // in the hardy cross equation, this is called n
        private readonly double amountOfFlowThroughNetwork;

        internal List<Edge> Edges {
            get {
                return edges;
            }
        }

        internal Node Source {
            get {
                return source;
            }
        }

        internal Node Sink {
            get {
                return sink;
            }
        }

        internal double MaximumErrorExclusive {
            get {
                return maximumErrorExclusive;
            }
        }

        internal double FlowExponentAmount {
            get {
                return flowExponentAmount;
            }
        }

        internal double AmountOfFlowThroughNetwork {
            get {
                return amountOfFlowThroughNetwork;
            }
        }
    }
}
