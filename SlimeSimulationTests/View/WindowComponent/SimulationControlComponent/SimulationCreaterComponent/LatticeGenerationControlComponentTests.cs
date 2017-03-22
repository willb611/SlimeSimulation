using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Configuration;
using System.Linq;
using SlimeSimulation.Model.Generation;
using SlimeSimulation.View.WindowComponent.SimulationCreationComponent;

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
            var edgeConnectionType = EdgeConnectionShape.EdgeConnectionShapeSquareWithCrossedDiagonals;
            var givenConfig = new ConfigForGraphGenerator(size,
                probablyNewNodeIsFood, minFoodSources, edgeConnectionType);

            var component = new LatticeGenerationControlComponent(givenConfig);
            var actualConfig = component.ReadGenerationConfig();
            Assert.IsNotNull(actualConfig);
            Assert.IsFalse(component.Errors().Any());
            Assert.AreEqual(givenConfig.MinimumFoodSources, actualConfig.MinimumFoodSources,
                0.0001);
            Assert.AreEqual(givenConfig.Size, actualConfig.Size);
            Assert.AreEqual(givenConfig.MinimumFoodSources, actualConfig.MinimumFoodSources);
            Assert.AreEqual(edgeConnectionType, actualConfig.EdgeConnectionType);
        }
    }
}
