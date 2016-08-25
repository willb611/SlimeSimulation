using System;

namespace SlimeSimulation.Configuration
{
    public class LatticeGraphWithFoodSourcesGenerationConfig
    {
        private static readonly double DefaultProbabilityNewNodeIsFood = 0.05;
        private static readonly int DefaultMinimumFoodSources = 2;
        private static readonly int DefaultSize = 4;

        public LatticeGraphWithFoodSourcesGenerationConfig() : this(DefaultSize)
        {
        }

        public LatticeGraphWithFoodSourcesGenerationConfig(int size) : this(size, DefaultProbabilityNewNodeIsFood, DefaultMinimumFoodSources)
        {
        }
        
        public LatticeGraphWithFoodSourcesGenerationConfig(int size, double probabilityNewNodeIsFoodSource,
            int minimumFoodSources)
        {
            if (size < 3)
            {
                throw new ArgumentException("SIze must be > 3. Given: " + size);
            }
            if (minimumFoodSources < 2)
            {
                throw new ArgumentException("Must have at least 2 food sources in network");
            }
            int predictedNodesInGraph = size * size - 3;
            if (minimumFoodSources > predictedNodesInGraph)
            {
                throw new ArgumentException(
                    String.Format("Wont be enough nodes in the graph {0} for minimum number of food nodes {1}",
                        predictedNodesInGraph, minimumFoodSources));
            }
            Size = size;
            ProbabilityNewNodeIsFoodSource = probabilityNewNodeIsFoodSource;
            MinimumFoodSources = minimumFoodSources;
        }


        public int MinimumFoodSources { get; private set; }
        public double ProbabilityNewNodeIsFoodSource { get; private set; }
        public int Size { get; private set; }
    }
}
