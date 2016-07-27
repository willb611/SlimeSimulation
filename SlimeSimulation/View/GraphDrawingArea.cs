using System;
using System.Collections.Generic;
using SlimeSimulation.Model;
using NLog;
using Cairo;

namespace SlimeSimulation.View {
    public class GraphDrawingArea : Gtk.DrawingArea {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly double MAX_LINE_WIDTH = 15;
        private readonly double FOOD_SOURCE_POINT_SIZE = 15;
        private readonly double WINDOW_SPACE_PERCENT_TO_DRAW_IN = 0.9;

        private readonly LineWeightController lineWeightController;
        private readonly ICollection<Edge> edges = new List<Edge>();
        private readonly ISet<Node> nodes = new HashSet<Node>();
        private double maxEdgeWeight = 0;
        private double maxNodeX = 0;
        private double maxNodeY = 0;

        private double maxWindowX = 100;
        private double maxWindowY = 100;

        public GraphDrawingArea(ICollection<Edge> edges, LineWeightController lineWidthController) {
            foreach (Edge edge in edges) {
                AddEdge(edge);
                maxEdgeWeight = Math.Max(maxEdgeWeight, edge.Connectivity);
            }
            foreach (Node node in nodes) {
                maxNodeX = Math.Max(node.X, maxNodeX);
                maxNodeY = Math.Max(node.Y, maxNodeY);
            }
            this.lineWeightController = lineWidthController;
            logger.Debug("[Constructor] Given number of edges: " + edges.Count);
        }

        private void AddEdge(Edge edge) {
            edges.Add(edge);
            nodes.Add(edge.A);
            nodes.Add(edge.B);
        }


        private void DrawPoint(Cairo.Context graphic, double x, double y, double size) {
            graphic.Save();
            double xscaled = ScaleX(x);
            double yscaled = ScaleY(y);
            logger.Trace("[DrawPoint] Drawing at: {0},{1}", xscaled, yscaled);

            graphic.SetSourceRGB(255, 0, 0);
            graphic.Rectangle(xscaled - size / 2, yscaled - size/2, size, size);
            graphic.Fill();
            graphic.Stroke();

            graphic.Restore();
        }

        private void DrawEdge(Cairo.Context graphic, Edge edge) {
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
        }

        private double ScaleX(double x) {
            double percent = x / maxNodeX;
            double spaceToUse = maxWindowX * WINDOW_SPACE_PERCENT_TO_DRAW_IN;
            return spaceToUse * percent;
        }

        private double ScaleY(double y) {
            double percent = y / maxNodeY;
            double spaceToUse = maxWindowY  * WINDOW_SPACE_PERCENT_TO_DRAW_IN;
            return spaceToUse * percent;
        }

        private double GetLineWidthForEdge(Edge edge) {
            double weight = lineWeightController.GetLineWeightForEdge(edge);
            double percent = weight / lineWeightController.GetMaximumLineWeight();
            return percent * MAX_LINE_WIDTH;
        }

        protected override bool OnExposeEvent(Gdk.EventExpose args) {
            logger.Debug("[OnExposeEvent] Entered");
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
                        DrawPoint(g, node.X, node.Y, FOOD_SOURCE_POINT_SIZE);
                    } else {
                        DrawPoint(g, node.X, node.Y, 3);
                    }
                }
            }
            return true;
        }
    }
}
