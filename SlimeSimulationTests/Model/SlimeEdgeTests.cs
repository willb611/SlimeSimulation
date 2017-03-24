using System;
using SlimeSimulation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SlimeSimulation.Model.Generation;

namespace SlimeSimulation.Model.Tests
{
    [TestClass()]
    public class SlimeEdgeTests
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

        [TestMethod()]
        public void LengthTest()
        {
            var a = new Node(0, 0, 0);
            var b = new Node(1, 1, 0);
            var ab = new SlimeEdge(a, b, 0.1);
            Assert.AreEqual(1, ab.Length());

            var c = new Node(2, 0, 1);
            var ac = new SlimeEdge(a, c, 0.1);
            Assert.AreEqual(1, ac.Length());
        }

        [TestMethod()]
        public void Length_WithPythag()
        {
            var a = new Node(0, 0, 0);
            var b = new Node(1, 3, 4);
            var ab = new SlimeEdge(a, b, 0.1);
            Assert.AreEqual(5, ab.Length());
        }
        [TestMethod()]
        public void Length_DifferentXAndY()
        {
            var a = new Node(0, 4, 0);
            var b = new Node(1, 0, 4);
            var ab = new SlimeEdge(a, b, 0.1);
            Assert.AreEqual(Math.Sqrt(32), ab.Length(), 0.0000001);
        }
    }
}
