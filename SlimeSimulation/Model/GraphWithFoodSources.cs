using System.Collections.Generic;
using System;

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
            return this.Equals(obj as GraphWithFoodSources);
        }

        public bool Equals(GraphWithFoodSources other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }
            else if (Object.ReferenceEquals(other, this))
            {
                return true;
            }

            if (this.GetType() != other.GetType())
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
