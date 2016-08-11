using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Configuration
{
    public class LatticeSlimeNetworkGenerationConfig
    {
        private static readonly double DEFAULT_PROBABILITY_NEW_NODE_IS_FOOD = 0.05;
        private static readonly int DEFAULT_MINIMUM_FOOD_SOURCES = 2;
        private static readonly int DEFAULT_STARTING_CONNECTIVITY = 1;
        private static readonly int DEFAULT_SIZE = 5;

        public LatticeSlimeNetworkGenerationConfig() : this(DEFAULT_SIZE)
        {
        }

        public LatticeSlimeNetworkGenerationConfig(int size) : this(size, DEFAULT_PROBABILITY_NEW_NODE_IS_FOOD, DEFAULT_MINIMUM_FOOD_SOURCES)
        {
        }

        public LatticeSlimeNetworkGenerationConfig(int size,
            double probabilityNewNodeIsFoodSource, int minimumFoodSources) : this(size, probabilityNewNodeIsFoodSource,
                minimumFoodSources, DEFAULT_STARTING_CONNECTIVITY)
        {
        }


        public LatticeSlimeNetworkGenerationConfig(int size, double probabilityNewNodeIsFoodSource,
            int minimumFoodSources, int startingConnectivity)
        {
            if (size < 3)
            {
                throw new ArgumentException("SIze must be > 3. Given: " + size);
            } else if (minimumFoodSources < 2)
            {
                throw new ArgumentException("Must have at least 2 food sources in network");
            } else
            {
                int predictedNodesInGraph = size * size - 3;
                if (minimumFoodSources > predictedNodesInGraph)
                {
                    throw new ArgumentException(
                        String.Format("Wont be enough nodes in the graph {0} for minimum number of food nodes {1}",
                        predictedNodesInGraph, minimumFoodSources));
                }
            } 
            this.Size = size;
            this.ProbabilityNewNodeIsFoodSource = probabilityNewNodeIsFoodSource;
            this.MinimumFoodSources = minimumFoodSources;
            this.StartingConnectivity = startingConnectivity;
        }


        public int MinimumFoodSources { get; private set; }
        public double ProbabilityNewNodeIsFoodSource { get; private set; }
        public int Size { get; private set; }
        public int StartingConnectivity { get; private set; }
    }
}
