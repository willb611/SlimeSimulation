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
      HashSet<Node> nodes = new HashSet<Node>() {a, b, c};

      Edge ab = new Edge(a, b, 1);
      Edge ac = new Edge(a, c, 1);
      Edge bc = new Edge(b, c, 1);
      HashSet<Edge> edges = new HashSet<Edge>() {ac, bc, ab};

      Graph graph = new Graph(edges, nodes);

      Edge acActual = graph.GetEdgeBetween(a, c);
      Assert.AreEqual(ac, acActual);

      Edge bcActual = graph.GetEdgeBetween(b, c);
      Assert.AreEqual(bc, bcActual);

      Edge abActual = graph.GetEdgeBetween(a, b);
      Assert.AreEqual(ab, abActual);
    }
  }
}