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
using SlimeSimulation.Model.Simulation;
using SlimeSimulation.View;

namespace SlimeSimulation.Controller.Factories
{
    public class SimulationControllerFactory
    {
        private readonly GtkLifecycleController _gtkLifecycleController;

        public SimulationControllerFactory() : this(GtkLifecycleController.Instance)
        {
            
        }
        public SimulationControllerFactory(GtkLifecycleController controller)
        {
            _gtkLifecycleController = controller;
        }

        public SimulationController MakeSimulationController(
            ApplicationStartWindowController applicationStartWindowController, SimulationConfiguration config)
        {
            FlowOnEdges.ShouldAllowDisconnection = config.ShouldAllowDisconnection;
            var graphWithFoodSources = MakeGraph(config.GenerationConfig);
            SlimeNetwork initial = new SlimeNetworkGenerator().FromSingleFoodSourceInGraph(graphWithFoodSources);

            var simulationUpdater = new SimulationUpdater(config);
            var initialState = new SimulationState(initial, false, graphWithFoodSources);

            return new SimulationController(applicationStartWindowController, config, _gtkLifecycleController, initialState,
                simulationUpdater);
        }

        private GraphWithFoodSources MakeGraph(LatticeGraphWithFoodSourcesGenerationConfig config)
        {
            var latticeGraphWithFoodSourcesGenerator = new LatticeGraphWithFoodSourcesGenerator(config);
            return latticeGraphWithFoodSourcesGenerator.Generate();
        }
    }
}
