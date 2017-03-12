using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using SlimeSimulation.Configuration;

namespace SlimeSimulation.Model.Generation
{
    public class GridGraphWithFoodSourcesGenerator : GraphWithFoodSourcesGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly LatticeGraphWithFoodSourcesGenerationConfig _config;
        private readonly Random _random = new Random();
        private int _nextId = 1;

        public GridGraphWithFoodSourcesGenerator(LatticeGraphWithFoodSourcesGenerationConfig config)
        {
            _config = config;
        }

        public override GraphWithFoodSources Generate()
        {
            try
            {
                return ActuallyGenerate();
            }
            catch (Exception e)
            {
                Logger.Error(e, $"[Generate] Error! {e}", e);
                throw e;
            }
        }

        public GraphWithFoodSources ActuallyGenerate()
        {
            ISet<Node> nodes = new HashSet<Node>();
            ISet<Edge> edges = new HashSet<Edge>();
            ISet<FoodSourceNode> foodSources = new HashSet<FoodSourceNode>();
            /*Construct like:
            * 
            * o-o-o
            * | | |
            * o-o-o
            * 
            */
            int rowLimit = _config.Size;
            int colLimit = rowLimit;
            int totalNodes = rowLimit * colLimit;
            var previousRowNodes = new List<Node>();
            int foodSourcesLeftToMake = GetNumberOfFoodSources(_config.MinimumFoodSources, _config.ProbabilityNewNodeIsFoodSource, totalNodes);
            for (var row = 1; row <= rowLimit; row++)
            {
                Logger.Debug("Row: {0}", row);
                var rowNodes = new List<Node>();
                for (var col = 1; col <= colLimit; col++)
                {
                    Node node;
                    if (IsNodeFoodSource(totalNodes - nodes.Count, foodSourcesLeftToMake))
                    {
                        node = MakeFoodSourceNode(ref _nextId, row, col);
                        foodSourcesLeftToMake--;
                        foodSources.Add((FoodSourceNode)node);
                    }
                    else
                    {
                        node = MakeNode(ref _nextId, row, col);
                    }
                    rowNodes.Add(node);
                    nodes.Add(node);
                }
                edges.UnionWith(CreateEdgesBetweenNodesInOrder(rowNodes));
                edges.UnionWith(CreateEdgesBetweenRowsAtSameIndex(rowNodes, previousRowNodes));
                previousRowNodes = rowNodes;
            }
            Logger.Debug("Finished with edges.Count: {0}. Nodes.Count: {1}", edges.Count, nodes.Count);
            return new GraphWithFoodSources(edges, nodes, foodSources);
        }

        private bool IsNodeFoodSource(int nodesLeftToMake, int foodSourcesLeftToMake)
        {
            if (foodSourcesLeftToMake >= nodesLeftToMake)
            {
                return true;
            }
            else
            {
                return _random.NextDouble() < foodSourcesLeftToMake / (double) nodesLeftToMake;
            }
        }

        private int GetNumberOfFoodSources(int minimumFoodSources, double configProbabilityNewNodeIsFoodSource, int totalNodes)
        {
            int possibleFoodSources = totalNodes - minimumFoodSources;
            int actualFoodSourceCount = minimumFoodSources;
            for (int i = 0; i < possibleFoodSources; i++)
            {
                if (_random.NextDouble() <= configProbabilityNewNodeIsFoodSource)
                {
                    actualFoodSourceCount++;
                }
            }
            return actualFoodSourceCount;
        }
        
        private Node MakeNode(ref int id, int row, int col)
        {
            return new Node(id++, row, col);
        }

        private FoodSourceNode MakeFoodSourceNode(ref int id, int row, int col)
        {
            return new FoodSourceNode(id++, row, col);
        }
    }
}
