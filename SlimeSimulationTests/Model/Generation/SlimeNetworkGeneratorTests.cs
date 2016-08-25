using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace SlimeSimulation.Model.Generation.Tests
{
    [TestClass()]
    public class SlimeNetworkGeneratorTests
    {
        [TestMethod()]
        public void FromGraphWithFoodSourcesTest()
        {
            var graph = new LatticeGraphWithFoodSourcesGenerator().Generate();
            Assert.IsTrue(graph.Edges.Count > 1);
            Assert.IsTrue(graph.Nodes.Count() > 1);
            Assert.IsTrue(graph.FoodSources.Count > 1);

            SlimeNetwork slime = new SlimeNetworkGenerator().FromGraphWithFoodSources(graph);
            Assert.AreEqual(graph.Edges.Count, slime.Edges.Count);
            Assert.IsTrue(graph.FoodSources.SequenceEqual(slime.FoodSources));
            Assert.IsTrue(graph.Nodes.SequenceEqual(slime.Nodes));
        }
    }
}
