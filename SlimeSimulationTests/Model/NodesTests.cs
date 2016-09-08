using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model.Tests
{
    [TestClass()]
    public class NodesTests
    {
        [TestMethod()]
        public void SortByYAscendingTest()
        {
            Node a = new Node(1, 1, 1);
            Node b = new Node(1, 1, 2);
            Node c = new Node(1, 1, 3);
            var unsortedNodes = new List<Node>() {c, b, a};
            var nodesSortedByYAscending = Nodes.SortByYAscending(unsortedNodes);

            Assert.AreEqual(a, nodesSortedByYAscending.First());
            Assert.AreEqual(c, nodesSortedByYAscending.Last());
        }

        [TestMethod()]
        public void SortByXYAscendingTest()
        {
            Node a = new Node(1, 1, 1);
            Node b = new Node(1, 2, 1);
            Node c = new Node(1, 3, 1);
            var unsortedNodes = new List<Node>() { c, b, a };
            var nodesSortedByXAscending = Nodes.SortByXAscending(unsortedNodes);

            Assert.AreEqual(a, nodesSortedByXAscending.First());
            Assert.AreEqual(c, nodesSortedByXAscending.Last());
        }
    }
}
