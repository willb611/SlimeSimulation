using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Algorithms.Pathing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;

namespace SlimeSimulation.Algorithms.Pathing.Tests
{
    [TestClass()]
    public class PathFinderTests
    {
        [TestMethod()]
        public void FindPathTest()
        {
            /*
             * A-B
             */
            var a = new Node(1, 1, 1);
            var b = new Node(2, 2, 2);
            var ab = new Edge(a, b);
            var graph = new Graph(new HashSet<Edge>() {ab});

            var pathFinder = new PathFinder();
            var path = pathFinder.FindPath(graph, new Route(a, b));
            Assert.AreEqual(2, path.NodesInPathCount());
            Assert.AreEqual(0, path.IntermediateNodesInPathCount());
        }
    }
}