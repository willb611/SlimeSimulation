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
            if (uniquePaths == 0)
            {
                return 0;
            }
            else
            {
                return totalSeperation / uniquePaths;
            }
        }

        internal int DegreeOfSeperation(SlimeNetwork slime, Node a, Node b)
        {
            var path = _pathFinder.FindPath(slime as Graph, new Route(a, b));
            Logger.Debug("[DegreeOfSeperation] For node {0} to {1} found {2}", a, b, path);
            return path == null ? int.MaxValue : path.IntermediateNodesInPathCount();
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
    }
}
