using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model.Tests {
    [TestClass()]
    public class DijkstrasTests {
        [TestMethod()]
        public void GetShortestPathToNodesTest() {
            Node source = new Node(1, 1, 1);
            Node a = new Node(2, 1, 2);
            Node b = new Node(3, 2, 1);
            Node sink = new Node(4, 2, 2);

            Edge srca = new Edge(source, a, 2);
            Edge srcb = new Edge(source, b, 2);
            Edge asink = new Edge(a, sink, 2);
            Edge bsink = new Edge(b, sink, 2);
            List<Edge> edges = new List<Edge>() { srca, srcb, asink, bsink };

            Graph graph = new Graph(edges);
            /*
             * b      -  sink
             * |        |
             * source -  a
            */
            SortedDictionary<int, List<Node>> distance = Dijkstras.GetShortestPathToNodes(source, graph);

            var nodesAtSource = distance[0];
            Assert.AreEqual(1, nodesAtSource.Count);
            Assert.AreEqual(source, nodesAtSource.First());

            var nodesStepAway = distance[1];
            Assert.AreEqual(2, nodesStepAway.Count);
            Assert.IsTrue(nodesStepAway.Contains(a));
            Assert.IsTrue(nodesStepAway.Contains(b));

            
            var nodesAtSink = distance[2];
            Assert.AreEqual(1, nodesAtSink.Count);
            Assert.AreEqual(sink, nodesAtSink.First());
        }
    }
}
