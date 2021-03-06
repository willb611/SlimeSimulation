using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using NLog;
using SlimeSimulation.Configuration;

namespace SlimeSimulation.Model.Generation
{
    public class GraphWithFoodSourcesFromFileGenerator : GraphWithFoodSourcesGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _filepath;
        private ConfigForGraphGenerator _configForGraphGenerator;
        private const double PointComparisonEqualityTolerance = 0.000001;

        private int EdgeConnectionType
        {
            get
            {
                if (_configForGraphGenerator != null)
                {
                    return _configForGraphGenerator.EdgeConnectionType;
                }
                return EdgeConnectionShape.DefaultEdgeConnectionType;
            }
        }

        public GraphWithFoodSourcesFromFileGenerator(ConfigForGraphGenerator config, string filepath)
        {
            if (String.IsNullOrWhiteSpace(filepath))
            {
                throw new ArgumentException("Given empty or null argument " + (nameof(filepath)));
            }
            _filepath = filepath;
            _configForGraphGenerator = config;
        }

        public override GraphWithFoodSources Generate()
        {
            try
            {
                string fileAsText = ReadDescriptionFromFile(_filepath);
                return CreateGraphFromDescription(fileAsText);
            }
            catch (Exception e)
            {
                Logger.Error("Error: " + e);
                throw;
            }
        }

        private string ReadDescriptionFromFile(string filepath)
        {
            try
            {
                return File.ReadAllText(_filepath);
            }
            catch (Exception e)
            {
                throw new IOException("Unable to read from file at location: "+ filepath, e);
            }
        }

        /*
         * -- File setup: 
         * rows
         * columns
         * foodSources
         * -- for i = 1 to foodSources
         * x,y
         */
        internal GraphWithFoodSources CreateGraphFromDescription(string fileAsText)
        {
            using (System.IO.StringReader reader = new System.IO.StringReader(fileAsText))
            {
                int rowLimit = Int32.Parse(reader.ReadLine());
                int colLimit = Int32.Parse(reader.ReadLine());
                string thirdParameter = reader.ReadLine();
                if (thirdParameter.Equals("grid"))
                {
                    return ReadInGrid(rowLimit, colLimit, reader);
                }
                int numberOfFoodSources = Int32.Parse(thirdParameter);
                HashSet<FoodSourceNode> foodSources = new HashSet<FoodSourceNode>();
                int nextId = 0;
                for (int i = 0; i < numberOfFoodSources; i++)
                {
                    foodSources.Add(GetFoodSourceFromLine(reader.ReadLine(), ref nextId));
                }
                
                Logger.Debug("Creating with {0} rows, {1} cols, {2} foodSources", rowLimit, colLimit, numberOfFoodSources);
                List<List<Node>> nodesAs2DArray = new List<List<Node>>();
                for (int row = 0; row < rowLimit; row++)
                {
                    nodesAs2DArray.Add(new List<Node>());
                    for (int col = 0; col < colLimit; col++)
                    {
                        Node node = GetOrMakeNode(ref nextId, row, col, foodSources);
                        nodesAs2DArray[row].Add(node);
                    }
                }
                ISet<Edge> edges = GenerateEdges(nodesAs2DArray, rowLimit, colLimit, EdgeConnectionType);
                var result = new GraphWithFoodSources(edges);
                if (result.FoodSources.Count != foodSources.Count)
                {
                    Logger.Warn("Some food sources not in the generated graph.");
                }
                return result;
            }
        }

        private GraphWithFoodSources ReadInGrid(int rowLimit, int colLimit, StringReader reader)
        {
            int id = 0;
            List<List<Node>> grid = new List<List<Node>>();
            for (int x = 0; x < colLimit; x++)
            {
                List<Node> row = new List<Node>();
                string line = reader.ReadLine();
                for (int y = rowLimit - 1; y >= 0; y--)
                {
                    char c = line[y];
                    if (c == 'n')
                    {
                        row.Add(new Node(id++, y, x));
                    } else if (c == 'f')
                    {
                        row.Add(new FoodSourceNode(id++, y, x));
                    }
                    else
                    {
                        row.Add(null);
                    }
                }
                grid.Add(row);
            }
            var edges = GenerateEdges(grid, rowLimit, colLimit, EdgeConnectionType);
            return new GraphWithFoodSources(edges);
        }

        private Node GetOrMakeNode(ref int nextId, int row, int col, HashSet<FoodSourceNode> foodSources)
        {
            Node node = GetFoodSourceAtPositionOrNull(col, row, foodSources);
            if (node == null)
            {
                node = new Node(nextId++, col, row);
            }
            return node;
        }

        private Node GetFoodSourceAtPositionOrNull(int x, int y, HashSet<FoodSourceNode> foodSources)
        {
            foreach (FoodSourceNode foodSource in foodSources)
            {
                if (Math.Abs(foodSource.X - x) < PointComparisonEqualityTolerance && Math.Abs(foodSource.Y - y) < PointComparisonEqualityTolerance)
                {
                    return foodSource;
                }
            }
            return null;
        }

        private FoodSourceNode GetFoodSourceFromLine(string readLine, ref int nextId)
        {
            string[] valuesSplit = readLine.Split(',');
            int x = Int32.Parse(valuesSplit[0]);
            int y = Int32.Parse(valuesSplit[1]);
            return new FoodSourceNode(nextId++, x, y);
        }
    }
}
