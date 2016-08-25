using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model.Generation
{
    public class SlimeNetworkGenerator
    {
        private const int DefaultStartingConnectivity = 1;

        public SlimeNetwork FromGraphWithFoodSources(GraphWithFoodSources graphWithFoodSources)
        {
            var edges = new HashSet<SlimeEdge>();
            foreach (var edge in graphWithFoodSources.Edges)
            {
                var slimeEdge = new SlimeEdge(edge, DefaultStartingConnectivity);
                edges.Add(slimeEdge);
            }
            return new SlimeNetwork(graphWithFoodSources.Nodes, graphWithFoodSources.FoodSources, edges);
        }
    }
}
