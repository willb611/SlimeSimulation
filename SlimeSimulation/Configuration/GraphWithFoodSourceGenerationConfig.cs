using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model.Generation;

namespace SlimeSimulation.Configuration
{
    public class GraphWithFoodSourceGenerationConfig
    {
        private static readonly int DefaultGeneratorToUse = GraphGeneratorFactory.GridType;

        public GraphWithFoodSourceGenerationConfig() : this(new LatticeGraphWithFoodSourcesGenerationConfig(), DefaultGeneratorToUse) { }
        public GraphWithFoodSourceGenerationConfig(LatticeGraphWithFoodSourcesGenerationConfig latticeConfig,
            int generatorToUse)
        {
            GeneratorTypeToUse = generatorToUse;
            ConfigForGenerator = latticeConfig;
        }


        public int GeneratorTypeToUse { get; }
        public LatticeGraphWithFoodSourcesGenerationConfig ConfigForGenerator { get; }
    }
}
