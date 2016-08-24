using System.Collections.Generic;
using System;

namespace SlimeSimulation.Model
{
    public class GraphWithFoodSources : Graph
    {
        private readonly ISet<FoodSourceNode> _foodSourcesNodes;

        public GraphWithFoodSources(ISet<Edge> edges, ISet<Node> nodes, ISet<FoodSourceNode> foodSourceNodes) : base(edges, nodes)
        {
            _foodSourcesNodes = foodSourceNodes;
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
            
            return _foodSourcesNodes.Equals(other._foodSourcesNodes)
                   && base.Equals(other);
        }

        public override int GetHashCode()
        {
            return _foodSourcesNodes.GetHashCode() * 17 + base.GetHashCode();
        }
    }
}
