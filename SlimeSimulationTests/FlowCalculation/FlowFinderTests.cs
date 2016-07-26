using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.FlowCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;
using NLog;

namespace SlimeSimulation.FlowCalculation.Tests {

    [TestClass()]
    public class FlowFinderTests {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [TestMethod()]
        public void GetLoopsWithDirectionForFlow_WhenSimpleLoop_ShouldWork() {
            Node source = new Node(1, 1, 1);
            Node a = new Node(2, 1, 2);
            Node b = new Node(3, 2, 1);
            Node sink = new Node(4, 2, 2);
            List<Node> nodes = new List<Node>() { source, a, b, sink };

            Edge srca = new Edge(source, a, 2);
            Edge srcb = new Edge(source, b, 2);
            Edge asink = new Edge(a, sink, 2);
            Edge bsink = new Edge(b, sink, 2);
            List<Edge> edges = new List<Edge>() { srca, srcb, asink, bsink };

            List<Loop> loops = new List<Loop>() { new Loop(nodes, edges) };
            List<Edge> asideEdge = new List<Edge>() { srca, asink };
            List<Edge> bsideEdge = new List<Edge>() { srcb, bsink };
            /*
             * b      -  sink
             * |        |
             * source -  a
            */
            FlowFinder flowFinder = new FlowFinder();
            Graph graph = new Graph(edges);
            List<LoopWithDirectionOfFlow> loopsWithDirection = flowFinder.GetLoopsWithDirectionForFlow(loops, source, sink, graph);

            LoopWithDirectionOfFlow actualLoopWithDirection = loopsWithDirection.First();
            if (ListEquals(actualLoopWithDirection.Clockwise, asideEdge)) {
                Assert.IsTrue(ListEquals(bsideEdge, actualLoopWithDirection.AntiClockwise));
            } else if (ListEquals(actualLoopWithDirection.AntiClockwise, asideEdge)) {
                Assert.IsTrue(ListEquals(bsideEdge, actualLoopWithDirection.Clockwise));
            } else {
                logger.Error("Aside not matched. Aside: ");
                LogEdges(asideEdge);
                logger.Error("Clockwise: ");
                LogEdges(actualLoopWithDirection.Clockwise);
                logger.Error("Anticlockwise: ");
                LogEdges(actualLoopWithDirection.AntiClockwise);
                Assert.Fail("Calculated loop with direction was wrong: " + actualLoopWithDirection);
            }
        }

        private bool ListEquals(List<Edge> a, List<Edge> b) {
            bool ret = true;
            foreach (Edge edge in a) {
                if (!b.Contains(edge)) {
                    ret = false;
                }
            }
            if (b.Count != a.Count) {
                ret = false;
            }
            return ret;
        }

        private void LogEdges(IEnumerable<Edge> edges) {
            foreach (Edge edge in edges) {
                logger.Error(edge);
            }
        }

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

            var flowFinder = new FlowFinder();
            flowFinder.SplitFlowIntoNeighbours(src, edges, ref mapping, ref flowOnEdges);
            Assert.AreEqual(flowAmount / 2, mapping[a]);
            Assert.AreEqual(flowAmount / 2, mapping[b]);
        }

        [TestMethod()]
        public void EstimateFlowForEdgesTest() {
            double ACCEPTED_ERROR = 0.01;
            var calculator = new FlowCalculator();
            double flowAmount = 2;
            List<Edge> edges = new List<Edge>();
            List<Node> nodes = new List<Node>();
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
            FlowFinder flowFinder = new FlowFinder();
            FlowOnEdges flowOnEdges = flowFinder.EstimateFlowForEdges(new Graph(nodes, edges), source, sink, (int) flowAmount);

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
