using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.WindowComponent;

namespace SlimeSimulation.View.Factories
{
    interface ISimulationControlBoxFactory
    {
        SimulationControlBox MakeControlBox(SimulationStepWindowController simulationStepWindowController, Window parentWindow);
    }
}
