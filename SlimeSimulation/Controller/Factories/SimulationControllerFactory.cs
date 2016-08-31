using System;
using NLog;
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
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly GtkLifecycleController _gtkLifecycleGtkLifecycleController;

        public SimulationControllerFactory() : this(GtkLifecycleController.Instance)
        {
            
        }
        public SimulationControllerFactory(GtkLifecycleController gtkLifecycleController)
        {
            if (gtkLifecycleController == null)
            {
                throw new ArgumentNullException($"[constructor] Given null {gtkLifecycleController}");
            }
            else
            {
                _gtkLifecycleGtkLifecycleController = gtkLifecycleController;
            }
        }

        public SimulationController MakeSimulationController(
            NewSimulationStarterWindowController applicationStartWindowController, SimulationConfiguration config)
        {
            FlowOnEdges.ShouldAllowDisconnection = config.ShouldAllowDisconnection;
            var graphWithFoodSources = MakeGraph(config.GenerationConfig);
            SlimeNetwork initial = new SlimeNetworkGenerator().FromSingleFoodSourceInGraph(graphWithFoodSources);

            var simulationUpdater = new SimulationUpdater(config);
            var initialState = new SimulationState(initial, false, graphWithFoodSources);

            return new SimulationController(applicationStartWindowController, config, _gtkLifecycleGtkLifecycleController, initialState,
                simulationUpdater);
        }

        private GraphWithFoodSources MakeGraph(LatticeGraphWithFoodSourcesGenerationConfig config)
        {
            var latticeGraphWithFoodSourcesGenerator = new LatticeGraphWithFoodSourcesGenerator(config);
            return latticeGraphWithFoodSourcesGenerator.Generate();
        }
    }
}
