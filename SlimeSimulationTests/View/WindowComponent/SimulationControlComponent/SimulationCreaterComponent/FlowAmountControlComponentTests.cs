using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationCreaterComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

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
            Assert.AreEqual(flowAmount, double.Parse(component.TextView.Buffer.Text), 0.000001);
        }
    }
}