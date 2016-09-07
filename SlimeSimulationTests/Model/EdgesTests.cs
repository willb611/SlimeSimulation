using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model.Generation;

namespace SlimeSimulation.Model.Tests
{
    [TestClass()]
    public class EdgesTests
    {
        [TestMethod()]
        public void FromSlimeEdges()
        {
            var slimeEdges =
                new SlimeNetworkGenerator().FromGraphWithFoodSources(
                    new LatticeGraphWithFoodSourcesGenerator().Generate()).SlimeEdges;

            var edges = Edges.FromSlimeEdges(slimeEdges);
            foreach (var slimeEdge in slimeEdges)
            {
                Assert.IsTrue(edges.Contains(slimeEdge.Edge) || edges.Contains(slimeEdge));
            }
        }

        [TestMethod()]
        public void CastToSlimeEdgesTest()
        {
            Node a = new Node(1, 1, 1);
            var b = new Node(2, 2, 2);
            var c = new Node(3, 3, 3);
            var ab = new Edge(a, b);
            var acSlime = new SlimeEdge(a, c, 1);
            var bcSlime = new SlimeEdge(b, c, 1);

            var edges = new HashSet<Edge>() { ab, acSlime, bcSlime };
            var slimeEdges = Edges.CastToSlimeEdges(edges);
            Assert.IsNotNull(slimeEdges);
            Assert.IsTrue(slimeEdges.Contains(acSlime));
            Assert.IsTrue(slimeEdges.Contains(bcSlime));
        }

        [TestMethod()]
        public void GetMaxNodeIdTest()
        {
            int maxNodeId = 5;
            Node a = new Node(1, 1, 1);
            var b = new Node(2, 2, 2);
            var c = new Node(maxNodeId, 3, 3);
            var ab = new Edge(a, b);
            var acSlime = new SlimeEdge(a, c, 1);
            var bcSlime = new SlimeEdge(b, c, 1);
            
            var edges = new HashSet<Edge>() { ab, acSlime, bcSlime };
            int actualMaxNodeId = Edges.GetMaxNodeId(edges);
            Assert.AreEqual(maxNodeId, actualMaxNodeId);
        }

        [TestMethod()]
        public void GetNodesContainedIn_ShouldReturnAllNodes()
        {
            Node a = new Node(1, 1, 1);
            var b = new Node(2, 2, 2);
            var c = new Node(3, 3, 3);
            var ab = new Edge(a, b);
            var acSlime = new SlimeEdge(a, c, 1);
            var bcSlime = new SlimeEdge(b, c, 1);
            var edges = new HashSet<Edge>() { ab, acSlime, bcSlime };

            var expected = new HashSet<Node>() {a,b,c};
            var nodes = Edges.GetNodesContainedIn(edges);
            
            Assert.IsTrue(expected.SetEquals(nodes));
        }
    }
}
