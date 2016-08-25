using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model.Generation
{
    class SlimeNetworkGenerator
    {
        private static readonly int DefaultStartingConnectivity = 1;

        public SlimeNetwork Generate(GraphWithFoodSources graphWithFoodSources)
        {
            var edges = new HashSet<SlimeEdge>();
            foreach (Edge edge in graphWithFoodSources.Edges)
            {
                var slimeEdge = new SlimeEdge(edge, DefaultStartingConnectivity);
            }
            return new SlimeNetwork(graphWithFoodSources.Nodes, graphWithFoodSources.FoodSources, edges);
        }
    }
}
