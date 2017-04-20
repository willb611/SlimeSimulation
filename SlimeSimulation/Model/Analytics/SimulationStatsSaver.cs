using System;
using System.IO;
using NLog;
using SlimeSimulation.Algorithms.Pathing;
using SlimeSimulation.Model.Simulation;

namespace SlimeSimulation.Model.Analytics
{
    public class SimulationStatsSaver
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly PathFinder _pathFinder = new PathFinder();
        private readonly SimulationStateAnalyticsGatherer _statGatherer = new SimulationStateAnalyticsGatherer();

        public Exception SaveStatsAboutSimulation(SimulationSave stateToSave, string saveLocation)
        {
            if (!stateToSave.SimulationState.HasFinishedExpanding)
            {
                var e = new ApplicationException("Can't save stats about a slime where it's not fully explored");
                Logger.Info(e);
                return e;
            }
            try
            {
                var slime = stateToSave.SimulationState.SlimeNetwork;
                using (StreamWriter streamWriter = new StreamWriter(saveLocation))
                {
                    streamWriter.WriteLine("Total Distance, Average degree of seperation, Fault Tolerance, Cost-Benefit ratio, Average Minimum Distance");
                    var totalLength = _statGatherer.TotalDistanceInSlime(slime);
                    streamWriter.Write(totalLength);
                    streamWriter.Write(",");
                    var pathsBetweenAllFoodSources = new AllPaths(slime);
                    streamWriter.Write(_statGatherer.AverageDegreeOfSeperation(slime, pathsBetweenAllFoodSources));
                    streamWriter.Write(",");
                    var faultTolerance = _statGatherer.FaultTolerance(slime, totalLength);
                    streamWriter.Write(faultTolerance);
                    streamWriter.Write(",");
                    streamWriter.Write(_statGatherer.CostBenefitRatio(faultTolerance, totalLength));
                    streamWriter.Write(",");
                    streamWriter.Write(_statGatherer.AverageMinimumDistance(slime, pathsBetweenAllFoodSources));
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
