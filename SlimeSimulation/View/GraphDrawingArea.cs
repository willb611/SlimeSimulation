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

        private List<Edge> edges = new List<Edge>();
        private HashSet<Node> nodes = new HashSet<Node>();

        public GraphDrawingArea(List<Edge> edges) {
            foreach (Edge edge in edges) {
                AddEdge(edge);
            }
            logger.Debug("[Constructor] Given number of edges: " + edges.Count);
        }

        private void AddEdge(Edge edge) {
            edges.Add(edge);
            nodes.Add(edge.A);
            nodes.Add(edge.B);
        }


        private void DrawPoint(Cairo.Context graphic, double x, double y) {
            graphic.Save();
            logger.Debug("Drawing a point at: {0},{1}", x, y);

            graphic.SetSourceRGB(255, 0, 0);
            graphic.Rectangle(x, y, 1, 1);
            graphic.Stroke();

            graphic.Restore();
        }

        private void DrawEdge(Cairo.Context graphic, Edge edge) {
            graphic.Save();
            logger.Debug("Drawing a line from {0},{1} to {2},{3}", edge.A.X, edge.A.Y,
                edge.B.X, edge.B.Y);

            graphic.MoveTo(edge.A.X, edge.A.Y);
            graphic.SetSourceRGB(0, 0, 0);
            graphic.LineTo(edge.B.X, edge.B.Y);
            graphic.LineWidth = 7;
            graphic.Stroke();

            graphic.Restore();
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
                    DrawPoint(g, node.X, node.Y);
                }
            }
            return true;
        }
    }
}
