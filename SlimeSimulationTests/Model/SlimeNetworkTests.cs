using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using SlimeSimulation.Model.Generation;

namespace SlimeSimulation.Model.Tests
{
    [TestClass()]
    public class SlimeNetworkTests
    {
        [TestMethod()]
        public void CoversGraphTest()
        {
            var graph = new LatticeGraphWithFoodSourcesGenerator().Generate();
            Assert.IsTrue(graph.EdgesInGraph.Count > 1);
            Assert.IsTrue(graph.NodesInGraph.Count() > 1);
            Assert.IsTrue(graph.FoodSources.Count > 1);

            SlimeNetwork slime = new SlimeNetworkGenerator().FromGraphWithFoodSources(graph);
            Assert.IsTrue(slime.CoversGraph(graph));
        }

        [TestMethod()]
        public void MakeEdgesFromSlimeOrGraph()
        {
            var a = new Node(1, 1, 1);
            var b = new FoodSourceNode(2, 2, 2);
            var c = new FoodSourceNode(3, 3, 3);
            var foodSources = new HashSet<FoodSourceNode>() { b, c };
            var nodes = new HashSet<Node>() { a, b, c };

            var ab = new Edge(a, b);
            var ac = new Edge(a, c);
            var bc = new Edge(b, c);
            var edges = new HashSet<Edge>() { ab, ac, bc };

            var abSlime = new SlimeEdge(ab, 1);

            var slime = new SlimeNetwork(new HashSet<SlimeEdge>() { abSlime });
            var graph = new GraphWithFoodSources(edges);

            var allEdgesWithSlime = slime.GetAllEdgesInGraphReplacingThoseWhichAreSlimed(graph);

            Assert.IsTrue(allEdgesWithSlime.Count == edges.Count);
            Assert.IsTrue(allEdgesWithSlime.Contains(abSlime));
        }

        [TestMethod()]
        public void InvalidSourceSinkTest()
        {
            Node a = new Node(1, 1, 1);
            Node b = new Node(2, 2, 2);
            var ab = new SlimeEdge(a, b, 1);
            var slimeEdges = new HashSet<SlimeEdge>() {ab};
            var slime = new SlimeNetwork(slimeEdges);

            Assert.IsTrue(slime.InvalidSourceSink(a, a), "Same node cannot be source AND sink");
            Assert.IsFalse(slime.InvalidSourceSink(a, b));
        }
    }
}
