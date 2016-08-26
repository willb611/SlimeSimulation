using System.Collections.Generic;
using SlimeSimulation.Controller.SimulationUpdaters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Model;

namespace SlimeSimulation.Controller.SimulationUpdaters.Tests
{
    [TestClass()]
    public class SlimeNetworkAdaptionCalculatorTests
    {
        [TestMethod()]
        public void FunctionOfFlowTest()
        {
            double feedbackParameter = 2;
            var calculator = new SlimeNetworkAdaptionCalculator(feedbackParameter);
            double expected = 4 / 5.0;
            double actual = calculator.FunctionOfFlow(2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetUpdatedFlowTest()
        {
            double feedbackParameter = 2;
            double connectivity = 0.5;
            Node a = new Node(1, 1, 1);
            Node b = new Node(2, 2, 2);
            SlimeEdge edge = new SlimeEdge(a, b, connectivity);

            double flow = 2;
            double expected = 0.8;
            var calculator = new SlimeNetworkAdaptionCalculator(feedbackParameter);
            double actual = calculator.NextConnectivityForEdge(edge, flow);

            Assert.AreEqual(expected, actual, 0.000001);
        }
        
        [TestMethod()]
        public void TestRemoveDisconnectedEdges()
        {
            var a = new Node(1, 1, 1);
            var b = new FoodSourceNode(2, 2, 2);
            var c = new FoodSourceNode(3, 3, 3);
            var foodSources = new HashSet<FoodSourceNode>() { b, c };
            var nodes = new HashSet<Node>() { a, b, c };

            var disconnectedEdge = new SlimeEdge(a, b, 0);
            var ac = new SlimeEdge(a, c, 1);
            var bc = new SlimeEdge(b, c, 2);
            var edges = new HashSet<SlimeEdge>() { disconnectedEdge, ac, bc };

            var connectedEdges = SlimeNetworkAdaptionCalculator.RemoveDisconnectedEdges(edges);
            Assert.IsFalse(connectedEdges.Contains(disconnectedEdge));
            Assert.IsTrue(connectedEdges.Contains(ac));
            Assert.IsTrue(connectedEdges.Contains(bc));
        }
    }
}
