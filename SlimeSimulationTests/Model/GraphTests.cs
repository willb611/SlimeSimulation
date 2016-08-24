using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.FlowCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;

namespace SlimeSimulation.FlowCalculation.Tests
{
    [TestClass()]
    public class GraphTests
    {
        [TestMethod()]
        public void GetEdgeBetween_WhenMultipleEdges_ShouldWork()
        {
            /*
             * a ---- b
             *   \   /
             *     c
             * */
            Node a = new Node(1, 1, 1);
            Node b = new Node(2, 2, 2);
            Node c = new Node(3, 3, 1);
            HashSet<Node> nodes = new HashSet<Node>() { a, b, c };

            SlimeEdge ab = new SlimeEdge(a, b, 1);
            SlimeEdge ac = new SlimeEdge(a, c, 1);
            SlimeEdge bc = new SlimeEdge(b, c, 1);
            HashSet<SlimeEdge> edges = new HashSet<SlimeEdge>() { ac, bc, ab };

            Graph graph = new Graph(edges, nodes);

            SlimeEdge acActual = graph.GetEdgeBetween(a, c);
            Assert.AreEqual(ac, acActual);

            SlimeEdge bcActual = graph.GetEdgeBetween(b, c);
            Assert.AreEqual(bc, bcActual);

            SlimeEdge abActual = graph.GetEdgeBetween(a, b);
            Assert.AreEqual(ab, abActual);
        }
    }
}
