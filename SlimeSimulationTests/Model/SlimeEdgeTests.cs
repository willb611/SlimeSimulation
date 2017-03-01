using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SlimeSimulation.Model.Generation;

namespace SlimeSimulation.Model.Tests
{
    [TestClass()]
    public class SlimeEdgesTests
    {
        [TestMethod()]
        public void IsDisconnected_WhenLessThanTolerance()
        {
            Node a = new Node(0, 0, 0);
            Node b = new Node(1, 1, 1);
            var tinyValue = 1e-8;
            var edge = new SlimeEdge(a, b, tinyValue);
            Assert.IsTrue(edge.IsDisconnected());
        }

        [TestMethod()]
        public void IsDisconnected_WhenAboveTolerance()
        {
            Node a = new Node(0, 0, 0);
            Node b = new Node(1, 1, 1);
            var smallValue = 1e-5;
            var edge = new SlimeEdge(a, b, smallValue);
            Assert.IsFalse(edge.IsDisconnected());
        }

    }
}
