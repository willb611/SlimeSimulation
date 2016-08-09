using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.FlowCalculation.LinearEquations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;
using NLog;

namespace SlimeSimulation.FlowCalculation.LinearEquations.Tests
{
    [TestClass()]
    public class FlowCalculatorTests
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [TestMethod()]
        public void SwapTest()
        {
            Node source = new Node(1, 1, 1);
            Node sink = new Node(2, 1, 1);
            Node other = new Node(3, 1, 1);
            List<Node> nodes = new List<Node>() { sink, source, other };

            FlowCalculator flowCalculator = new FlowCalculator(new LupDecompositionSolver());
            flowCalculator.EnsureSourceSinkInCorrectPositions(nodes, source, sink);
            Assert.AreEqual(nodes[0], source);
            Assert.AreEqual(nodes[2], sink);
            logger.Info(LogHelper.CollectionToString(nodes));
        }

        [TestMethod()]
        public void CalculateFlowTest()
        {
            var generator = new LatticeSlimeNetworkGenerator(7);
            var network = generator.Generate();
            var calculator = new FlowCalculator(new LupDecompositionSolver());

            Node source = network.FoodSources.First();
            Node sink = network.FoodSources.Last();
            var flowAmount = 10;
            var dflow = 10.0;
            var result = calculator.CalculateFlow(network.Edges, network.Nodes, source, sink, flowAmount);

            Assert.AreEqual(dflow, Math.Abs(result.GetFlowOnNode(source)), 0.000001);
            Assert.AreEqual(dflow, Math.Abs(result.GetFlowOnNode(sink)), 0.000001);
        }
    }
}
