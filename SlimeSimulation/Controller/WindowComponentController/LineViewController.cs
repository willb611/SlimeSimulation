using System;
using System.Collections.Generic;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;
using SlimeSimulation.View;

namespace SlimeSimulation.Controller.WindowComponentController
{
    public abstract class LineViewController
    {
        public abstract double GetLineWeightForEdge(Edge edge);
        public abstract double GetMaximumLineWeight();
        public abstract Rgb GetColourForEdge(Edge edge);
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

        public override Rgb GetColourForEdge(Edge edge)
        {
            return Rgb.Black;
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
        private List<SlimeEdge> _edges;
        private readonly double _max = 0.0;
        private readonly double _weightForNonSlimeEdge;

        public ConnectivityLineViewController(List<SlimeEdge> edges)
        {
            _edges = edges;
            foreach (var edge in edges)
            {
                _max = Math.Max(edge.Connectivity, _max);
            }
            _weightForNonSlimeEdge = _max/4;
        }

        public Rgb SlimeColour => Rgb.Yellow;
        public Rgb NonSlimeColour => Rgb.Black;

        public override Rgb GetColourForEdge(Edge edge)
        {
            var slimeEdge = edge as SlimeEdge;
            return slimeEdge == null ? NonSlimeColour : SlimeColour;
        }

        public override double GetLineWeightForEdge(Edge edge)
        {
            var slimeEdge = edge as SlimeEdge;
            return slimeEdge?.Connectivity ?? _weightForNonSlimeEdge;
        }

        public override double GetMaximumLineWeight()
        {
            return _max;
        }
    }
}
