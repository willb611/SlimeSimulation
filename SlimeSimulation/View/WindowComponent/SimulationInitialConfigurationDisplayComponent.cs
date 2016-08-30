using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent
{
    class SimulationInitialConfigurationDisplayComponent : VBox
    {
        private readonly SimulationStepWindowControllerTemplate _simulationStepWindowController;

        public SimulationInitialConfigurationDisplayComponent(
            SimulationStepWindowControllerTemplate simulationStepWindowController)
        {
            _simulationStepWindowController = simulationStepWindowController;
            AddComponents(simulationStepWindowController);
        }

        private void AddComponents(SimulationStepWindowControllerTemplate simulationStepWindowController)
        {
            //simulationStepWindowController
        }
    }
}

