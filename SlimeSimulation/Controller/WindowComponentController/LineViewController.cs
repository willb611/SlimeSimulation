using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;

namespace SlimeSimulation.Controller.WindowsComponentController
{
    public abstract class LineViewController
    {
        public abstract double GetLineWeightForEdge(Edge edge);
        public abstract double GetMaximumLineWeight();
    }

    internal class FlowResultLineViewController : LineViewController
    {
        private readonly FlowResult _flowResult;
        private readonly double _maxLineWidth;

        public FlowResultLineViewController(FlowResult flowResult)
        {
            this._flowResult = flowResult;
            _maxLineWidth = flowResult.GetMaximumFlowOnEdge();
        }

        public override double GetLineWeightForEdge(Edge edge)
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
        private List<Edge> _edges;
        private readonly double _max;

        public ConnectivityLineViewController(List<Edge> edges)
        {
            this._edges = edges;
            var max = 0.0;
            foreach (Edge edge in edges)
            {
                max = Math.Max(edge.Connectivity, max);
            }
            this._max = max;
        }

        public override double GetLineWeightForEdge(Edge edge)
        {
            return edge.Connectivity;
        }

        public override double GetMaximumLineWeight()
        {
            return _max;
        }
    }
}
