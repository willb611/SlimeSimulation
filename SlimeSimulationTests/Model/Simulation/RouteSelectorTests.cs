using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Model.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model.Simulation.Tests
{
    [TestClass()]
    public class RouteSelectorTests
    {
        [TestMethod()]
        public void SelectRouteTest()
        {
            var a = new FoodSourceNode(1, 1, 1);
            var b = new FoodSourceNode(2, 2, 2);
            var ab = new SlimeEdge(a, b, 1);
            var slimeEdges = new HashSet<SlimeEdge>() {ab};
            var slime = new SlimeNetwork(slimeEdges);
            
            var routeSelector = new RouteSelector();
            var actualRoute = routeSelector.SelectRoute(slime);
            Assert.IsNotNull(actualRoute);
            Assert.IsNotNull(actualRoute.Sink);
            Assert.IsNotNull(actualRoute.Source);
            var actualSink = actualRoute.Sink;
            Assert.IsTrue(actualSink.Equals(a) || actualSink.Equals(b));
            var actualSource = actualRoute.Source;
            Assert.IsNotNull(actualSource);
            Assert.AreEqual(ab.GetOtherNode(actualSink), actualSource);
        }
    }
}
