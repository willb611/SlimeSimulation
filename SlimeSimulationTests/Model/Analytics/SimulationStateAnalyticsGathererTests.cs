using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SlimeSimulation.Model.Analytics.Tests
{
    [TestClass()]
    public class SimulationStateAnalyticsGathererTests
    {
        [TestMethod()]
        public void TotalDistanceInSlimeTest()
        {
            /*
             * A-B
             *  \|
             *   C
             *   ab = 3, bc = 4, ab = 5 (pythagoras)
             */
            var a = new Node(1, 0, 4);
            var b = new Node(2, 3, 4);
            var c = new Node(3, 3, 0);

            var ab = new Edge(a, b);
            var bc = new Edge(b, c);
            var ac = new Edge(a, c);
            var slimeEdges = new HashSet<SlimeEdge>()
            {
                new SlimeEdge(ab, 0.5),
                new SlimeEdge(bc, 0.5),
                new SlimeEdge(ac, 0.5)
            };
            var slime = new SlimeNetwork(slimeEdges);

            var expected = 12.0;
            var statExtractor = new SimulationStateAnalyticsGatherer();
            Assert.AreEqual(expected, statExtractor.TotalDistanceInSlime(slime));
        }
    }
}