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

        public double TotalDistanceInSlime(SlimeNetwork slime)
        {
            double dist = 0;
            foreach (var edge in slime.SlimeEdges)
            {
                var a = edge.A;
                var b = edge.B;
                var xdelta = a.X - b.X;
                var ydelta = a.X - b.X;
                if (xdelta < Tolerance || ydelta < Tolerance)
                {
                    dist += Math.Abs(xdelta + ydelta);
                }
                else
                {
                    dist += Math.Sqrt(xdelta * xdelta + ydelta * ydelta);
                }
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
    }
}

