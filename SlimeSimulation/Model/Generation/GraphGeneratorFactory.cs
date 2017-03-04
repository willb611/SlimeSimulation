using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Configuration;

namespace SlimeSimulation.Model.Generation
{
    class GraphGeneratorFactory
    {
        public const int DiagonalConnectedGridType = 0;
        public const string DiagonalConnectedGridTypeDescription = "Grid with diagonal connects";

        public const int GridType = 1;
        public const string GridTypeDescription = "Grid";
        public static String[] Descriptions = new string[] {DiagonalConnectedGridTypeDescription, GridTypeDescription};

        public static int GetValueForDescription(string description)
        {
            switch (description)
            {
                case GridTypeDescription:
                default:
                    return GridType;
                case DiagonalConnectedGridTypeDescription:
                    return DiagonalConnectedGridType;
            }
        }

        public static IGraphWithFoodSourcesGenerator MakeGenerator(GraphWithFoodSourceGenerationConfig config)
        {
            switch (config.GeneratorTypeToUse)
            {
                case GridType:
                    return new GridGraphWithFoodSourcesGenerator(config.ConfigForGenerator);
                case DiagonalConnectedGridType:
                default:
                    return new LatticeGraphWithFoodSourcesGenerator(config.ConfigForGenerator);
            }
        }
    }
}
