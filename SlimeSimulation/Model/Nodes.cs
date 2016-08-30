using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model.Bfs;
using SlimeSimulation.StdLibHelpers;

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

        public static IEnumerable<FoodSourceNode> GetFoodSourcesConnectedToNodeInGraph(Node node,
            GraphWithFoodSources graphWithFoodSources)
        {
            return CastToFood(new BfsSolver().From(node, graphWithFoodSources).ConnectedNodes().Where(n => n is FoodSourceNode).Except(new List<Node>() { node }));
        }

        public static IEnumerable<FoodSourceNode> CastToFood(IEnumerable<Node> nodes)
        {
            return nodes.Cast<FoodSourceNode>();
        }
    }
}
