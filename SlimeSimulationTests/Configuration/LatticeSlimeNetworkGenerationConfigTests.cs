using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SlimeSimulation.Configuration.Tests
{
    [TestClass()]
    public class LatticeSlimeNetworkGenerationConfigTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void NewInstance_SizeTwo_ShouldThrowException()
        {
            new LatticeGraphWithFoodSourcesGenerationConfig(2);
        }
    }
}
