using System.Collections.Generic;
using System.Linq;
using SlimeSimulation.Algorithms.Bfs;

namespace SlimeSimulation.Model
{
    public class Nodes
    {
        public static ISet<FoodSourceNode> GetFoodSourceNodes(ISet<Node> nodes)
        {
            var foodSources = new HashSet<FoodSourceNode>();
            foreach (var node in nodes)
            {
                if (node.IsFoodSource)
                {
                    foodSources.Add((FoodSourceNode)node);
                }
            }
            return foodSources;
        }

        public static IEnumerable<FoodSourceNode> GetFoodSourcesConnectedToNodeInGraph(Node node,
            GraphWithFoodSources graphWithFoodSources)
        {
            var connectedNodes = new BfsSolver().From(node, graphWithFoodSources).NodesInGraph;
            var connectedFood = connectedNodes.Where(n => n is FoodSourceNode);
            return CastToFood(connectedFood);
        }

        public static IEnumerable<FoodSourceNode> CastToFood(IEnumerable<Node> nodes)
        {
            return nodes.Cast<FoodSourceNode>();
        }

        public static List<Node> SortByXAscending(ICollection<Node> nodes)
        {
            return nodes.OrderBy(node => node.X).ToList();
        }

        public static List<Node> SortByYAscending(ICollection<Node> nodes)
        {
            return nodes.OrderBy(node => node.Y).ToList();
        }
    }
}
