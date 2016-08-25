using System.Collections.Generic;

namespace SlimeSimulation.Model
{
    public class GraphWithFoodSources : Graph
    {
        public ISet<FoodSourceNode> FoodSources { get; }

        public GraphWithFoodSources(ISet<Edge> edges, ISet<Node> nodes, ISet<FoodSourceNode> foodSourcesNodes) : base(edges, nodes)
        {
            FoodSources = foodSourcesNodes;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GraphWithFoodSources);
        }

        public bool Equals(GraphWithFoodSources other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            else if (ReferenceEquals(other, this))
            {
                return true;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }
            
            return FoodSources.Equals(other.FoodSources)
                   && base.Equals(other);
        }

        public override int GetHashCode()
        {
            return FoodSources.GetHashCode() * 17 + base.GetHashCode();
        }
    }
}
