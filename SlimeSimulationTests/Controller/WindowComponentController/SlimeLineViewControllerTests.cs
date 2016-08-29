using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Controller.WindowComponentController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;

namespace SlimeSimulation.Controller.WindowComponentController.Tests
{
    [TestClass()]
    public class SlimeLineViewControllerTests
    {
        [TestMethod()]
        public void GetLineWeightForEdgeTest()
        {
            var edge = new Edge(new Node(1,1,1), new Node(2,2,2));
            var slimeEdge = new SlimeEdge(edge, 2);

            var controller = new SlimeLineViewController(new List<SlimeEdge>() {slimeEdge});
            Assert.AreEqual(slimeEdge.Connectivity, controller.GetLineWeightForEdge(slimeEdge));
            Assert.AreEqual(slimeEdge.Connectivity, controller.GetMaximumLineWeight(),
                "When 1 edge is provided, connectivity of that edge should be the max line weight");
        }
    }
}
