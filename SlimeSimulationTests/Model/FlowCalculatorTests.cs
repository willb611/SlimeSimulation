using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model.Tests {
    [TestClass()]
    public class FlowCalculatorTests {

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void calculateFlow_WhenNullEdges_ShouldThrowException() {
            var calculator = new FlowCalculator();
            calculator.calculateFlow(null, new List<Loop>(), new Node(1, 1, 1), new Node(2, 2, 2));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void calculateFlow_WhenNullLoops_ShouldThrowException() {
            var calculator = new FlowCalculator();
            calculator.calculateFlow(new List<Edge>(), null, new Node(1, 1, 1), new Node(2, 2, 2));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void calculateFlow_WhenNullSource_ShouldThrowException() {
            var calculator = new FlowCalculator();
            calculator.calculateFlow(new List<Edge>(), new List<Loop>(), null, new Node(2, 2, 2));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void calculateFlow_WhenNullSink_ShouldThrowException() {
            var calculator = new FlowCalculator();
            calculator.calculateFlow(new List<Edge>(), new List<Loop>(), new Node(1, 1, 1), null);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void calculateFlow_WhenSourceEqualsSink_ShouldThrowException() {
            var calculator = new FlowCalculator();
            var node = new Node(1, 1, 1);
            calculator.calculateFlow(new List<Edge>(), new List<Loop>(), node, node);
        }

        [TestMethod()]
        public void getInitialFlow_TotalFlowLeavingNodeAddsToFlowAmount() {
            double ACCEPTED_ERROR = 0.01;
            var calculator = new FlowCalculator();
            double flowAmount = 2;
            List<Edge> edges = new List<Edge>();
            Node source = new Node(1, 1, 1);
            Node a = new Node(2, 1, 2);
            Edge srca = new Edge(source, a, 2);
            edges.Add(srca);
            Node b = new Node(3, 2, 1);
            Edge srcb = new Edge(source, b, 2);
            edges.Add(srcb);
            Node sink = new Node(4, 2, 2);
            Edge asink = new Edge(a, sink, 2);
            edges.Add(asink);
            Edge bsink = new Edge(b, sink, 2);
            edges.Add(bsink);
            /*
             * b      -  sink
             * |        |
             * source -  a
             */

            FlowOnEdges flowOnEdges = calculator.getInitialFlow(edges, source, sink);
            double flowASrc = flowOnEdges.GetFlowOnEdge(srca);
            double flowBSrc = flowOnEdges.GetFlowOnEdge(srcb);
            Assert.IsTrue(flowAmount - (flowASrc + flowBSrc) < ACCEPTED_ERROR);

            double flowASink = flowOnEdges.GetFlowOnEdge(asink);
            double flowBSink = flowOnEdges.GetFlowOnEdge(bsink);
            Assert.IsTrue(flowAmount - (flowASink + flowBSink) < ACCEPTED_ERROR);
        }
    }
}
