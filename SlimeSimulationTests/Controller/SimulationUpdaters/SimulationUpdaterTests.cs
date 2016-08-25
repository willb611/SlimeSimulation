using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Controller.SimulationUpdaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;
using SlimeSimulation.Model.Generation;
using SlimeSimulation.StdLibHelpers;

namespace SlimeSimulation.Controller.SimulationUpdaters.Tests
{
    [TestClass()]
    public class SimulationUpdaterTests
    {

        [TestMethod()]
        public void ExpandSlimeNetwork_ShouldIncreaseSize()
        {
            var graph = new LatticeGraphWithFoodSourcesGenerator().Generate();
            var slimeNodes = new HashSet<FoodSourceNode>() {graph.FoodSources.PickRandom()};
            var slimeNetwork = new SlimeNetwork(new HashSet<Node>(slimeNodes), slimeNodes, new HashSet<SlimeEdge>());
            var simulationUpdater = new SimulationUpdater();
            SlimeNetwork expandedSlimeNetwork = simulationUpdater.ActuallyExpandSlime(slimeNetwork, graph);

            Assert.IsTrue(expandedSlimeNetwork.Nodes.Count > 1);
            Assert.IsTrue(expandedSlimeNetwork.Edges.Count >= 1);
        }
    }
}
