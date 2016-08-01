using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using SlimeSimulation.Model;
using NLog;

namespace SlimeSimulation.FlowCalculation.Tests {

    [TestClass()]
    public class InitialFlowEstimatorTests {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [TestMethod()]
        public void SplitFlowIntoNeighbours_ShouldSplitEqually() {
            Node src = new Node(0, 1, 7);
            Node a = new Node(1, 1, 1);
            Node b = new Node(1, 2, 2);
            Edge srca = new Edge(src, a, 1);
            Edge srcb = new Edge(src, b, 1);
            List<Edge> edges = new List<Edge>() { srca, srcb };

            double flowAmount = 8;
            Dictionary<Node, double> mapping = new Dictionary<Node, double>();
            FlowOnEdges flowOnEdges = new FlowOnEdges(edges);
            mapping[src] = flowAmount;

            var initialFlowEstimator = new InitialFlowEstimator();
            initialFlowEstimator.SplitFlowIntoNeighbours(src, edges, ref mapping, ref flowOnEdges);
            Assert.AreEqual(flowAmount / 2, mapping[a]);
            Assert.AreEqual(flowAmount / 2, mapping[b]);
        }

        [TestMethod()]
        public void EstimateFlowForEdgesTest() {
            double ACCEPTED_ERROR = 0.01;
            var calculator = new HardyCrossFlowCalculator();
            double flowAmount = 2;
            HashSet<Edge> edges = new HashSet<Edge>();
            HashSet<Node> nodes = new HashSet<Node>();
            Node source = new Node(1, 1, 1);
            Node a = new Node(2, 1, 2);
            Edge srca = new Edge(source, a, 2);
            Node b = new Node(3, 2, 1);
            Edge srcb = new Edge(source, b, 2);
            Node sink = new Node(4, 2, 2);
            Edge asink = new Edge(a, sink, 2);
            Edge bsink = new Edge(b, sink, 2);
            edges.Add(srca);
            edges.Add(srcb);
            edges.Add(bsink);
            edges.Add(asink);
            nodes.Add(a);
            nodes.Add(b);
            nodes.Add(source);
            nodes.Add(sink);
            /*
             * b      -  sink
             * |        |
             * source -  a
             */
            var initialFlowEstimator = new InitialFlowEstimator();
            FlowOnEdges flowOnEdges = initialFlowEstimator.EstimateFlowForEdges(new Graph(edges, nodes), source, sink, (int) flowAmount);

            double flowASrc = flowOnEdges.GetFlowOnEdge(srca);
            double flowBSrc = flowOnEdges.GetFlowOnEdge(srcb);
            Assert.IsTrue(flowAmount - (flowASrc + flowBSrc) < ACCEPTED_ERROR,
                "flow should split equally, expected: " + flowAmount + ", calculated : " + (flowASrc + flowBSrc));

            double flowASink = flowOnEdges.GetFlowOnEdge(asink);
            double flowBSink = flowOnEdges.GetFlowOnEdge(bsink);
            Assert.IsTrue(flowAmount - (flowASink + flowBSink) < ACCEPTED_ERROR,
                "flow should split equally, expected: " + flowAmount + ", calculated : " + (flowASink + flowBSink));
        }
    }
}
