using System;
using System.Collections.Generic;
using SlimeSimulation.Model;
using NLog;
using Cairo;
using Gdk;
using SlimeSimulation.Controller;
using SlimeSimulation.Controller.WindowsComponentController;

namespace SlimeSimulation.View.WindowComponent
{
    public class GraphDrawingArea : Gtk.DrawingArea
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const double MaxLineWidth = 15;

        private const double WindowSpacePercentToDrawIn = 0.9;
        private const double LinePaddingPercent = 0.05;

        private readonly LineViewController _lineWeightController;
        private readonly NodeViewController _nodeHighlightController;
        private readonly ICollection<Edge> _edges = new List<Edge>();
        private readonly ISet<Node> _nodes = new HashSet<Node>();
        private EdgeDrawing _edgeDrawingOption = EdgeDrawing.WithoutWeight;
        private readonly double _maxEdgeWeight = 0;
        private readonly double _maxNodeX = 0;
        private readonly double _maxNodeY = 0;

        private readonly double _minNodeY = double.MaxValue;
        private readonly double _minNodeX = double.MaxValue;

        private double _maxWindowX = 100;
        private double _maxWindowY = 100;

        public GraphDrawingArea(ICollection<Edge> edges, LineViewController lineWidthController,
          NodeViewController nodeHighlightController)
        {
            this._nodeHighlightController = nodeHighlightController;
            foreach (Edge edge in edges)
            {
                AddEdge(edge);
                _maxEdgeWeight = Math.Max(_maxEdgeWeight, edge.Connectivity);
            }
            foreach (Node node in _nodes)
            {
                _maxNodeX = Math.Max(node.X, _maxNodeX);
                _maxNodeY = Math.Max(node.Y, _maxNodeY);

                _minNodeX = Math.Min(node.X, _minNodeX);
                _minNodeY = Math.Min(node.Y, _minNodeY);
            }
            this._lineWeightController = lineWidthController;
            Logger.Debug("[Constructor] Given number of edges: {0}", edges.Count);
        }

        private void AddEdge(Edge edge)
        {
            _edges.Add(edge);
            _nodes.Add(edge.A);
            _nodes.Add(edge.B);
        }

        private void DrawPoint(Cairo.Context graphic, Node node)
        {
            graphic.Save();

            double xscaled = ScaleX(node.X);
            double yscaled = ScaleY(node.Y);
            Logger.Trace("[DrawPoint] Drawing at: {0},{1}", xscaled, yscaled);
            Rgb color = _nodeHighlightController.GetColourForNode(node);
            double size = _nodeHighlightController.GetSizeForNode(node);

            graphic.SetSourceRGB(color.R, color.G, color.B);
            graphic.Rectangle(xscaled - size / 2, yscaled - size / 2, size, size);
            graphic.Fill();
            graphic.Stroke();

            graphic.Restore();
        }

        protected virtual void DrawEdge(Cairo.Context graphic, Edge edge)
        {
            if (Math.Abs(_lineWeightController.GetLineWeightForEdge(edge)) < 0.0001)
            {
                return;
            }
            graphic.Save();
            Logger.Trace("[DrawEdge] Drawing from {0},{1} to {2},{3}", ScaleX(edge.A.X), ScaleY(edge.A.Y),
              ScaleX(edge.B.X), ScaleY(edge.B.Y));

            graphic.MoveTo(ScaleX(edge.A.X), ScaleY(edge.A.Y));
            graphic.SetSourceRGB(0, 0, 0);
            graphic.LineWidth = GetLineWidthForEdge(edge);
            Logger.Trace("[DrawEdge] For edge {0}, using lineWidth: {1}", edge, graphic.LineWidth);
            graphic.LineTo(ScaleX(edge.B.X), ScaleY(edge.B.Y));
            graphic.Stroke();

            graphic.Restore();
            if (_edgeDrawingOption == EdgeDrawing.WithWeight)
            {
                DrawTextNearCoord(graphic, "w:" + String.Format("{0:0.000}", _lineWeightController.GetLineWeightForEdge(edge)),
                  ScaleX((edge.A.X + edge.B.X) / 2), ScaleY(edge.A.Y + edge.B.Y) / 2);
            }
        }

        private void DrawTextNearCoord(Cairo.Context graphic, String s, double x, double y)
        {
            graphic.Save();

            graphic.MoveTo(x - 30, y - 30);
            graphic.ShowText(s);

            graphic.Restore();
        }


        private double ScaleX(double x)
        {
            double percent = (x - _minNodeX) / (_maxNodeX - _minNodeX);
            double availableDrawingSpace = _maxWindowX * WindowSpacePercentToDrawIn;
            double padding = (_maxWindowX - availableDrawingSpace) / 2;
            return availableDrawingSpace * percent + padding;
        }

        private double ScaleY(double y)
        {
            double percent = (y - _minNodeY) / (_maxNodeY - _minNodeY);
            double availableDrawingSpace = _maxWindowY * WindowSpacePercentToDrawIn;
            double padding = (_maxWindowY - availableDrawingSpace) / 2;
            return availableDrawingSpace * percent + padding;
        }

        private double GetLineWidthForEdge(Edge edge)
        {
            double weight = _lineWeightController.GetLineWeightForEdge(edge);
            double percent = weight / _lineWeightController.GetMaximumLineWeight();
            double padding = MaxLineWidth * LinePaddingPercent;
            return percent * (MaxLineWidth - 2 * padding) + padding;
        }

        protected override bool OnExposeEvent(Gdk.EventExpose args)
        {
            Logger.Debug("[OnExposeEvent] Redrawing");
            Gdk.Rectangle allocation = this.Allocation;
            _maxWindowX = allocation.Width;
            _maxWindowY = allocation.Height;
            using (Context g = Gdk.CairoHelper.Create(args.Window))
            {
                Logger.Trace("[OnExposeEvent] Drawing all edges, total #: {0}", _edges.Count);
                foreach (Edge edge in _edges)
                {
                    DrawEdge(g, edge);
                }
                Logger.Trace("[OnExposeEvent] Drawing all nodes, total #: {0}", _nodes.Count);
                foreach (Node node in _nodes)
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
