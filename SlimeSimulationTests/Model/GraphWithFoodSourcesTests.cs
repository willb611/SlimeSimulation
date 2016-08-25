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
    public class GraphWithFoodSourcesTests
    {

        [TestMethod()]
        public void EqualsTest()
        {
            var a = new Node(1, 1, 1);
            var b = new Node(2, 2, 2);
            var c = new FoodSourceNode(3, 3, 3);
            var nodes = new HashSet<Node>() { a, b, c };
            var foodSources = new HashSet<FoodSourceNode>() {c};

            var ab = new Edge(a, b);
            var bc = new Edge(b, c);
            var ac = new Edge(a, c);
            var edges = new HashSet<Edge>() { ab, bc, ac };

            var graph = new GraphWithFoodSources(edges, nodes, foodSources);
            var other = new GraphWithFoodSources(edges, nodes, foodSources);
            Assert.AreEqual(graph, other);
        }
    }
}
