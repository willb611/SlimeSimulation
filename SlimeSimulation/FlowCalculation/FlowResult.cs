using SlimeSimulation.Model;
using System.Collections.Generic;
using NLog;

namespace SlimeSimulation.FlowCalculation {
    public class FlowResult {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly List<Edge> edges;
        private readonly Node source, sink;
        private readonly int flowAmount;
        private readonly FlowOnEdges flowOnEdges;

        public FlowResult(List<Edge> edges, Node source, Node sink, int flowAmount,
                FlowOnEdges flowOnEdges) {
            this.source = source;
            this.sink = sink;
            this.flowAmount = flowAmount;
            this.edges = edges;
            this.flowOnEdges = flowOnEdges;
            logger.Debug("[constructor] Creating flowResult for flow: " + flowAmount + ", and numer of edges: " + edges.Count);
        }

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

        public int FlowAmount {
            get {
                return flowAmount;
            }
        }

        public double FlowOnEdge(Edge edge) {
            return flowOnEdges.GetFlowOnEdge(edge);
        }
    }
}
