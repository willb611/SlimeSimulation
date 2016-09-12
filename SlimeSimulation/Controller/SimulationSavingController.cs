using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Configuration;
using SlimeSimulation.Model.Simulation;
using SlimeSimulation.Model.Simulation.Persistence;

namespace SlimeSimulation.Controller
{
    class SimulationSavingController
    {
        private const string SaveLocationPrefix = "save";
        private const string SaveLocationFileExtension = ".sim";
        private const string CurrentDateTimeSafeForFilenameFormat = "yyyy.MM.dd-H.mm.ss";
        
        public string LastAttemptedSaveLocation { get; set; }

        private readonly SimulationSaver _simulationSaver = new SimulationSaver();
        

        public Exception SaveSimulation(SimulationSave stateToSave)
        {
            var saveLocation = GetSaveLocation(stateToSave);
            LastAttemptedSaveLocation = saveLocation;
            return _simulationSaver.SaveSimulation(stateToSave, saveLocation);
        }

        private string GetSaveLocation(SimulationSave simulatonSave)
        {
            var state = simulatonSave.SimulationState;
            DateTime currentDateTime = DateTime.Now;
            var dateTimeString = currentDateTime.ToString(CurrentDateTimeSafeForFilenameFormat);
            string simulationStateDescription = "";
            if (state.HasFinishedExpanding)
            {
                simulationStateDescription = "adaptedSteps" + state.StepsTakenInAdaptingState;
            }
            else
            {
                simulationStateDescription = "growthSteps" + state.StepsTakenInExploringState;
            }
            var saveLocation = SaveLocationPrefix + dateTimeString + simulationStateDescription + SaveLocationFileExtension;
            return saveLocation;
        }
    }
}
