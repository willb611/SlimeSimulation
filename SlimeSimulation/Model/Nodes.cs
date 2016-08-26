using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model
{
    class Nodes
    {
        public static ISet<FoodSourceNode> GetFoodSourceNodes(ISet<Node> nodes)
        {
            var foodSources = new HashSet<FoodSourceNode>();
            foreach (var node in nodes)
            {
                if (node.IsFoodSource())
                {
                    foodSources.Add((FoodSourceNode)node);
                }
            }
            return foodSources;
        }
    }
}
