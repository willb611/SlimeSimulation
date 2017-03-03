using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Configuration;
using System.Linq;
using SlimeSimulation.View.WindowComponent.SimulationConfigurationComponent;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationCreaterComponent.Tests
{
    [TestClass()]
    public class FeedbackParameterControlComponentTests
    {
        [TestMethod()]
        public void FeedbackParameterControlComponentTest()
        {
            var feedbackParam = 1.35123;
            var component = new FeedbackParameterControlComponent(feedbackParam);

            var actualFeedbackParam = component.ReadFeedbackParameter();
            Assert.IsNotNull(actualFeedbackParam);
            Assert.AreEqual(feedbackParam, actualFeedbackParam.Value);
            Assert.IsFalse(component.Errors().Any());
        }
    }
}
