using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using SlimeSimulation.View.WindowComponent.SimulationConfigurationComponent;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationCreaterComponent.Tests
{
    [TestClass()]
    public class FlowAmountControlComponentTests
    {
        [TestMethod()]
        public void FlowAmountControlComponentTest()
        {
            double flowAmount = 5.32;
            var component = new FlowAmountControlComponent(flowAmount);
            var readValue = component.ReadFlowAmount();
            Assert.IsTrue(readValue.HasValue);
            Assert.AreEqual(flowAmount, readValue.Value, 0.000001);
            Assert.IsFalse(component.Errors().Any());
        }
    }
}
