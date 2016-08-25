using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Configuration;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                var generator = new LatticeGraphWithFoodSourcesGenerator(new LatticeGraphWithFoodSourcesGenerationConfig(i));
                var network = generator.Generate();
                Assert.IsTrue(network.FoodSources.Count >= 2, "2 food sources should always be produced");
            }
        }
    }
}
