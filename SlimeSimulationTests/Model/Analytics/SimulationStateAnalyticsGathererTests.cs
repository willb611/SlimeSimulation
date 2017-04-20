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
        public void AverageDegreeOfSeperation_DisconnectedSubgraphs()
        {
            /*
             * A-B   C-D
             */
            var a = new FoodSourceNode(1, 1, 0);
            var b = new Node(2, 2, 0);
            var c = new FoodSourceNode(3, 3, 0);
            var d = new Node(4, 4, 0);

            var ab = new Edge(a, b);
            var cd = new Edge(c, d);
            var slimeEdges = new HashSet<SlimeEdge>()
            {
                new SlimeEdge(ab, 0.5),
                new SlimeEdge(cd, 0.5)
            };
            var slime = new SlimeNetwork(slimeEdges);

            double expectedSeperation = 0.0;
            var statExtractor = new SimulationStateAnalyticsGatherer();
            Assert.AreEqual(expectedSeperation, statExtractor.AverageDegreeOfSeperation(slime));
        }

        [TestMethod()]
        public void DegreeOfSeperation_SomeSeperation()
        {
            /*
             * A-B-C
             */
            var a = new FoodSourceNode(1, 1, 0);
            var b = new FoodSourceNode(2, 2, 0);
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
             * AC - seperation = 0
             * AD - sep = 1
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

            var expectedSeperation = 1 / 3.0;
            var statExtractor = new SimulationStateAnalyticsGatherer();
            Assert.AreEqual(expectedSeperation, statExtractor.AverageDegreeOfSeperation(new SlimeNetwork(slimeEdges)));
        }

        [TestMethod()]
        public void AverageDegreeOfSeparation_SeperateByNode()
        {
            /*
             * A-B-C
             * B not food, so degree of separation ac = 0
             */
            var a = new FoodSourceNode(1, 1, 0);
            var b = new Node(2, 2, 0);
            var c = new FoodSourceNode(3, 3, 0);

            var slimeEdges = new HashSet<SlimeEdge>()
            {
                new SlimeEdge(a, b, 0.5),
                new SlimeEdge(b, c, 0.5)
            };

            var expectedSeperation = 0.0;
            var statExtractor = new SimulationStateAnalyticsGatherer();
            Assert.AreEqual(expectedSeperation, statExtractor.AverageDegreeOfSeperation(new SlimeNetwork(slimeEdges)));
        }

        [TestMethod()]
        public void MinimumDistance()
        {
            /*
             * A-B-C
             * Ab - length = 1
             * Ac - length = 2
             * bc - length = 1
             */
            var a = new FoodSourceNode(1, 1, 0);
            var b = new FoodSourceNode(2, 2, 0);
            var c = new FoodSourceNode(3, 3, 0);

            var ab = new Edge(a, b);
            var bc = new Edge(b, c);
            var slimeEdges = new HashSet<SlimeEdge>()
            {
                new SlimeEdge(ab, 0.5),
                new SlimeEdge(bc, 0.5)
            };

            var expectedSeperation = 4.0 / 3.0;
            var statExtractor = new SimulationStateAnalyticsGatherer();
            Assert.AreEqual(expectedSeperation, statExtractor.AverageMinimumDistance(new SlimeNetwork(slimeEdges)));
        }

        [TestMethod()]
        public void FaultToleranceTest_AlwaysFault()
        {
            /*
             * A-B-C-D
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
            var slime = new SlimeNetwork(slimeEdges);

            var expected = 0;
            var statExtractor = new SimulationStateAnalyticsGatherer();
            Assert.AreEqual(expected, statExtractor.FaultTolerance(slime));
        }

        [TestMethod()]
        public void FaultToleranceTest_SometimesFaults()
        {
            /*
             * A-B
             *  \| 
             *   C-D
             *  
             *  AB = 3
             *  BC = 4
             *  AC = 5
             *  CD = 1
             *  ft = 1 / 13 ?
             */
            var a = new FoodSourceNode(1, 0, 4);
            var b = new FoodSourceNode(2, 3, 4);
            var c = new FoodSourceNode(3, 3, 0);
            var d = new FoodSourceNode(4, 4, 0);

            var ab = new Edge(a, b);
            var bc = new Edge(b, c);
            var ac = new Edge(a, c);
            var cd = new Edge(c, d);
            var slimeEdges = new HashSet<SlimeEdge>()
            {
                new SlimeEdge(ab, 0.5),
                new SlimeEdge(bc, 0.5),
                new SlimeEdge(ac, 0.5),
                new SlimeEdge(cd, 0.5)
            };
            var slime = new SlimeNetwork(slimeEdges);

            double expected = 12.0 / 13.0;
            var statExtractor = new SimulationStateAnalyticsGatherer();
            Assert.AreEqual(expected, statExtractor.FaultTolerance(slime));
        }
    }
}
