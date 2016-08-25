using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;

namespace SlimeSimulation.Controller.WindowsComponentController
{
    public abstract class LineViewController
    {
        public abstract double GetLineWeightForEdge(SlimeEdge edge);
        public abstract double GetMaximumLineWeight();
    }

    internal class FlowResultLineViewController : LineViewController
    {
        private readonly FlowResult _flowResult;
        private readonly double _maxLineWidth;

        public FlowResultLineViewController(FlowResult flowResult)
        {
            _flowResult = flowResult;
            _maxLineWidth = flowResult.GetMaximumFlowOnEdge();
        }

        public override double GetLineWeightForEdge(SlimeEdge edge)
        {
            return Math.Abs(_flowResult.FlowOnEdge(edge));
        }

        public override double GetMaximumLineWeight()
        {
            return _maxLineWidth;
        }
    }

    internal class ConnectivityLineViewController : LineViewController
    {
        private List<SlimeEdge> _edges;
        private readonly double _max;

        public ConnectivityLineViewController(List<SlimeEdge> edges)
        {
            _edges = edges;
            var max = 0.0;
            foreach (SlimeEdge edge in edges)
            {
                max = Math.Max(edge.Connectivity, max);
            }
            _max = max;
        }

        public override double GetLineWeightForEdge(SlimeEdge edge)
        {
            return edge.Connectivity;
        }

        public override double GetMaximumLineWeight()
        {
            return _max;
        }
    }
}
