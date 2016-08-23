using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent
{
    class SimulationControlBox : VBox
    {

        public SimulationControlBox(SimulationStepWindowController simulationStepWindowController, Window parentWindow) : base(true, 10)
        {
            AddControls(simulationStepWindowController, parentWindow);
        }

        private void AddControls(SimulationStepWindowController simulationStepWindowController, Window parentWindow)
        {
            Add(new SimulationStepButton(simulationStepWindowController));
            Add(new SimulationStepNumberOfTimesComponent(simulationStepWindowController, parentWindow));
        }
    }
}
