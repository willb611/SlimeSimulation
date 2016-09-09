using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SlimeSimulation.Model;

namespace SlimeSimulation.Algorithms.Bfs.Tests
{
    [TestClass()]
    public class BfsSolverTests
    {
        [TestMethod()]
        public void FindConnectedSubgraphsTest()
        {
            var a = new FoodSourceNode(1, 1, 1);
            var b = new FoodSourceNode(2, 2, 2);
            var ab = new SlimeEdge(a, b, 1);

            var c = new FoodSourceNode(3, 3, 3);
            var d = new FoodSourceNode(4, 4, 4);
            var cd = new SlimeEdge(c, d, 1);
            var slimeEdges = new HashSet<SlimeEdge>() { ab, cd };
            var slime = new SlimeNetwork(slimeEdges);

            var solver = new BfsSolver();
            var connectedSubgraphs = solver.SplitIntoSubgraphs(slime);
            Subgraph connectionResultForA = connectedSubgraphs.SubgraphContainingNode(a);
            Assert.IsNotNull(connectionResultForA);
            Assert.IsNotNull(connectionResultForA.NodesInGraph);
            Assert.IsTrue(connectionResultForA.NodesInGraph.Contains(b));
            Assert.IsTrue(connectionResultForA.NodesInGraph.Contains(a));

            Subgraph connectionResultForC = connectedSubgraphs.SubgraphContainingNode(c);
            Assert.IsNotNull(connectionResultForC);
            Assert.IsNotNull(connectionResultForC.NodesInGraph);
            Assert.IsTrue(connectionResultForC.NodesInGraph.Contains(c));
            Assert.IsTrue(connectionResultForC.NodesInGraph.Contains(d));
        }
    }
}
