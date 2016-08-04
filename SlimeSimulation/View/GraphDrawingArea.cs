using System;
using System.Collections.Generic;
using SlimeSimulation.Model;
using NLog;
using Cairo;
using Gdk;

namespace SlimeSimulation.View {
    public class GraphDrawingArea : Gtk.DrawingArea {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly double MAX_LINE_WIDTH = 15;
        private readonly double WINDOW_SPACE_PERCENT_TO_DRAW_IN = 0.9;

        private readonly LineWeightController lineWeightController;
        private NodeHighlightController nodeHighlightController;
        private readonly ICollection<Edge> edges = new List<Edge>();
        private readonly ISet<Node> nodes = new HashSet<Node>();
        private EdgeDrawing edgeDrawingOption = EdgeDrawing.WithoutWeight;
        private double maxEdgeWeight = 0;
        private double maxNodeX = 0;
        private double maxNodeY = 0;

        private double minNodeY = double.MaxValue;
        private double minNodeX = double.MaxValue;

        private double maxWindowX = 100;
        private double maxWindowY = 100;

        public GraphDrawingArea(ICollection<Edge> edges, LineWeightController lineWidthController,
            NodeHighlightController nodeHighlightController) {
            this.nodeHighlightController = nodeHighlightController;
            foreach (Edge edge in edges) {
                AddEdge(edge);
                maxEdgeWeight = Math.Max(maxEdgeWeight, edge.Connectivity);
            }
            foreach (Node node in nodes) {
                maxNodeX = Math.Max(node.X, maxNodeX);
                maxNodeY = Math.Max(node.Y, maxNodeY);

                minNodeX = Math.Min(node.X, minNodeX);
                minNodeY = Math.Min(node.Y, minNodeY);
            }
            this.lineWeightController = lineWidthController;
            logger.Debug("[Constructor] Given number of edges: " + edges.Count);
        }
        private void AddEdge(Edge edge) {
            edges.Add(edge);
            nodes.Add(edge.A);
            nodes.Add(edge.B);
        }

        private void DrawPoint(Cairo.Context graphic, Node node) {
            graphic.Save();

            double xscaled = ScaleX(node.X);
            double yscaled = ScaleY(node.Y);
            logger.Trace("[DrawPoint] Drawing at: {0},{1}", xscaled, yscaled);
            RGB color = nodeHighlightController.GetColourForNode(node);
            double size = nodeHighlightController.GetSizeForNode(node);

            graphic.SetSourceRGB(color.R, color.G, color.B);
            graphic.Rectangle(xscaled - size / 2, yscaled - size/2, size, size);
            graphic.Fill();
            graphic.Stroke();

            graphic.Restore();
        }

        virtual protected void DrawEdge(Cairo.Context graphic, Edge edge) {
            graphic.Save();
            logger.Trace("[DrawEdge] Drawing from {0},{1} to {2},{3}", ScaleX(edge.A.X), ScaleY(edge.A.Y),
                ScaleX(edge.B.X), ScaleY(edge.B.Y));

            graphic.MoveTo(ScaleX(edge.A.X), ScaleY(edge.A.Y));
            graphic.SetSourceRGB(0, 0, 0);
            graphic.LineWidth = GetLineWidthForEdge(edge);
            logger.Trace("[DrawEdge] For edge " + edge + ", using lineWidth: " + graphic.LineWidth);
            graphic.LineTo(ScaleX(edge.B.X), ScaleY(edge.B.Y));
            graphic.Stroke();

            graphic.Restore();
            if (edgeDrawingOption == EdgeDrawing.WithWeight) {
                DrawTextNearCoord(graphic, "w:" + String.Format("{0:0.000}", lineWeightController.GetLineWeightForEdge(edge)),
                            ScaleX((edge.A.X + edge.B.X) / 2), ScaleY(edge.A.Y + edge.B.Y) / 2);
            }
        }
        
        private void DrawTextNearCoord(Cairo.Context graphic, String s, double x, double y) {
           graphic.Save();

           graphic.MoveTo(x - 30, y - 30);
           graphic.ShowText(s);

           graphic.Restore();
        }


        private double ScaleX(double x) {
            double percent = (x - minNodeX) / (maxNodeX - minNodeX);
            double availableDrawingSpace = maxWindowX * WINDOW_SPACE_PERCENT_TO_DRAW_IN;
            double padding = (maxWindowX - availableDrawingSpace) / 2;
            return availableDrawingSpace * percent + padding;
        }

        private double ScaleY(double y) {
            double percent = (y - minNodeY) / (maxNodeY - minNodeY);
            double availableDrawingSpace = maxWindowY  * WINDOW_SPACE_PERCENT_TO_DRAW_IN;
            double padding = (maxWindowY - availableDrawingSpace) / 2;
            return availableDrawingSpace * percent + padding;
        }

        private double GetLineWidthForEdge(Edge edge) {
            double weight = lineWeightController.GetLineWeightForEdge(edge);
            double percent = weight / lineWeightController.GetMaximumLineWeight();
            double padding = MAX_LINE_WIDTH * 0.1;
            return percent * (MAX_LINE_WIDTH - 2 * padding) + padding;
        }

        protected override bool OnExposeEvent(Gdk.EventExpose args) {
            logger.Debug("[OnExposeEvent] Redrawing");
            Gdk.Rectangle allocation = this.Allocation;
            maxWindowX = allocation.Width;
            maxWindowY = allocation.Height;
            using (Context g = Gdk.CairoHelper.Create(args.Window)) {
                logger.Trace("[OnExposeEvent] Drawing all edges, total #: " + edges.Count);
                foreach (Edge edge in edges) {
                    DrawEdge(g, edge);
                }
                logger.Trace("[OnExposeEvent] Drawing all nodes, total #: " + nodes.Count);
                foreach (Node node in nodes) {
                    if (node.IsFoodSource()) {
                        DrawPoint(g, node);
                    } else {
                        DrawPoint(g, node);
                    }
                }
            }
            return true;
        }

        public void InvertEdgeDrawing() {
            if (edgeDrawingOption == EdgeDrawing.WithoutWeight) {
                edgeDrawingOption = EdgeDrawing.WithWeight;
            } else {
                edgeDrawingOption = EdgeDrawing.WithoutWeight;
            }
            QueueDraw();
        }
    }


    public enum EdgeDrawing {
        WithWeight, WithoutWeight
    }
}
