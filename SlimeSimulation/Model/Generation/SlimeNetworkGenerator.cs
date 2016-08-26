using System.Collections.Generic;

namespace SlimeSimulation.Model.Generation
{
    public class SlimeNetworkGenerator
    {
        private const int DefaultStartingConnectivity = 1;

        public SlimeNetwork FromGraphWithFoodSources(GraphWithFoodSources graphWithFoodSources)
        {
            var edges = new HashSet<SlimeEdge>();
            foreach (var edge in graphWithFoodSources.EdgesInGraph)
            {
                var slimeEdge = new SlimeEdge(edge, DefaultStartingConnectivity);
                edges.Add(slimeEdge);
            }
            return new SlimeNetwork(graphWithFoodSources.NodesInGraph, graphWithFoodSources.FoodSources, edges);
        }
    }
}
