using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Model;
using SlimeSimulation.FlowCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [Ignore]
        public void GetInitialFlow_TotalFlowLeavingNodeAddsToFlowAmount() {
            double ACCEPTED_ERROR = 0.01;
            var calculator = new FlowCalculator();
            double flowAmount = 2;
            List<Edge> edges = new List<Edge>();
            List<Node> nodes = new List<Node>();
            Node source = new Node(1, 1, 1);
            Node sink = new Node(4, 2, 2);
            /*
             * b      -  sink
             * |        |
             * source -  a
             */
            Loop loop = new Loop(nodes, edges);
            IntermediateFlowResult intermediateResult = calculator.GetInitialFlow(edges, new List<Loop>() { loop }, source, sink, 5);
            

        }
    }
}
