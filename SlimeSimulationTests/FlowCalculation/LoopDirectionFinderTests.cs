using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.FlowCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using SlimeSimulation.Model;

namespace SlimeSimulation.FlowCalculation.Tests {
    [TestClass()]
    public class LoopDirectionFinderTests {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [TestMethod()]
        public void GetLoopsWithDirectionForFlow_WhenSimpleLoop_ShouldWork() {
            Node source = new Node(1, 1, 1);
            Node a = new Node(2, 1, 2);
            Node b = new Node(3, 2, 1);
            Node sink = new Node(4, 2, 2);
            List<Node> nodes = new List<Node>() { source, a, b, sink };

            Edge srca = new Edge(source, a, 2);
            Edge srcb = new Edge(source, b, 2);
            Edge asink = new Edge(a, sink, 2);
            Edge bsink = new Edge(b, sink, 2);
            List<Edge> edges = new List<Edge>() { srca, srcb, asink, bsink };

            List<Loop> loops = new List<Loop>() { new Loop(nodes, edges) };
            List<Edge> asideEdge = new List<Edge>() { srca, asink };
            List<Edge> bsideEdge = new List<Edge>() { srcb, bsink };
            /*
             * b      -  sink
             * |        |
             * source -  a
            */
            LoopDirectionFinder loopDirectionFinder = new LoopDirectionFinder();
            Graph graph = new Graph(edges);
            List<LoopWithDirectionOfFlow> loopsWithDirection = loopDirectionFinder.GetLoopsWithDirectionForFlow(loops, source, sink, graph);

            LoopWithDirectionOfFlow actualLoopWithDirection = loopsWithDirection.First();
            if (ListEquals(actualLoopWithDirection.Clockwise, asideEdge)) {
                Assert.IsTrue(ListEquals(bsideEdge, actualLoopWithDirection.AntiClockwise));
            } else if (ListEquals(actualLoopWithDirection.AntiClockwise, asideEdge)) {
                Assert.IsTrue(ListEquals(bsideEdge, actualLoopWithDirection.Clockwise));
            } else {
                logger.Error("Aside not matched. Aside: ");
                logger.Error(LogHelper.ListToString(asideEdge));
                logger.Error("Clockwise: ");
                logger.Error(LogHelper.ListToString(actualLoopWithDirection.Clockwise));
                logger.Error("Anticlockwise: ");
                logger.Error(LogHelper.ListToString(actualLoopWithDirection.AntiClockwise));
                Assert.Fail("Calculated loop with direction was wrong: " + actualLoopWithDirection);
            }
        }

        private bool ListEquals(List<Edge> a, List<Edge> b) {
            bool ret = true;
            foreach (Edge edge in a) {
                if (!b.Contains(edge)) {
                    ret = false;
                }
            }
            if (b.Count != a.Count) {
                ret = false;
            }
            return ret;
        }

    }
}
