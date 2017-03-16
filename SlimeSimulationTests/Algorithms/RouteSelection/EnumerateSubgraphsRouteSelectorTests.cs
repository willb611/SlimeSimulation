using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Algorithms.RouteSelection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Configuration;
using SlimeSimulation.Model;
using SlimeSimulation.Model.Generation;

namespace SlimeSimulation.Algorithms.RouteSelection.Tests
{
    [TestClass()]
    public class EnumerateSubgraphsRouteSelectorTests
    {
        [TestMethod()]
        public void SelectRoute_SelectsSourceDeterminstically()
        {
            var graphWithFoodSources = new LatticeGraphWithFoodSourcesGenerator(new ConfigForGraphGenerator(15, 0.1, 5)).Generate();
            var slimeNetwork = new SlimeNetworkGenerator().FromGraphWithFoodSources(graphWithFoodSources);
            ISet<Node> foodSourcesUsed = new HashSet<Node>();
            var routeSelector = new EnumerateSubgraphsRouteSelector();
            for (int i = 0; i < graphWithFoodSources.FoodSources.Count; i++)
            {
                var route = routeSelector.SelectRoute(slimeNetwork);
                foodSourcesUsed.Add(route.Source);
            }

            Assert.AreEqual(foodSourcesUsed.Count, slimeNetwork.FoodSources.Count, 
                "enumerateBySubgraphs should choose sources in a deterministic sequential fashion");
            foreach (var food in slimeNetwork.FoodSources)
            {
                Assert.IsTrue(foodSourcesUsed.Contains(food as Node),
                    "if ran for n (num of food sources in graph) times in a graph which isnt split into subgraphs, all nodes should food sources in the n routes");
            }
        }

        [TestMethod()]
        public void SelectRoute_OnlyChoosesFoodSources()
        {
            var graphWithFoodSources = new LatticeGraphWithFoodSourcesGenerator(new ConfigForGraphGenerator(15, 0.1, 5)).Generate();
            var slimeNetwork = new SlimeNetworkGenerator().FromGraphWithFoodSources(graphWithFoodSources);
            var routeSelector = new EnumerateSubgraphsRouteSelector();
            for (int i = 0; i < graphWithFoodSources.FoodSources.Count; i++)
            {
                var route = routeSelector.SelectRoute(slimeNetwork);
                Assert.IsTrue(route.Source.IsFoodSource, "Only food sources should be used in a route");
                Assert.IsTrue(route.Sink.IsFoodSource, "Only food sources should be used in a route");
            }
        }
    }
}
