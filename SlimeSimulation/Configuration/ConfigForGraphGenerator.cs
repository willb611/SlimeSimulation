using System;
using Newtonsoft.Json;
using SlimeSimulation.Model.Generation;

namespace SlimeSimulation.Configuration
{
    public class ConfigForGraphGenerator
    {
        private static readonly double DefaultProbabilityNewNodeIsFood = 0.03;
        private static readonly int DefaultMinimumFoodSources = 2;
        private static readonly int DefaultSize = 9;
        private static readonly int DefaultEdgeConnectionType = EdgeConnectionShape.DefaultEdgeConnectionType;

        public ConfigForGraphGenerator() : this(DefaultSize)
        {
        }

        public ConfigForGraphGenerator(int size) : this(size, DefaultProbabilityNewNodeIsFood, DefaultMinimumFoodSources)
        {
        }
        public ConfigForGraphGenerator(int size, double probabilityNewNodeIsFoodSource,
            int minimumFoodSources) : this(size, DefaultProbabilityNewNodeIsFood, DefaultMinimumFoodSources, DefaultEdgeConnectionType)
        {
        }
        [JsonConstructor]
        public ConfigForGraphGenerator(int size, double probabilityNewNodeIsFoodSource,
            int minimumFoodSources, int edgeConnectionType)
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
            EdgeConnectionType = edgeConnectionType;
        }


        public int MinimumFoodSources { get; internal set; }
        public double ProbabilityNewNodeIsFoodSource { get; internal set; }
        public int Size { get; internal set; }
        public int EdgeConnectionType { get; internal set; }
    }
}
