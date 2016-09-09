using System;
using NLog;
using SlimeSimulation.Algorithms.FlowCalculation;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller.SimulationUpdaters;
using SlimeSimulation.Controller.WindowController.Templates;
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
                throw new ArgumentNullException($"[constructor] Given null {nameof(gtkLifecycleController)}");
            }
            else
            {
                _gtkLifecycleGtkLifecycleController = gtkLifecycleController;
            }
        }

        public SimulationController MakeSimulationController(AbstractSimulationControllerStarter simulationControllerStarter,
            SimulationSave save)
        {
            return new SimulationController(simulationControllerStarter, GtkLifecycleController.Instance,
                new AsyncSimulationUpdater(save.SimulationConfiguration), save);
        }

        public SimulationController MakeSimulationController(
            AbstractSimulationControllerStarter simulationControllerStarter,
            SimulationConfiguration simulationConfiguration, SimulationControlInterfaceValues controlInterfaceValues,
            SimulationState initial)
        {
            var initialSave = new SimulationSave(initial, controlInterfaceValues, simulationConfiguration);
            return MakeSimulationController(simulationControllerStarter, initialSave);
        }

        public SimulationController MakeSimulationController(
            AbstractSimulationControllerStarter simulationControllerStarter, SimulationConfiguration config)
        {
            FlowOnEdges.ShouldAllowDisconnection = config.ShouldAllowDisconnection;
            var graphWithFoodSources = new LatticeGraphWithFoodSourcesGenerator(config.GenerationConfig).Generate();
            SlimeNetwork initial = new SlimeNetworkGenerator().FromSingleFoodSourceInGraph(graphWithFoodSources);
            var initialState = new SimulationState(initial, false, graphWithFoodSources);

            return MakeSimulationController(simulationControllerStarter, config, new SimulationControlInterfaceValues(),
                initialState);
        }

    }
}
