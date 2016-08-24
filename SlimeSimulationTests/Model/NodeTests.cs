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
    public class NodeTests
    {

        [TestMethod()]
        public void UpdateEdgesWithReplacement_ShouldDoReplacement()
        {
            Node c = new Node(1, 1, 1);
            Node replacement = new Node(155, 155, 155);

            Node a = new Node(2, 2, 2);
            Node b = new Node(3, 3, 3);
            /*
             * a ------- b
             * |        /
             * replace (c)
             */
            SlimeEdge ab = new SlimeEdge(a, b, 1);
            SlimeEdge ac = new SlimeEdge(a, c, 1);
            SlimeEdge bc = new SlimeEdge(b, c, 1);
            HashSet<SlimeEdge> edges = new HashSet<SlimeEdge>() { ac, ab, bc };

            c.ReplaceWithGivenNodeInEdges(replacement, edges);
            // Check edges connecting to c were removed
            Assert.IsFalse(edges.Contains(ac));
            Assert.IsFalse(edges.Contains(bc));

            edges.Remove(ab);
            // Check edges left are conections from a -> replacement, and b -> replacement
            Assert.IsTrue(edges.Count == 2);
            List<Node> nodesReplacementShouldConnectTo = new List<Node>() { a, b };
            foreach (SlimeEdge edge in edges)
            {
                Node connected = edge.GetOtherNode(replacement);
                nodesReplacementShouldConnectTo.Remove(connected);
            }
            Assert.IsFalse(nodesReplacementShouldConnectTo.Any());
        }
    }
}
