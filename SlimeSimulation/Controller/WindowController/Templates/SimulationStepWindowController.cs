using NLog;
using System;
using System.Collections.Generic;
namespace SlimeSimulation.Controller.WindowController
{
    abstract class SimulationStepWindowController : WindowController
    {

        protected SimulationController SimulationController;

        public SimulationStepWindowController(SimulationController simulationController)
        {
            this.SimulationController = simulationController;
        }

        public override void OnQuit()
        {
            base.OnQuit();
            SimulationController.DoNextSimulationStep();
        }
    }
}
