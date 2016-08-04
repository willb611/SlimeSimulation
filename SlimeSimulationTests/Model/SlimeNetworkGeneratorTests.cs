using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using SlimeSimulation.Model;
using SlimeSimulation.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.View.Tests {
    [TestClass()]
    public class SlimeNetworkGeneratorTests {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [TestMethod()]
        public void UpdateEdgesWithReplacement_ShouldDoReplacement() {
            var slimeNetworkGenerator = new LatticeSlimeNetworkGenerator();
            Node c = new Node(1, 1, 1);
            Node replacement = new Node(155, 155, 155);

            Node a = new Node(2, 2, 2);
            Node b = new Node(3, 3, 3);
            /*
             * a ------- b
             * |        /
             * replace (c)
             */
            Edge ab = new Edge(a, b, 1);
            Edge ac = new Edge(a, c, 1);
            Edge bc = new Edge(b, c, 1);
            HashSet<Edge> edges = new HashSet<Edge>() { ac, ab, bc };

            slimeNetworkGenerator.UpdateEdgesWithReplacement(edges, c, replacement);
            // Check edges connecting to c were removed
            Assert.IsFalse(edges.Contains(ac));
            Assert.IsFalse(edges.Contains(bc));

            edges.Remove(ab);
            // Check edges left are conections from a -> replacement, and b -> replacement
            Assert.IsTrue(edges.Count == 2);
            List<Node> nodesReplacementShouldConnectTo = new List<Node>() { a, b };
            foreach (Edge edge in edges) {
                Node connected = edge.GetOtherNode(replacement);
                nodesReplacementShouldConnectTo.Remove(connected);
            }
            Assert.IsFalse(nodesReplacementShouldConnectTo.Any());
        }
    }
}
