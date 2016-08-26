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
            foreach (var slime in slimeEdges)
            {
                Assert.IsTrue(edges.Contains(slime.Edge));
            }
        }
    }
}
