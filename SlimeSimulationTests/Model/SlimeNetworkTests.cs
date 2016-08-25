using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model.Generation;

namespace SlimeSimulation.Model.Tests
{
    [TestClass()]
    public class SlimeNetworkTests
    {
        [TestMethod()]
        public void CoversGraphTest()
        {
            var graph = new LatticeGraphWithFoodSourcesGenerator().Generate();
            Assert.IsTrue(graph.Edges.Count > 1);
            Assert.IsTrue(graph.Nodes.Count() > 1);
            Assert.IsTrue(graph.FoodSources.Count > 1);

            SlimeNetwork slime = new SlimeNetworkGenerator().FromGraphWithFoodSources(graph);
            Assert.IsTrue(slime.CoversGraph(graph));
        }
    }
}
