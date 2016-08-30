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
        [TestMethod(), Timeout(3000)]
        public void DoSimulationStep_WhenStateIsSlimeExpanding_ShouldExpandSlime()
        {
            var slimeNetworkGenerator = new SlimeNetworkGenerator();
            var initial = new LatticeGraphWithFoodSourcesGenerator().Generate();
            var slime = slimeNetworkGenerator.FromSingleFoodSourceInGraph(initial);
            var nonExpandedSlimeState = new SimulationState(slime, false, initial);

            var updatedSlime = slimeNetworkGenerator.FromGraphWithFoodSources(initial);
            var resultState = new SimulationState(updatedSlime, initial);

            var updaterMock = new Mock<SimulationUpdater>();
            Task<SimulationState> nextStateAsync = Task.Run(() => resultState);
            updaterMock.Setup(updater => updater.TaskExpandSlime(nonExpandedSlimeState)).Returns(nextStateAsync);

            SimulationController controller = new SimulationController(null, null, null, nonExpandedSlimeState, updaterMock.Object);
            Assert.IsFalse(controller.ShouldFlowResultsBeDisplayed, "expect flow results to not be displayed for this test");

            controller.DoNextSimulationStep();
            SimulationState actualSimulationState = controller.GetSimulationState();

            updaterMock.Verify(t => t.TaskExpandSlime(nonExpandedSlimeState));
            Assert.AreEqual(resultState, actualSimulationState);

            Assert.AreEqual(1, controller.SimulationStepsCompleted);
        }

        [TestMethod(), Timeout(3000)]
        public void RunStepsUntilSlimeHasFullyExplored_ShouldFullyExploreSlime()
        {
            var slimeNetworkGenerator = new SlimeNetworkGenerator();
            var initial = new LatticeGraphWithFoodSourcesGenerator().Generate();
            var slime = slimeNetworkGenerator.FromSingleFoodSourceInGraph(initial);
            var initialState = new SimulationState(slime, false, initial);
            var fullyEploredSlimeState = new SimulationState(slime, true, initial);

            var updaterMock = new Mock<SimulationUpdater>();
            Task<SimulationState> incompleteStateTask = Task.Run((() => initialState));
            Task<SimulationState> nextStateAsync = Task.Run(() => fullyEploredSlimeState);
            updaterMock.SetupSequence(updater => updater.TaskExpandSlime(initialState))
                .Returns(incompleteStateTask)
                .Returns(incompleteStateTask)
                .Returns(incompleteStateTask)
                .Returns(incompleteStateTask)
                .Returns(nextStateAsync);
            SimulationController controller = new SimulationController(null, null, null, initialState, updaterMock.Object);
            Assert.IsFalse(controller.ShouldFlowResultsBeDisplayed, "expect flow results to not be displayed for this test");

            controller.RunStepsUntilSlimeHasFullyExplored();

            SimulationState actualSimulationState = controller.GetSimulationState();
            Assert.AreEqual(fullyEploredSlimeState, actualSimulationState);
            updaterMock.VerifyAll();
        }
    }
}
