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

        private readonly ConfigForGraphGenerator _config;
        private readonly Random _random = new Random();
        private int _nextId = 1;

        public GridGraphWithFoodSourcesGenerator(ConfigForGraphGenerator config)
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
            ISet<FoodSourceNode> foodSources = new HashSet<FoodSourceNode>();
            int rowLimit = _config.Size;
            int colLimit = rowLimit;
            var nodesAs2DArray = GenerateNodes(_config, foodSources);
            var edges = GenerateEdges(nodesAs2DArray, rowLimit, colLimit, _config.EdgeConnectionType);
            Logger.Debug("[ActuallyGenerate] Finished with edges.Count: {0}.", edges.Count);
            var result = new GraphWithFoodSources(edges);
            if (result.FoodSources.Count != foodSources.Count)
            {
                Logger.Warn("Some food sources not in the generated graph.");
            }
            return result;
        }

        private List<List<Node>> GenerateNodes(ConfigForGraphGenerator config, ISet<FoodSourceNode> foodSources)
        {
            List<List<Node>> nodesAs2DArray = new List<List<Node>>();
            ISet<Node> nodes = new HashSet<Node>();
            int rowLimit = config.Size;
            int colLimit = rowLimit;
            int totalNodes = rowLimit * colLimit;
            int foodSourcesLeftToMake = GetNumberOfFoodSources(config.MinimumFoodSources, config.ProbabilityNewNodeIsFoodSource, totalNodes);
            for (int row = 1; row <= rowLimit; row++)
            {
                nodesAs2DArray.Add(new List<Node>());
                for (int col = 1; col <= colLimit; col++)
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
                    nodesAs2DArray[row - 1].Add(node);
                    nodes.Add(node);
                }
            }
            return nodesAs2DArray;
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
