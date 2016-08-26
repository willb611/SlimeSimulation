using System;
using System.Collections.Generic;

namespace SlimeSimulation.Model
{
    public class GraphWithFoodSources : Graph
    {
        private ISet<FoodSourceNode> _foodSources;
        public ISet<FoodSourceNode> FoodSources {
            get
            {
                return _foodSources;
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("FoodSources");
                }
                else
                {
                    _foodSources = value;
                }
            }
        }

        public GraphWithFoodSources(ISet<Edge> edgesInGraph) : base(edgesInGraph)
        {
            FoodSources = Nodes.GetFoodSourceNodes(NodesInGraph);
        }
        public GraphWithFoodSources(ISet<Edge> edgesInGraph, ISet<Node> nodesInGraph, ISet<FoodSourceNode> foodSourcesNodes) : base(edgesInGraph, nodesInGraph)
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
            if (ReferenceEquals(other, this))
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
