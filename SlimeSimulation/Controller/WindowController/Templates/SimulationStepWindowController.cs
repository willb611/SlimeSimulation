using NLog;
using System;
using System.Collections.Generic;
namespace SlimeSimulation.Controller.WindowController
{
    abstract class SimulationStepWindowController : WindowController
    {

        protected SimulationController simulationController;

        public SimulationStepWindowController(SimulationController simulationController)
        {
            this.simulationController = simulationController;
        }

        public override void OnQuit()
        {
            base.OnQuit();
            simulationController.DoNextSimulationStep();
        }
    }
}
