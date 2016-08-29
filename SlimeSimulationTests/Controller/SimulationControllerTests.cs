using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SlimeSimulation.Controller.SimulationUpdaters;
using SlimeSimulation.Model.Generation;
using SlimeSimulation.Model.Simulation;

namespace SlimeSimulation.Controller.Tests
{
    [TestClass()]
    public class SimulationControllerTests
    {
        [TestMethod()]
        public void DoSimulationStepTest()
        {
            var slimeNetworkGenerator = new SlimeNetworkGenerator();
            var initial = new LatticeGraphWithFoodSourcesGenerator().Generate();
            var slime = slimeNetworkGenerator.FromSingleFoodSourceInGraph(initial);
            var updatedSlime = slimeNetworkGenerator.FromGraphWithFoodSources(initial);

            var updaterMock = new Mock<SimulationUpdater>();
            var state = new SimulationState(slime, true, initial);
            var otherState = new SimulationState(updatedSlime, false, initial);
            SimulationController controller = new SimulationController(null, null, null, state, updaterMock.Object);
            Task<SimulationState> nextStateAsync = Task.Run(() => otherState);
            updaterMock.Setup(updater => updater.TaskCalculateFlowAndUpdateNetwork(state)).Returns(nextStateAsync);
            Assert.IsFalse(controller.ShouldFlowResultsBeDisplayed);

            controller.DoNextSimulationStep();
            SimulationState actualSimulationState = controller.GetSimulationState();

            Assert.AreEqual(otherState, actualSimulationState);
            updaterMock.Verify(t => t.TaskCalculateFlowAndUpdateNetwork(state));
        }
    }
}
