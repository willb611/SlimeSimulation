using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Configuration;
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
            var calculator = new SlimeNetworkAdaptionCalculator(new SlimeNetworkAdaptionCalculatorConfig(feedbackParameter, 0.5));
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
            double timePerSimStep = 0.5;
            // expectedDelta = 0.3;
            // expectedFlow = connectivity (0.5) + [expectedDelta * timeperStep]
                         // = 0.5 + 0.3 * 0.5
            double expectedFlow = 0.65;
            var calculator = new SlimeNetworkAdaptionCalculator(new SlimeNetworkAdaptionCalculatorConfig(feedbackParameter, timePerSimStep));
            double actual = calculator.NextConnectivityForEdge(edge, flow);

            Assert.AreEqual(expectedFlow, actual, 0.000001);
        }

        [TestMethod()]
        public void GetUpdatedFlowTest_WithSmallerTimePerStep()
        {
            double feedbackParameter = 2;
            double connectivity = 0.5;
            Node a = new Node(1, 1, 1);
            Node b = new Node(2, 2, 2);
            SlimeEdge edge = new SlimeEdge(a, b, connectivity);

            double flow = 2;
            double timePerSimStep = 0.25;
            // expectedDelta = 0.3;
            // expectedFlow = connectivity (0.5) + [expectedDelta * timeperStep]
                         // = 0.5 + 0.3 * 0.25
            double expectedFlow = 0.575;
            var calculator = new SlimeNetworkAdaptionCalculator(new SlimeNetworkAdaptionCalculatorConfig(feedbackParameter, timePerSimStep));
            double actual = calculator.NextConnectivityForEdge(edge, flow);

            Assert.AreEqual(expectedFlow, actual, 0.000001);
        }

        [TestMethod()]
        public void TestRemoveDisconnectedEdges_AllowedToDisconnect()
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

            var calc = new SlimeNetworkAdaptionCalculator(new SlimeNetworkAdaptionCalculatorConfig(), true);
            var connectedEdges = calc.RemoveDisconnectedEdges(edges);
            Assert.IsFalse(connectedEdges.Contains(disconnectedEdge));
            Assert.IsTrue(connectedEdges.Contains(ac));
            Assert.IsTrue(connectedEdges.Contains(bc));
        }

        [TestMethod()]
        public void TestRemoveDisconnectedEdges_NotAllowedToDisconnect()
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

            var calc = new SlimeNetworkAdaptionCalculator(new SlimeNetworkAdaptionCalculatorConfig(), false);
            var connectedEdges = calc.RemoveDisconnectedEdges(edges);
            Assert.IsTrue(connectedEdges.Contains(disconnectedEdge));
            Assert.IsTrue(connectedEdges.Contains(ac));
            Assert.IsTrue(connectedEdges.Contains(bc));
        }
    }
}
