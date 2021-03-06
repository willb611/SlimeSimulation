using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Configuration;

namespace SlimeSimulation.Model.Generation.Tests
{
    [TestClass()]
    public class LatticeSlimeNetworkGeneratorTests
    {
        [TestMethod()]
        public void Generate_SizeThree_ShouldWork()
        {
            for (int i = 3; i < 9; i++)
            {
                var generator = new LatticeGraphWithFoodSourcesGenerator(new ConfigForGraphGenerator(i));
                var network = generator.Generate();
                Assert.IsTrue(network.FoodSources.Count >= 2, "2 food sources should always be produced");
            }
        }
    }
}
