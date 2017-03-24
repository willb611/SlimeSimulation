using System;
using System.IO;
using NLog;
using SlimeSimulation.Model.Simulation;

namespace SlimeSimulation.Model.Analytics
{
    public class SimulationStatsSaver
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        private readonly SimulationStateAnalyticsGatherer _statGatherer = new SimulationStateAnalyticsGatherer();

        public Exception SaveStatsAboutSimulation(SimulationSave stateToSave, string saveLocation)
        {
            try
            {
                var slime = stateToSave.SimulationState.SlimeNetwork;
                using (StreamWriter streamWriter = new StreamWriter(saveLocation))
                {
                    streamWriter.WriteLine("Total Distance, Average degree of seperation, Fault Tolerance");
                    streamWriter.Write(_statGatherer.TotalDistanceInSlime(slime));
                    streamWriter.Write(",");
                    streamWriter.Write(_statGatherer.AverageDegreeOfSeperation(slime));
                    streamWriter.Write(",");
                    streamWriter.Write(_statGatherer.FaultTolerance(slime));
                }
                Logger.Info("[SaveStatsAboutSimulation] Saved stats to {0}", saveLocation);
                return null;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return e;
            }
        }
    }
}
