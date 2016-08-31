using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.StdLibHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;

namespace SlimeSimulation.StdLibHelpers.Tests
{
    [TestClass()]
    public class EnumeratorExtensionTests
    {
        [TestMethod()]
        public void AsListTest()
        {
            var a = new FoodSourceNode(1, 1, 1);
            var b = new FoodSourceNode(2, 2, 2);
            var c = new FoodSourceNode(3, 3, 3);
            var nodes = new List<Node>() {a, b, c};

            using (var enumerator = nodes.GetEnumerator())
            {
                var actualNodes = enumerator.AsList();
                Assert.AreEqual(nodes.Count, actualNodes.Count);
                Assert.AreNotEqual(nodes, actualNodes);
            }
        }
    }
}
