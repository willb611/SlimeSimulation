using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.View.WindowComponent.SimulationControlComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Configuration;
using SlimeSimulation.StdLibHelpers;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent.Tests
{
    [TestClass()]
    public class SimulationStepNumberOfTimesComponentTests
    {
        [TestMethod()]
        public void SimulationStepNumberOfTimesComponentTest()
        {
            var interfaceValues = new SimulationControlInterfaceValues();
            var stepsToRun = 15;
            var nStepsToSaveAt = 7;
            var shouldSaveEveryNSteps = true;
            interfaceValues.NumberOfStepsToRun = stepsToRun;
            interfaceValues.ShouldSaveEveryNSteps = shouldSaveEveryNSteps;
            interfaceValues.IntervalAtWhichToSaveSimulationWhileRunningSteps = nStepsToSaveAt;

            var component = new SimulationStepNumberOfTimesComponent(null, null, interfaceValues);

            Assert.AreEqual(nStepsToSaveAt, component._nStepsToSaveAt.ExtractIntFromView().Value);
            Assert.AreEqual(stepsToRun, component._numberOfTimesToStepTextView.ExtractIntFromView().Value);
            Assert.AreEqual(shouldSaveEveryNSteps, component._shouldSaveEveryNSteps.Active);
        }
    }
}
