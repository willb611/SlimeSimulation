using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SlimeSimulation.Algorithms.FlowCalculation;
using SlimeSimulation.Model;

namespace SlimeSimulation.Controller.WindowComponentController.Tests
{
    [TestClass()]
    public class FlowResultLineViewControl {
        [TestMethod()]
        public void GetLineWeightForEdgeTest()
        {
            var a = new Node(1,1,1);
            var b = new Node(2, 2, 2);
            var ab = new SlimeEdge(a, b, 1);
            var slimeEdges = new HashSet<SlimeEdge>() {ab};
            var slime = new SlimeNetwork(slimeEdges);
            var flow = new FlowOnEdges(slime.EdgesInGraph);
            const double flowOnEdgeAb = 1;
            flow.IncreaseFlowOnEdgeBy(ab, flowOnEdgeAb);
            var flowResult = new FlowResult(slime, new Route(a, b), 1, flow);

            FlowResultLineViewController controller = new FlowResultLineViewController(flowResult);
            Assert.AreEqual(flowOnEdgeAb, controller.GetLineWeightForEdge(ab), 0.000001);
            Assert.AreEqual(flowOnEdgeAb, controller.GetMaximumLineWeight(), 0.000001);
        }
    }
}
