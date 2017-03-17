using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Model.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GLib;
using SlimeSimulation.Configuration;

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
            var nodes = new List<Node>() { a, b, c };

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
            var secondRow = new List<Node>() { c, d };

            var ac = new Edge(a, c);
            var bd = new Edge(b, d);
            var generator = FormatterServices.GetUninitializedObject(typeof(GridGraphWithFoodSourcesGenerator)) as GridGraphWithFoodSourcesGenerator;
            var result = generator.CreateEdgesBetweenRowsAtSameIndex(firstRow, secondRow);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains(ac));
            Assert.IsTrue(result.Contains(bd));
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod()]
        public void CreateEdgesLikeSnakeFromTopToBottomTest()
        {
            Node a = new Node(1, 1, 1);
            Node b = new Node(2, 2, 2);
            Node e = new Node(5, 5, 5);
            var firstRow = new List<Node>() { a, b, e };

            Node c = new Node(3, 3, 3);
            Node d = new Node(4, 4, 4);
            Node f = new Node(6, 6, 6);
            var secondRow = new List<Node>() { c, d, f };

            /*
             * Should be connected like:
             * a b  e
             *  \  /
             * c d f
             */
            var ad = new Edge(a, d);
            var de = new Edge(d, e);

            var generator = FormatterServices.GetUninitializedObject(typeof(GridGraphWithFoodSourcesGenerator)) as GridGraphWithFoodSourcesGenerator;
            var result = generator.CreateEdgesLikeSnakeFromTopToBottom(firstRow, secondRow);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(ad));
            Assert.IsTrue(result.Contains(de));
        }

        [TestMethod()]
        public void Generate_EdgesWithDiagonalsTest()
        {
            var a = new Node(1, 0, 0);
            var b = new Node(2, 1, 0);
            var c = new Node(3, 1, 1);
            var d = new Node(4, 0, 1);
            var dcList = new List<Node> {d, c};
            var abList = new List<Node> {a, b};
            List<List<Node>> nodes = new List<List<Node>>()
            {
                dcList, abList
            };
            /*Should create like:
             * d - c
             * | \ |
             * a - b
             */
            var config = new ConfigForGraphGenerator(5, 0.3, 5, GraphWithFoodSourcesGenerator.EdgeConnectionTypeSquareWithDiamonds);
            var generator = new GridGraphWithFoodSourcesGenerator(config);
            var edges = generator.GenerateEdges(nodes, 2, 2, config.EdgeConnectionType);
            Assert.IsNotNull(edges, "Should create a not null result fod valid input");
            Assert.AreEqual(5, edges.Count, "should be 5 food edges");
            var bottomRow = new Edge(a, b);
            Assert.IsTrue(edges.Contains(bottomRow), "bottom row missing, expected: " + bottomRow);

            var leftVertical = new Edge(a, d);
            Assert.IsTrue(edges.Contains(leftVertical), "left vertical missing, expected: " + leftVertical);

            var topRow = new Edge(d, c);
            Assert.IsTrue(edges.Contains(topRow), "top row missing, expected: " + topRow);

            var rightVertical = new Edge(c, b);
            Assert.IsTrue(edges.Contains(rightVertical), "right vertical missing, expected: " + rightVertical);

            var diagonal = new Edge(b, d);
            Assert.IsTrue(edges.Contains(diagonal), "diagonal from top left to bottom right missing, expected: " + diagonal);
        }
    }
}
