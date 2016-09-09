using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NLog;

namespace SlimeSimulation.Model
{
    public class GraphWithFoodSources : Graph
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
                    throw new ArgumentNullException(nameof(value));
                }
                else
                {
                    _foodSources = value;
                }
            }
        }

        public GraphWithFoodSources(ISet<Edge> edgesInGraph) : base(edgesInGraph)
        {
            if (edgesInGraph == null)
            {
                throw new ArgumentNullException(nameof(edgesInGraph));
            }
            FoodSources = Nodes.GetFoodSourceNodes(NodesInGraph);
            Logger.Trace("[Constructor : 1 param] Finished with foodSources.Count {0}, nodesInGraph.Count {1}, edgesINGraph.Count {2}",
                FoodSources.Count, NodesInGraph.Count, edgesInGraph.Count);
        }
        [JsonConstructor]
        public GraphWithFoodSources(ISet<Edge> edgesInGraph, ISet<Node> nodesInGraph, ISet<FoodSourceNode> foodSources) : base(edgesInGraph, nodesInGraph)
        {
            if (edgesInGraph == null)
            {
                throw new ArgumentNullException(nameof(edgesInGraph));
            } else if (nodesInGraph == null)
            {
                throw new ArgumentNullException(nameof(nodesInGraph));
            } else if (foodSources == null)
            {
                throw new ArgumentNullException(nameof(foodSources));
            }
            FoodSources = foodSources;
            Logger.Trace("[Constructor : 3 param] Finished with foodSources.Count {0}, nodesInGraph.Count {1}, edgesINGraph.Count {2}",
                FoodSources.Count, NodesInGraph.Count, edgesInGraph.Count);
        }

        public IEnumerable<FoodSourceNode> FoodSourcesConnectedTo(FoodSourceNode node)
        {
            var connectedFood = AllNodesConnectedTo(node).Where(n => n is FoodSourceNode);
            return Nodes.CastToFood(connectedFood);
        }

        public new bool Equals(object obj)
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

        public new int GetHashCode()
        {
            return FoodSources.GetHashCode() * 17 + base.GetHashCode();
        }
    }
}
