using System;
using System.Collections.Generic;
using Cairo;
using Gdk;
using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowComponentController;
using SlimeSimulation.Model;

namespace SlimeSimulation.View.WindowComponent
{
    public class GraphDrawingArea : DrawingArea
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const double MaxLineWidth = 8;

        private const double WindowSpacePercentToDrawIn = 0.9;
        private const double LinePaddingPercent = 0.05;

        private const double MinEdgeWeightToDraw = 0.0001;

        private readonly LineViewController _lineWeightController;
        private readonly NodeViewController _nodeHighlightController;
        private readonly ICollection<Edge> _edges = new List<Edge>();
        private readonly ISet<Node> _nodes = new HashSet<Node>();
        private EdgeDrawing _edgeDrawingOption = EdgeDrawing.WithoutWeight;
        private readonly double _maxNodeX;
        private readonly double _maxNodeY;

        private readonly double _minNodeY = double.MaxValue;
        private readonly double _minNodeX = double.MaxValue;

        private double _maxWindowX = 100;
        private double _maxWindowY = 100;


        public GraphDrawingArea(ICollection<Edge> edges, LineViewController lineWidthController,
          NodeViewController nodeHighlightController)
        {
            _nodeHighlightController = nodeHighlightController;
            foreach (var edge in edges)
            {
                AddEdge(edge);
            }
            foreach (var node in _nodes)
            {
                _maxNodeX = Math.Max(node.X, _maxNodeX);
                _maxNodeY = Math.Max(node.Y, _maxNodeY);

                _minNodeX = Math.Min(node.X, _minNodeX);
                _minNodeY = Math.Min(node.Y, _minNodeY);
            }
            _lineWeightController = lineWidthController;
            Logger.Debug("[Constructor] Given number of edges: {0}", edges.Count);
        }

        private void AddEdge(Edge slimeEdge)
        {
            _edges.Add(slimeEdge);
            _nodes.Add(slimeEdge.A);
            _nodes.Add(slimeEdge.B);
        }

        private void DrawPoint(Context graphic, Node node)
        {
            graphic.Save();

            var xscaled = ScaleX(node.X);
            var yscaled = ScaleY(node.Y);
            Logger.Trace("[DrawPoint] Drawing at: {0},{1}", xscaled, yscaled);
            var color = _nodeHighlightController.GetColourForNode(node);
            double size = _nodeHighlightController.GetSizeForNode(node);

            graphic.SetSourceRGB(color.R, color.G, color.B);
            graphic.Rectangle(xscaled - size / 2, yscaled - size / 2, size, size);
            graphic.Fill();
            graphic.Stroke();

            graphic.Restore();
        }

        protected virtual void DrawEdge(Context graphic, Edge edge)
        {
            if (Math.Abs(_lineWeightController.GetLineWeightForEdge(edge)) < MinEdgeWeightToDraw)
            {
                return;
            }
            graphic.Save();
            Logger.Trace("[DrawEdge] Drawing from {0},{1} to {2},{3}", ScaleX(edge.A.X), ScaleY(edge.A.Y),
              ScaleX(edge.B.X), ScaleY(edge.B.Y));

            graphic.MoveTo(ScaleX(edge.A.X), ScaleY(edge.A.Y));
            var color = _lineWeightController.GetColourForEdge(edge);
            graphic.SetSourceRGB(color.R, color.G, color.B);
            graphic.LineWidth = GetLineWidthForEdge(edge);
            Logger.Trace("[DrawEdge] For SlimeEdge {0}, using lineWidth: {1}", edge, graphic.LineWidth);
            graphic.LineTo(ScaleX(edge.B.X), ScaleY(edge.B.Y));
            graphic.Stroke();

            graphic.Restore();
            if (_edgeDrawingOption == EdgeDrawing.WithWeight)
            {
                DrawTextNearCoord(graphic, "w:" + String.Format("{0:0.000}", _lineWeightController.GetLineWeightForEdge(edge)),
                  ScaleX((edge.A.X + edge.B.X) / 2), ScaleY(edge.A.Y + edge.B.Y) / 2);
            }
        }

        private void DrawTextNearCoord(Context graphic, String s, double x, double y)
        {
            graphic.Save();

            graphic.MoveTo(x - 30, y - 30);
            graphic.ShowText(s);

            graphic.Restore();
        }


        private double ScaleX(double x)
        {
            var percent = (x - _minNodeX) / (_maxNodeX - _minNodeX);
            var availableDrawingSpace = _maxWindowX * WindowSpacePercentToDrawIn;
            var padding = (_maxWindowX - availableDrawingSpace) / 2;
            return availableDrawingSpace * percent + padding;
        }

        private double ScaleY(double y)
        {
            var percent = (y - _minNodeY) / (_maxNodeY - _minNodeY);
            var availableDrawingSpace = _maxWindowY * WindowSpacePercentToDrawIn;
            var padding = (_maxWindowY - availableDrawingSpace) / 2;
            return availableDrawingSpace * percent + padding;
        }

        private double GetLineWidthForEdge(Edge slimeEdge)
        {
            var weight = _lineWeightController.GetLineWeightForEdge(slimeEdge);
            var percent = weight / _lineWeightController.GetMaximumLineWeight();
            var padding = MaxLineWidth * LinePaddingPercent;
            return percent * (MaxLineWidth - 2 * padding) + padding;
        }

        protected override bool OnExposeEvent(EventExpose args)
        {
            Logger.Debug("[OnExposeEvent] Redrawing");
            var allocation = Allocation;
            _maxWindowX = allocation.Width;
            _maxWindowY = allocation.Height;
            using (var g = CairoHelper.Create(args.Window))
            {
                Logger.Trace("[OnExposeEvent] Drawing all edges, total #: {0}", _edges.Count);
                foreach (var edge in _edges)
                {
                    DrawEdge(g, edge);
                }
                Logger.Trace("[OnExposeEvent] Drawing all nodes, total #: {0}", _nodes.Count);
                foreach (var node in _nodes)
                {
                    if (node.IsFoodSource())
                    {
                        DrawPoint(g, node);
                    }
                    else
                    {
                        DrawPoint(g, node);
                    }
                }
            }
            return true;
        }

        public void InvertEdgeDrawing()
        {
            if (_edgeDrawingOption == EdgeDrawing.WithoutWeight)
            {
                _edgeDrawingOption = EdgeDrawing.WithWeight;
            }
            else
            {
                _edgeDrawingOption = EdgeDrawing.WithoutWeight;
            }
            QueueDraw();
        }
    }


    public enum EdgeDrawing
    {
        WithWeight,
        WithoutWeight
    }
}
