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

        public GraphWithFoodSourceGenerationConfig() : this(new LatticeGraphWithFoodSourcesGenerationConfig(), DefaultGeneratorToUse) { }

        [JsonConstructor]
        public GraphWithFoodSourceGenerationConfig(LatticeGraphWithFoodSourcesGenerationConfig configForGenerator,
            int generatorTypeToUse)
        {
            GeneratorTypeToUse = generatorTypeToUse;
            ConfigForGenerator = configForGenerator;
        }


        public int GeneratorTypeToUse { get; private set; }
        public LatticeGraphWithFoodSourcesGenerationConfig ConfigForGenerator { get; private set; }
    }
}
