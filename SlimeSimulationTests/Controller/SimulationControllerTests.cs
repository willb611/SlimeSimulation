using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller.SimulationUpdaters;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.Model.Generation;

namespace SlimeSimulation.Controller.Tests
{
    [TestClass()]
    public class SimulationControllerTests
    {
        [TestMethod()]
        public void SimulationControllerTest()
        {
            var generator = new LatticeGraphWithFoodSourcesGenerator(new LatticeGraphWithFoodSourcesGenerationConfig());
            var network = generator.Generate();
            var slime = new SlimeNetworkGenerator().FromGraphWithFoodSources(network);

            var simulationUpdater = new SimulationUpdater();
            var startingWindowController = new ApplicationStartWindowController();

            var controller = new SimulationController(startingWindowController, simulationUpdater, slime, network);
            var state = controller.SimulationState;
            Assert.IsTrue(state.HasFinishedExpanding);
        }
    }
}
