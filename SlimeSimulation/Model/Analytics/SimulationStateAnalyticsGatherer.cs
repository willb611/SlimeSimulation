using System;
using SlimeSimulation.Model.Simulation;

namespace SlimeSimulation.Model.Analytics
{
    public class SimulationStateAnalyticsGatherer
    {
        private const double Tolerance = 0.00001;

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
    }
}

