using System;
using System.Collections.Generic;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;
using SlimeSimulation.View;
using SlimeSimulation.View.WindowComponent;

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

    internal class SlimeLineViewController : LineViewController
    {
        private readonly double _maxSlimeEdgeConnectivity;
        private readonly double _weightForNonSlimeEdge = GraphDrawingArea.MinEdgeWeightToDraw;

        public SlimeLineViewController(IEnumerable<Edge> edges)
        {
            foreach (var edge in edges)
            {
                var slimeEdge = edge as SlimeEdge;
                var slimeEdgeConnectivity = slimeEdge?.Connectivity ?? 0.0;
                _maxSlimeEdgeConnectivity = Math.Max(slimeEdgeConnectivity, _maxSlimeEdgeConnectivity);
            }
            if (_maxSlimeEdgeConnectivity == 0)
            {
                _weightForNonSlimeEdge = 1;
            }
        }

        public Rgb SlimeColour => SlimeNodeViewController.SlimeNodeColour;
        public Rgb NonSlimeColour => SlimeNodeViewController.NormalNodeColour;

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
            if (_maxSlimeEdgeConnectivity == 0)
            {
                return _weightForNonSlimeEdge;
            }
            else
            {
                return _maxSlimeEdgeConnectivity;
            }
        }
    }
}
