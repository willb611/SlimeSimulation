using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.FlowCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;

namespace SlimeSimulation.Model.Tests
{
    [TestClass()]
    public class GraphTests
    {
        [TestMethod()]
        public void EqualsTest()
        {
            var a = new Node(1, 1, 1);
            var b = new Node(2, 2, 2);
            var c = new Node(3, 3, 3);
            var nodes = new HashSet<Node>() {a, b, c};

            var ab = new Edge(a, b);
            var bc = new Edge(b, c);
            var ac = new Edge(a, c);
            var edges = new HashSet<Edge>() {ab, bc, ac};

            var graph = new Graph(edges, nodes);
            var other = new Graph(edges, nodes);
            Assert.AreEqual(graph, other);
        }

        [TestMethod()]
        public void GetEdgeBetween_WhenMultipleEdges_ShouldWork()
        {
            /*
             * a ---- b
             *   \   /
             *     c
             * */
            var a = new Node(1, 1, 1);
            var b = new Node(2, 2, 2);
            var c = new Node(3, 3, 1);
            var nodes = new HashSet<Node>() { a, b, c };

            var ab = new Edge(a, b);
            var ac = new Edge(a, c);
            var bc = new Edge(b, c);
            var edges = new HashSet<Edge>() { ac, bc, ab };

            var graph = new Graph(edges, nodes);

            var acActual = graph.GetEdgeBetween(a, c);
            Assert.AreEqual(ac, acActual);

            var bcActual = graph.GetEdgeBetween(b, c);
            Assert.AreEqual(bc, bcActual);

            var abActual = graph.GetEdgeBetween(a, b);
            Assert.AreEqual(ab, abActual);
        }
    }
}
