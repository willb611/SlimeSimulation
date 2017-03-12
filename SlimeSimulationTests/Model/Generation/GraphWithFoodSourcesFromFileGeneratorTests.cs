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
        public void GenerateTest()
        {
            string description = "2\n" +
                                 "2\n" +
                                 "2\n" +
                                 "0,0\n" +
                                 "0,1";
            /*Should create like:
             * f - n
             * |   |
             * f - n
             */
            var generator = FormatterServices.GetUninitializedObject(typeof(GraphWithFoodSourcesFromFileGenerator)) as GraphWithFoodSourcesFromFileGenerator;
            var result = generator.CreateGraphFromDescription(description);
            Assert.IsNotNull(result, "Should create a not null result fod valid input");
            Logger.Info("Got result: {0}",
                JsonConvert.SerializeObject(result, SerializationSettings.JsonSerializerSettings));
            Assert.AreEqual(2, result.FoodSources.Count, "should be 2 food sources");
            Assert.AreEqual(4, result.NodesInGraph.Count, "with 2x2 grid should be 4 nodes");
            Assert.AreEqual(4, result.EdgesInGraph.Count, "with 2x2 grid should be 4 edges");
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Construct_TestBadArgument()
        {
            var config = new GraphWithFoodSourceGenerationConfig(new LatticeGraphWithFoodSourcesGenerationConfig(),
                    GraphGeneratorFactory.GenerateFromFileType, null);
            new GraphWithFoodSourcesFromFileGenerator(config);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Construct_TestBadArgument_EmptyFilepath()
        {
            var config = new GraphWithFoodSourceGenerationConfig(new LatticeGraphWithFoodSourcesGenerationConfig(),
                    GraphGeneratorFactory.GenerateFromFileType, "");
            new GraphWithFoodSourcesFromFileGenerator(config);
        }
    }
}
