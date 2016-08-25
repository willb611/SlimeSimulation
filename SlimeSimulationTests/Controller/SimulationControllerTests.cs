using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller.SimulationUpdaters;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;
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
