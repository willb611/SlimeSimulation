using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Model.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model.Generation.Tests
{
    [TestClass()]
    public class GraphWithFoodSourcesGeneratorTests
    {
        [TestMethod()]
        public void JoinCellsInRowTest()
        {
            Node a = new Node(1, 1, 1);
            Node b = new Node(2, 2, 2);
            Node c = new Node(3, 3, 3);
            var nodes = new List<Node>() {a,b,c};

            var abEdge = new Edge(a, b);
            var bcEdge = new Edge(b, c);
            var generator = FormatterServices.GetUninitializedObject(typeof(GridGraphWithFoodSourcesGenerator)) as GridGraphWithFoodSourcesGenerator;
            var result = generator.CreateEdgesBetweenNodesInOrder(nodes);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(abEdge));
            Assert.IsTrue(result.Contains(bcEdge));
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod()]
        public void JoinCells_WithPreviousRow()
        {
            Node a = new Node(1, 1, 1);
            Node b = new Node(2, 2, 2);
            var firstRow = new List<Node>() { a, b };

            Node c = new Node(3, 3, 3);
            Node d = new Node(4, 4, 4);
            var secondRow = new List<Node>() {c, d};

            var ac = new Edge(a, c);
            var bd = new Edge(b, d);
            var generator = FormatterServices.GetUninitializedObject(typeof(GridGraphWithFoodSourcesGenerator)) as GridGraphWithFoodSourcesGenerator;
            var result = generator.CreateEdgesBetweenRowsAtSameIndex(firstRow, secondRow);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(ac));
            Assert.IsTrue(result.Contains(bd));
            Assert.AreEqual(2, result.Count);
        }
    }
}
