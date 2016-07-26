using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;

namespace SlimeSimulation.FlowCalculation.Tests {
    [TestClass()]
    public class FlowCalculatorTests {

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CalculateFlow_WhenNullEdges_ShouldThrowException() {
            var calculator = new FlowCalculator();
            calculator.CalculateFlow(null, new List<Loop>(), new Node(1, 1, 1), new Node(2, 2, 2), 5);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CalculateFlow_WhenNullLoops_ShouldThrowException() {
            var calculator = new FlowCalculator();
            calculator.CalculateFlow(new List<Edge>(), null, new Node(1, 1, 1), new Node(2, 2, 2), 5);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CalculateFlow_WhenNullSource_ShouldThrowException() {
            var calculator = new FlowCalculator();
            calculator.CalculateFlow(new List<Edge>(), new List<Loop>(), null, new Node(2, 2, 2), 5);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CalculateFlow_WhenNullSink_ShouldThrowException() {
            var calculator = new FlowCalculator();
            calculator.CalculateFlow(new List<Edge>(), new List<Loop>(), new Node(1, 1, 1), null, 5);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateFlow_WhenSourceEqualsSink_ShouldThrowException() {
            var calculator = new FlowCalculator();
            var node = new Node(1, 1, 1);
            calculator.CalculateFlow(new List<Edge>(), new List<Loop>(), node, node, 5);
        }

        [TestMethod()]
        public void GetInitialFlow_TotalFlowLeavingNodeAddsToFlowAmount() {
            var calculator = new FlowCalculator();
            int flowAmount = 10;
            Node source = new Node(1, 1, 1);
            Node a = new Node(2, 2, 1);
            Node b = new Node(3, 1, 2);
            Node sink = new Node(4, 2, 2);
            List<Node> nodes = new List<Node>() { source, a, b, sink };

            Edge srca = new Edge(source, a, 1);
            Edge srcb = new Edge(source, b, 1);
            Edge sinka = new Edge(sink, a, 1);
            Edge sinkb = new Edge(sink, b, 1);
            List<Edge> edges = new List<Edge>() { srca, srcb, sinka, sinkb };
            /*
             * b      -  sink
             * |        |
             * source -  a
             */
            Loop loop = new Loop(nodes, edges);
            IntermediateFlowResult intermediateResult = calculator.GetInitialFlow(new Graph(nodes, edges), new List<Loop>() { loop }, source, sink, flowAmount);
            FlowOnEdges flowOnEdges = intermediateResult.FlowOnEdges;
            Assert.AreEqual(flowAmount / 2, flowOnEdges.GetFlowOnEdge(srca));
            Assert.AreEqual(flowAmount / 2, flowOnEdges.GetFlowOnEdge(srcb));

            Assert.AreEqual(flowAmount / 2, flowOnEdges.GetFlowOnEdge(sinka));
            Assert.AreEqual(flowAmount / 2, flowOnEdges.GetFlowOnEdge(sinkb));
        }
    }
}
