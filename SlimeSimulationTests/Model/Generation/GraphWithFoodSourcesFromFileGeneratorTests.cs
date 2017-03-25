using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Model.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using NLog.Layouts;
using SlimeSimulation.Configuration;
using SlimeSimulation.Model.Simulation.Persistence;

namespace SlimeSimulation.Model.Generation.Tests
{
    [TestClass()]
    public class GraphWithFoodSourcesFromFileGeneratorTests
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [TestMethod()]
        public void Generate_FromDescription()
        {
            string description = "2\n" +
                                 "2\n" +
                                 "2\n" +
                                 "0,0\n" +
                                 "0,1";
            /*Should create like:
             * f - n                      -- f is food, n is node (not food)
             * |   |
             * f - n
             */
            var config = new ConfigForGraphGenerator(5, 0.2, 4, EdgeConnectionShape.EdgeConnectionShapeSquare);
            var generator = new GraphWithFoodSourcesFromFileGenerator(config, "test.file");
            var result = generator.CreateGraphFromDescription(description);
            Assert.IsNotNull(result, "Should create a not null result fod valid input");
            Logger.Info("Got result: {0}",
                JsonConvert.SerializeObject(result, SerializationSettings.JsonSerializerSettings));
            Assert.AreEqual(2, result.FoodSources.Count, "should be 2 food sources");
            Assert.AreEqual(4, result.NodesInGraph.Count, "with 2x2 grid should be 4 nodes");
            Assert.AreEqual(4, result.EdgesInGraph.Count, "with 2x2 grid should be 4 edges");
        }

        [TestMethod()]
        public void Generate_FromSpecifiedGrid()
        {
            string description = "3\n" +
                                 "3\n" +
                                 "grid\n" +
                                 "-n-\n" +
                                 "-fn\n" +
                                 "nnf";
            /*Should create like:
             *     n                      -- f is food, n is node (not food)
             *     |
             *     f - n
             *     |   |
             * n - n - f
             */
            var config = new ConfigForGraphGenerator(5, 0.2, 4, EdgeConnectionShape.EdgeConnectionShapeSquare);
            var generator = new GraphWithFoodSourcesFromFileGenerator(config, "test.file");
            var result = generator.CreateGraphFromDescription(description);
            Assert.IsNotNull(result, "Should create a not null result fod valid input");
            Logger.Info("Got result: {0}",
                JsonConvert.SerializeObject(result, SerializationSettings.JsonSerializerSettings));
            Assert.AreEqual(2, result.FoodSources.Count, "should be 2 food sources");
            Assert.AreEqual(6, result.NodesInGraph.Count, "Reading from grid with 6 nodes, should be 6 nodes");
            Assert.AreEqual(6, result.EdgesInGraph.Count, "Reading from a grid with square shape should be be 6 edges");
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Construct_TestBadArgument()
        {
            new GraphWithFoodSourcesFromFileGenerator(new ConfigForGraphGenerator(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Construct_TestBadArgument_EmptyFilepath()
        {
            new GraphWithFoodSourcesFromFileGenerator(new ConfigForGraphGenerator(), "");
        }
    }
}
