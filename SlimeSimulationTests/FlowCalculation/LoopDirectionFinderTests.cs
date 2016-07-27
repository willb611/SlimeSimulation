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
            HashSet<Node> nodes = new HashSet<Node>() { source, a, b, sink };

            Edge srca = new Edge(source, a, 2);
            Edge srcb = new Edge(source, b, 2);
            Edge asink = new Edge(a, sink, 2);
            Edge bsink = new Edge(b, sink, 2);
            ISet<Edge> edges = new HashSet<Edge>() { srca, srcb, asink, bsink };

            HashSet<Loop> loops = new HashSet<Loop>() { new Loop(nodes, edges) };
            HashSet<Edge> asideEdge = new HashSet<Edge>() { srca, asink };
            HashSet<Edge> bsideEdge = new HashSet<Edge>() { srcb, bsink };
            /*
             * b      -  sink
             * |        |
             * source -  a
            */
            LoopDirectionFinder loopDirectionFinder = new LoopDirectionFinder();
            Graph graph = new Graph(edges);
            ISet<LoopWithDirectionOfFlow> loopsWithDirection = loopDirectionFinder.GetLoopsWithDirectionForFlow(loops, source, sink, graph);

            LoopWithDirectionOfFlow actualLoopWithDirection = loopsWithDirection.First();
            if (ListEquals(actualLoopWithDirection.Clockwise, asideEdge)) {
                Assert.IsTrue(ListEquals(bsideEdge, actualLoopWithDirection.AntiClockwise));
            } else if (ListEquals(actualLoopWithDirection.AntiClockwise, asideEdge)) {
                Assert.IsTrue(ListEquals(bsideEdge, actualLoopWithDirection.Clockwise));
            } else {
                logger.Error("Aside not matched. Aside: ");
                logger.Error(LogHelper.CollectionToString(asideEdge));
                logger.Error("Clockwise: ");
                logger.Error(LogHelper.CollectionToString(actualLoopWithDirection.Clockwise));
                logger.Error("Anticlockwise: ");
                logger.Error(LogHelper.CollectionToString(actualLoopWithDirection.AntiClockwise));
                Assert.Fail("Calculated loop with direction was wrong: " + actualLoopWithDirection);
            }
        }

        private bool ListEquals(ICollection<Edge> a, ICollection<Edge> b) {
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
