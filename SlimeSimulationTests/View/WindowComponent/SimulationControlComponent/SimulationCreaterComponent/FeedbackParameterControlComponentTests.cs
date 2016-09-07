using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Configuration;
using SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationCreaterComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationCreation;
using SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationUpdateParameters;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationCreaterComponent.Tests
{
    [TestClass()]
    public class FeedbackParameterControlComponentTests
    {
        [TestMethod()]
        public void FeedbackParameterControlComponentTest()
        {
            var feedbackParam = 1.35123;
            var givenConfig = new SlimeNetworkAdaptionCalculatorConfig(feedbackParam);
            var component = new FeedbackParameterControlComponent(givenConfig);

            var actualConfig = component.ReadConfiguration();
            Assert.IsNotNull(actualConfig);
            Assert.AreEqual(givenConfig.FeedbackParam, actualConfig.FeedbackParam);
            Assert.IsFalse(component.Errors().Any());
        }
    }
}
