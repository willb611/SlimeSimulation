using SlimeSimulation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace SlimeSimulation.Model.Tests
{
    [TestClass()]
    public class NodeTests
    {

        [TestMethod()]
        public void UpdateEdgesWithReplacement_ShouldDoReplacement()
        {
            var c = new Node(1, 1, 1);
            var replacement = new Node(155, 155, 155);

            var a = new Node(2, 2, 2);
            var b = new Node(3, 3, 3);
            /*
             * a ------- b
             * |        /
             * replace (c)
             */
            var ab = new Edge(a, b);
            var ac = new Edge(a, c);
            var bc = new Edge(b, c);
            var edges = new HashSet<Edge>() { ac, ab, bc };

            c.ReplaceWithGivenNodeInEdges(replacement, edges);
            // Check edges connecting to c were removed
            Assert.IsFalse(edges.Contains(ac));
            Assert.IsFalse(edges.Contains(bc));

            edges.Remove(ab);
            // Check edges left are conections from a -> replacement, and b -> replacement
            Assert.IsTrue(edges.Count == 2);
            var nodesReplacementShouldConnectTo = new List<Node>() { a, b };
            foreach (var edge in edges)
            {
                var connected = edge.GetOtherNode(replacement);
                nodesReplacementShouldConnectTo.Remove(connected);
            }
            Assert.IsFalse(nodesReplacementShouldConnectTo.Any());
        }

        [TestMethod()]
        public void CompareTo_ById()
        {
            Node a = new Node(1, 1, 1);
            Node b = new Node(2, 1, 1);
            Assert.IsTrue(a.CompareTo(b) < 0);
            Assert.IsTrue(b.CompareTo(a) > 0);
        }

        [TestMethod()]
        public void CompareTo_ByX()
        {
            Node a = new Node(1, 1, 1);
            Node b = new Node(1, 2, 1);
            Assert.IsTrue(a.CompareTo(b) < 0);
            Assert.IsTrue(b.CompareTo(a) > 0);
        }

        [TestMethod()]
        public void CompareTo_ByY()
        {
            Node a = new Node(1, 1, 1);
            Node b = new Node(1, 1, 2);
            Assert.IsTrue(a.CompareTo(b) < 0);
            Assert.IsTrue(b.CompareTo(a) > 0);
        }

        [TestMethod()]
        public void CompareTo_WhenEqual()
        {
            Node a = new Node(1, 1, 1);
            Node b = new Node(1, 1, 1);
            Assert.AreEqual(0, a.CompareTo(b));
        }
    }
}
