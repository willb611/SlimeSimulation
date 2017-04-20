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
        private const double Tolerance = 0.00001;

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
            List<FoodSourceNode> foodSources = new List<FoodSourceNode>(slime.FoodSources);
            int fsCount = foodSources.Count;
            double totalSeperation = 0;
            int uniquePaths = 0;
            for (int i = 0; i < fsCount - 1; i++)
            {
                for (int j = i + 1; j < fsCount; j++)
                {
                    var a = foodSources[i];
                    var b = foodSources[j];
                    var degree = DegreeOfSeperation(slime, a, b);
                    if (degree == int.MaxValue)
                    {
                        continue;
                    }
                    totalSeperation += degree;
                    uniquePaths++;
                }
            }
            Logger.Debug("[AverageDegreeOfSeperation] TotalSeperation: {0}, UniquePaths: {1}, foodSources: {2}",
                totalSeperation, uniquePaths, fsCount);
            var result = 0.0;
            if (uniquePaths == 0)
            {
                result = 0;
            }
            else
            {
                result = totalSeperation / uniquePaths;
            }
            Logger.Info("[AverageDegreeOfSeperation] For slime found result: {0}", result);
            return result;
        }

        internal int DegreeOfSeperation(SlimeNetwork slime, Node a, Node b)
        {
            var path = _pathFinder.FindPath(slime as Graph, new Route(a, b));
            Logger.Debug("[DegreeOfSeperation] For node {0} to {1} found {2}", a, b, path);
            return path == null ? int.MaxValue : path.IntermediateFoodSourcesInPathCount();
        }

        public double AverageMinimumDistance(SlimeNetwork slimeNetwork)
        {
            List<FoodSourceNode> foodSources = new List<FoodSourceNode>(slimeNetwork.FoodSources);
            int fsCount = foodSources.Count;
            double totalMinimumDistance = 0;
            int uniquePaths = 0;
            for (int i = 0; i < fsCount - 1; i++)
            {
                for (int j = i + 1; j < fsCount; j++)
                {
                    var a = foodSources[i];
                    var b = foodSources[j];
                    var distance = MinimumDistance(slimeNetwork, a, b);
                    if (distance == int.MaxValue)
                    {
                        continue;
                    }
                    totalMinimumDistance += distance;
                    uniquePaths++;
                }
            }
            Logger.Debug("[AverageMinimumDistance] AverageMinimumDistance: {0}, UniquePaths: {1}, foodSources: {2}",
                totalMinimumDistance, uniquePaths, fsCount);
            var result = 0.0;
            if (uniquePaths == 0)
            {
                result = 0;
            }
            else
            {
                result = totalMinimumDistance / uniquePaths;
            }
            Logger.Info("[AverageMinimumDistance] For slime found result: {0}", result);
            return result;
        }

        private int MinimumDistance(SlimeNetwork slimeNetwork, FoodSourceNode a, FoodSourceNode b)
        {
            var path = _pathFinder.FindPath(slimeNetwork as Graph, new Route(a, b));
            Logger.Debug("[MinimumDistance] For node {0} to {1} found {2}", a, b, path);
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
