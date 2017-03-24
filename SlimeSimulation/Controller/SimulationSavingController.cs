using System;
using SlimeSimulation.Model.Analytics;
using SlimeSimulation.Model.Simulation;
using SlimeSimulation.Model.Simulation.Persistence;

namespace SlimeSimulation.Controller
{
    class SimulationSavingController
    {
        private const string SaveLocationPrefix = "save";
        private const string SaveLocationFileExtension = ".sim";
        private const string StatisticsSaveLocationFileExtension = "stats.csv";
        private const string CurrentDateTimeSafeForFilenameFormat = "yyyy.MM.dd-H.mm.ss";
        
        public string LastAttemptedSaveLocation { get; set; }

        private readonly SimulationSaver _simulationSaver = new SimulationSaver();
        private readonly SimulationStatsSaver _simulationStatsSaver = new SimulationStatsSaver();

        public Exception SaveStatsAboutSimulation(SimulationSave stateToSave)
        {
            var saveLocation = GetSaveLocationForStatistics(stateToSave);
            return _simulationStatsSaver.SaveStatsAboutSimulation(stateToSave, saveLocation);
        }
       
        public Exception SaveSimulation(SimulationSave stateToSave)
        {
            var saveLocation = GetSaveLocation(stateToSave);
            LastAttemptedSaveLocation = saveLocation;
            return _simulationSaver.SaveSimulation(stateToSave, saveLocation);
        }

        private string GetSimulationStateDescription(SimulationState simulationState)
        {
            if (simulationState.HasFinishedExpanding)
            {
                return "adaptedSteps" + simulationState.StepsTakenInAdaptingState;
            }
            else
            {
                return "growthSteps" + simulationState.StepsTakenInExploringState;
            }
        }

        private string GetSaveLocation(SimulationSave simulatonSave)
        {
            var dateTimeString = DateTime.Now.ToString(CurrentDateTimeSafeForFilenameFormat);
            string simulationStateDescription = GetSimulationStateDescription(simulatonSave.SimulationState);
            var saveLocation = SaveLocationPrefix + dateTimeString + simulationStateDescription + SaveLocationFileExtension;
            return saveLocation;
        }
        private string GetSaveLocationForStatistics(SimulationSave stateToSave)
        {
            var dateTimeString = DateTime.Now.ToString(CurrentDateTimeSafeForFilenameFormat);
            string simulationStateDescription = GetSimulationStateDescription(stateToSave.SimulationState);
            return SaveLocationPrefix + dateTimeString + simulationStateDescription + StatisticsSaveLocationFileExtension;
        }
    }
}
