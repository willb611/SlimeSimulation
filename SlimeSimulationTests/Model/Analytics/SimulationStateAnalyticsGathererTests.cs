using SlimeSimulation.Model.Analytics;
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

        [TestMethod()]
        public void DegreeOfSeperation_Simple()
        {
            /*
             * A-B
             *  \|
             *   C
             */
            var a = new FoodSourceNode(1, 0, 4);
            var b = new Node(2, 3, 4);
            var c = new FoodSourceNode(3, 3, 0);

            var ab = new Edge(a, b);
            var bc = new Edge(b, c);
            var ac = new Edge(a, c);
            var slimeEdges = new HashSet<SlimeEdge>()
            {
                new SlimeEdge(ab, 0.5),
                new SlimeEdge(bc, 0.5),
                new SlimeEdge(ac, 0.5)
            };

            var expectedSeperation = 0;
            var statExtractor = new SimulationStateAnalyticsGatherer();
            var slime = new SlimeNetwork(slimeEdges);
            Assert.AreEqual(expectedSeperation, statExtractor.DegreeOfSeperation(slime, a, c));
        }
        [TestMethod()]
        public void DegreeOfSeperation_SomeSeperation()
        {
            /*
             * A-B-C
             */
            var a = new FoodSourceNode(1, 1, 0);
            var b = new Node(2, 2, 0);
            var c = new FoodSourceNode(3, 3, 0);

            var ab = new Edge(a, b);
            var bc = new Edge(b, c);
            var slimeEdges = new HashSet<SlimeEdge>()
            {
                new SlimeEdge(ab, 0.5),
                new SlimeEdge(bc, 0.5)
            };

            var expectedSeperation = 1;
            var slime = new SlimeNetwork(slimeEdges);
            var statExtractor = new SimulationStateAnalyticsGatherer();
            Assert.AreEqual(expectedSeperation, statExtractor.DegreeOfSeperation(slime, a, c));
        }
        [TestMethod()]
        public void AverageDegreeOfSeperation_TwoFoods()
        {
            /*
             * A-B-C-D
             * AC - seperation = 1
             * AD - sep = 2
             * CD - sep = 0
             */
            var a = new FoodSourceNode(1, 1, 0);
            var b = new Node(2, 2, 0);
            var c = new FoodSourceNode(3, 3, 0);
            var d = new FoodSourceNode(4, 4, 0);

            var ab = new Edge(a, b);
            var bc = new Edge(b, c);
            var cd = new Edge(c, d);
            var slimeEdges = new HashSet<SlimeEdge>()
            {
                new SlimeEdge(ab, 0.5),
                new SlimeEdge(bc, 0.5),
                new SlimeEdge(cd, 0.5)
            };

            var expectedSeperation = 1.0;
            var statExtractor = new SimulationStateAnalyticsGatherer();
            Assert.AreEqual(expectedSeperation, statExtractor.AverageDegreeOfSeperation(new SlimeNetwork(slimeEdges)));
        }
    }
}