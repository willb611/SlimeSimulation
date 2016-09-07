using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Configuration;
using SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationCreaterComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationCreation;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationCreaterComponent.Tests
{
    [TestClass()]
    public class LatticeGenerationControlComponentTests
    {
        [TestMethod()]
        public void ReadGenerationConfigTest()
        {
            var size = 123;
            var probablyNewNodeIsFood = 0.2312;
            var minFoodSources = 15;
            var givenConfig = new LatticeGraphWithFoodSourcesGenerationConfig(size,
                probablyNewNodeIsFood, minFoodSources);

            var component = new LatticeGenerationControlComponent(givenConfig);
            var actualConfig = component.ReadGenerationConfig();
            Assert.IsNotNull(actualConfig);
            Assert.IsFalse(component.Errors().Any());
            Assert.AreEqual(givenConfig.MinimumFoodSources, actualConfig.MinimumFoodSources,
                0.0001);
            Assert.AreEqual(givenConfig.Size, actualConfig.Size);
            Assert.AreEqual(givenConfig.MinimumFoodSources, actualConfig.MinimumFoodSources);
        }
    }
}
