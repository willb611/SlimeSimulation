using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SlimeSimulation.Model.Generation;

namespace SlimeSimulation.Configuration
{
    public class GraphWithFoodSourceGenerationConfig
    {
        private static readonly int DefaultGeneratorToUse = GraphGeneratorFactory.GridType;
        private static readonly string DefaultFileToLoadFrom = "exampleFile.txt";

        public GraphWithFoodSourceGenerationConfig()
            : this(new ConfigForGraphGenerator(), DefaultGeneratorToUse)
        {
        }

        public GraphWithFoodSourceGenerationConfig(ConfigForGraphGenerator configForGenerator,
            int generatorTypeToUse)
            : this(configForGenerator, generatorTypeToUse, DefaultFileToLoadFrom)
        {
        }

        [JsonConstructor]
        public GraphWithFoodSourceGenerationConfig(ConfigForGraphGenerator configForGenerator,
            int generatorTypeToUse, string fileToLoadFrom)
        {
            GeneratorTypeToUse = generatorTypeToUse;
            ConfigForGenerator = configForGenerator;
            FileToLoadFrom = fileToLoadFrom;
        }


        public int GeneratorTypeToUse { get; private set; }
        public ConfigForGraphGenerator ConfigForGenerator { get; private set; }
        public string FileToLoadFrom { get; private set; }
    }
}
