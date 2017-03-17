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
        public const int LatticeType = 0;
        public const string LatticeTypeDescription = "Lattice";

        public const int GridType = 1;
        public const string GridTypeDescription = "Grid";

        public const int GenerateFromFileType = 2;
        public const string GenerateFromFileTypeDescription = "Read in from file description";

        public static String[] Descriptions = new string[] {GridTypeDescription, LatticeTypeDescription};

        public static int GetValueForDescription(string description)
        {
            switch (description)
            {
                case GridTypeDescription:
                default:
                    return GridType;
                case LatticeTypeDescription:
                    return LatticeType;
                case GenerateFromFileTypeDescription:
                    return GenerateFromFileType;
            }
        }

        public static GraphWithFoodSourcesGenerator MakeGenerator(GraphWithFoodSourceGenerationConfig config)
        {
            switch (config.GeneratorTypeToUse)
            {
                case GenerateFromFileType:
                    return new GraphWithFoodSourcesFromFileGenerator(config.ConfigForGenerator, config.FileToLoadFrom);
                case GridType:
                    return new GridGraphWithFoodSourcesGenerator(config.ConfigForGenerator);
                case LatticeType:
                default:
                    return new LatticeGraphWithFoodSourcesGenerator(config.ConfigForGenerator);
            }
        }
    }
}
