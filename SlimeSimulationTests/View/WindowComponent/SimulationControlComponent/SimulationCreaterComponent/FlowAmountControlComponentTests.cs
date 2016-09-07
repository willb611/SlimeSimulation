using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationCreaterComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
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
