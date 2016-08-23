using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Configuration
{
    public class LatticeSlimeNetworkGenerationConfig
    {
        private static readonly double DefaultProbabilityNewNodeIsFood = 0.05;
        private static readonly int DefaultMinimumFoodSources = 2;
        private static readonly int DefaultStartingConnectivity = 1;
        private static readonly int DefaultSize = 5;

        public LatticeSlimeNetworkGenerationConfig() : this(DefaultSize)
        {
        }

        public LatticeSlimeNetworkGenerationConfig(int size) : this(size, DefaultProbabilityNewNodeIsFood, DefaultMinimumFoodSources)
        {
        }

        public LatticeSlimeNetworkGenerationConfig(int size,
            double probabilityNewNodeIsFoodSource, int minimumFoodSources) : this(size, probabilityNewNodeIsFoodSource,
                minimumFoodSources, DefaultStartingConnectivity)
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
