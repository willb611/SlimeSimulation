using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Moq;
using SlimeSimulation.Configuration;
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
            var initialSave = new SimulationSave(nonExpandedSlimeState, new SimulationControlInterfaceValues(), new SimulationConfiguration());

            var updatedSlime = slimeNetworkGenerator.FromGraphWithFoodSources(initial);
            var resultState = new SimulationState(updatedSlime, true, initial, 1, 0);

            var updaterMock = new Mock<AsyncSimulationUpdater>();
            Task<SimulationState> nextStateAsync = Task.Run(() => resultState);
            updaterMock.Setup(updater => updater.TaskExpandSlime(nonExpandedSlimeState)).Returns(nextStateAsync);

            SimulationController controller = new SimulationController(null, null, updaterMock.Object, initialSave);
            Assert.IsFalse(controller.ShouldFlowResultsBeDisplayed, "expect flow results to not be displayed for this test");

            controller.AsyncDoNextSimulationStep();
            SimulationState actualSimulationState = controller.GetSimulationState();

            updaterMock.Verify(t => t.TaskExpandSlime(nonExpandedSlimeState));
            Assert.AreEqual(resultState, actualSimulationState);

            Assert.AreEqual(1, controller.StepsTakenForSlimeToExplore);
        }

        [TestMethod(), Timeout(3000)]
        public void RunStepsUntilSlimeHasFullyExplored_ShouldFullyExploreSlime()
        {
            var slimeNetworkGenerator = new SlimeNetworkGenerator();
            var initial = new LatticeGraphWithFoodSourcesGenerator().Generate();
            var slime = slimeNetworkGenerator.FromSingleFoodSourceInGraph(initial);
            var initialState = new SimulationState(slime, false, initial);
            var initialSave = new SimulationSave(initialState, new SimulationControlInterfaceValues(), new SimulationConfiguration());
            var fullyEploredSlimeState = new SimulationState(slime, true, initial);

            var updaterMock = new Mock<AsyncSimulationUpdater>();
            Task<SimulationState> incompleteStateTask = Task.Run((() => initialState));
            Task<SimulationState> nextStateAsync = Task.Run(() => fullyEploredSlimeState);
            updaterMock.SetupSequence(updater => updater.TaskExpandSlime(initialState))
                .Returns(incompleteStateTask)
                .Returns(incompleteStateTask)
                .Returns(incompleteStateTask)
                .Returns(incompleteStateTask)
                .Returns(nextStateAsync);
            SimulationController controller = new SimulationController(null, null, updaterMock.Object, initialSave);
            Assert.IsFalse(controller.ShouldFlowResultsBeDisplayed, "expect flow results to not be displayed for this test");

            controller.AsyncRunStepsUntilSlimeHasFullyExplored();

            SimulationState actualSimulationState = controller.GetSimulationState();
            Assert.AreEqual(fullyEploredSlimeState, actualSimulationState);
            updaterMock.VerifyAll();
        }
    }
}
