using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Configuration;

namespace SlimeSimulation.Model.Simulation
{
    public class SimulationSave
    {
        public SimulationState SimulationState { get; }
        public SimulationControlInterfaceValues SimulationControlInterfaceValues { get; }
        public SimulationConfiguration SimulationConfiguration { get; }

        public SimulationSave(SimulationState simulationState, SimulationControlInterfaceValues simulationControlInterfaceValues,
            SimulationConfiguration simulationConfiguration)
        {
            SimulationState = simulationState;
            SimulationControlInterfaceValues = simulationControlInterfaceValues;
            SimulationConfiguration = simulationConfiguration;
        }
    }
}
