using System;
using System.Collections.Generic;
using NLog;
using SlimeSimulation.Algorithms.Bfs;
using SlimeSimulation.Algorithms.Pathing;
using SlimeSimulation.Model.Simulation;

namespace SlimeSimulation.Model.Analytics
{
    public class SimulationStateAnalyticsGatherer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly PathFinder _pathFinder = new PathFinder();
        private readonly BfsSolver _bfsSolver = new BfsSolver();

        public double TotalDistanceInSlime(SlimeNetwork slime)
        {
            double dist = 0;
            foreach (var edge in slime.SlimeEdges)
            {
                dist += edge.Length();
            }
            Logger.Info("[TotalDistanceInSlime] Found distance as {0}", dist);
            return dist;
        }

        public double AverageDegreeOfSeperation(SlimeNetwork slime)
        {
            return AverageDegreeOfSeperation(slime, new AllPaths(slime));
        }
        public double AverageDegreeOfSeperation(SlimeNetwork slime, AllPaths allPaths)
        {
            int fsCount = slime.FoodSources.Count;
            double totalSeperation = 0;
            int uniquePaths = 0;
            foreach (var path in allPaths.AsList())
            {
                var degree = DegreeOfSeperation(path);
                if (degree == int.MaxValue)
                {
                    continue;
                }
                totalSeperation += degree;
                uniquePaths++;
            }
            Logger.Debug("[AverageDegreeOfSeperation] TotalSeperation: {0}, UniquePaths: {1}, foodSources: {2}",
                totalSeperation, uniquePaths, fsCount);
            var result = 0.0;
            if (uniquePaths > 0)
            {
                result = totalSeperation / uniquePaths;
            }
            Logger.Info("[AverageDegreeOfSeperation] For slime found result: {0}", result);
            return result;
        }

        internal int DegreeOfSeperation(Path path)
        {
            return path == null ? int.MaxValue : path.IntermediateFoodSourcesInPathCount();
        }

        public double AverageMinimumDistance(SlimeNetwork slimeNetwork)
        {
            return AverageMinimumDistance(slimeNetwork, new AllPaths(slimeNetwork));
        }
        public double AverageMinimumDistance(SlimeNetwork slimeNetwork, AllPaths allPaths)
        {
            int fsCount = slimeNetwork.FoodSources.Count;
            double totalMinimumDistance = 0;
            int uniquePaths = 0;
            foreach (var path in allPaths.AsList())
            {
                var distance = MinimumDistance(path);
                if (distance == int.MaxValue)
                {
                    continue;
                }
                totalMinimumDistance += distance;
                uniquePaths++;
            }
            Logger.Debug("[AverageMinimumDistance] AverageMinimumDistance: {0}, UniquePaths: {1}, foodSources: {2}",
                totalMinimumDistance, uniquePaths, fsCount);
            var result = 0.0;
            if (uniquePaths > 0)
            {
                result = totalMinimumDistance / uniquePaths;
            }
            Logger.Info("[AverageMinimumDistance] For slime found result: {0}", result);
            return result;
        }

        private int MinimumDistance(Path path)
        {
            return path == null ? int.MaxValue : path.Distance();
        }

        public double FaultTolerance(SlimeNetwork slime)
        {
            return FaultTolerance(slime, TotalDistanceInSlime(slime));
        }
        // Fault tolerance = length of edges which cause fault if dc'd / Total Length
        public double FaultTolerance(SlimeNetwork slime, double totalLength)
        {
            GraphSplitIntoSubgraphs slimeSplit = _bfsSolver.SplitIntoSubgraphs(slime);
            var subgraphCount = slimeSplit.Subgraphs.Count;
            Logger.Info("[FaultTolerance] Found sugraphCount as: {0}", subgraphCount);
            double nonFaultedLength = 0;
            foreach (var e in slime.SlimeEdges)
            {
                var edgesWithFault = new HashSet<SlimeEdge>(slime.SlimeEdges);
                if (!edgesWithFault.Remove(e))
                {
                    Logger.Warn("[FaultTolerance] Failed to remove edge {0} from slime network", e);
                }
                // Construct passing in nodes, food sources. So if a node is disconnected on it's own, it get's included still
                // and counts as a seperate subgraph in the slime network.
                var faultySlime = new SlimeNetwork(slime.NodesInGraph, slime.FoodSources, edgesWithFault);
                var faultySplit = _bfsSolver.SplitIntoSubgraphs(faultySlime);
                int faultySubgraphCount = faultySplit.Subgraphs.Count;
                int difference = subgraphCount - faultySubgraphCount;
                if (Math.Abs(difference) > 0)
                {
                    Logger.Trace("[FaultTolerance] Found a fault at edge: {0}", e);
                }
                else
                {
                    nonFaultedLength += e.Length();
                }
            }
            double faultTolerance = nonFaultedLength/totalLength;
            Logger.Info("[FaultTolerance] Total Length: {0} FT: {1}", totalLength, faultTolerance);
            return faultTolerance;
        }

        public double CostBenefitRatio(double faultTolerance, double totalLength)
        {
            return faultTolerance/totalLength;
        }
    }
}
