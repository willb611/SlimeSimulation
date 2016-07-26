using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;
using NLog;
using Cairo;

namespace SlimeSimulation.View {
    public class GraphDrawingArea : Gtk.DrawingArea {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly double MAX_LINE_WIDTH = 20;
        private readonly double FOOD_SOURCE_POINT_SIZE = 20;

        private List<Edge> edges = new List<Edge>();
        private HashSet<Node> nodes = new HashSet<Node>();
        private double maxEdgeWeight = 0;
        private readonly LineWidthController lineWidthController;

        public GraphDrawingArea(List<Edge> edges, LineWidthController lineWidthController) {
            foreach (Edge edge in edges) {
                AddEdge(edge);
                maxEdgeWeight = Math.Max(maxEdgeWeight, edge.Connectivity);
            }
            this.lineWidthController = lineWidthController;
            logger.Debug("[Constructor] Given number of edges: " + edges.Count);
        }

        private void AddEdge(Edge edge) {
            edges.Add(edge);
            nodes.Add(edge.A);
            nodes.Add(edge.B);
        }


        private void DrawPoint(Cairo.Context graphic, double x, double y, double size) {
            graphic.Save();
            logger.Trace("Drawing a point at: {0},{1}", x, y);

            graphic.SetSourceRGB(255, 0, 0);
            graphic.Rectangle(x - size / 2, y - size/2, size, size);
            graphic.Fill();
            graphic.Stroke();

            graphic.Restore();
        }

        private void DrawEdge(Cairo.Context graphic, Edge edge) {
            graphic.Save();
            logger.Trace("Drawing a line from {0},{1} to {2},{3}", edge.A.X, edge.A.Y,
                edge.B.X, edge.B.Y);

            graphic.MoveTo(edge.A.X, edge.A.Y);
            graphic.SetSourceRGB(0, 0, 0);
            graphic.LineWidth = GetLineWidthForEdge(edge);
            logger.Debug("For edge " + edge + ", using lineWidth: " + graphic.LineWidth);
            graphic.LineTo(edge.B.X, edge.B.Y);
            graphic.Stroke();

            graphic.Restore();
        }

        private double GetLineWidthForEdge(Edge edge) {
            double width = lineWidthController.GetLineWidthForEdge(edge);
            double percent = width / lineWidthController.GetMaximumLineWidth();
            return percent * MAX_LINE_WIDTH;
        }

        protected override bool OnExposeEvent(Gdk.EventExpose args) {
            logger.Debug("[OnExposeEvent] Entered");
            using (Context g = Gdk.CairoHelper.Create(args.Window)) {
                logger.Debug("Drawing all edges, total #: " + edges.Count);
                foreach (Edge edge in edges) {
                    DrawEdge(g, edge);
                }
                logger.Debug("Drawing all nodes, total #: " + nodes.Count);
                foreach (Node node in nodes) {
                    if (node.IsFoodSource()) {
                        DrawPoint(g, node.X, node.Y, FOOD_SOURCE_POINT_SIZE);
                    } else {
                        DrawPoint(g, node.X, node.Y, 1);
                    }
                }
            }
            return true;
        }
    }
}
