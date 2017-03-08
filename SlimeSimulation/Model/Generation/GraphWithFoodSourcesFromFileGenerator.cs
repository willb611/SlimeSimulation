using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using NLog;
using SlimeSimulation.Configuration;

namespace SlimeSimulation.Model.Generation
{
    public class GraphWithFoodSourcesFromFileGenerator : IGraphWithFoodSourcesGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _filepath;

        public GraphWithFoodSourcesFromFileGenerator(GraphWithFoodSourceGenerationConfig config) : this(config.FileToLoadFrom)
        {
        }
        public GraphWithFoodSourcesFromFileGenerator(string filepath)
        {
            _filepath = filepath;
        }

        public GraphWithFoodSources Generate()
        {
            try
            {
                string fileAsText = File.ReadAllText(_filepath);
                return CreateGraphFromDescription(fileAsText);
            }
            catch (Exception e)
            {
                Logger.Error("Error: " + e);
                return null;
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
                int rows = Int32.Parse(reader.ReadLine());
                int cols = Int32.Parse(reader.ReadLine());
                int numberOfFoodSources = Int32.Parse(reader.ReadLine());
                HashSet<FoodSourceNode> foodSources = new HashSet<FoodSourceNode>();
                HashSet<Node> nodes = new HashSet<Node>();
                int nextId = 0;
                for (int i = 0; i < numberOfFoodSources; i++)
                {
                    foodSources.Add(GetFoodSourceFromLine(reader.ReadLine(), ref nextId));
                }
                Logger.Debug("Creating with {0} rows, {1} cols, {2} foodSources", rows, cols, numberOfFoodSources);
                // Do some stuff
                ISet<Edge> edges = new HashSet<Edge>();
                var previousRowNodes = new List<Node>();
                for (int rowNum = 0; rowNum < rows; rowNum++)
                {
                    var rowNodes = new List<Node>();
                    for (int colNum = 0; colNum < cols; colNum++)
                    {
                        Node node = GetNodeFoodSource(colNum, rowNum, foodSources);
                        if (node == null) {
                            node = new Node(nextId++, colNum, rowNum);
                        }
                        var edgesForNode = MakeEdgesForNode(colNum, node, rowNodes, previousRowNodes);
                        edges.UnionWith(edgesForNode);
                        rowNodes.Add(node);
                        nodes.Add(node);
                    }
                    previousRowNodes = rowNodes;
                }
                return new GraphWithFoodSources(edges, nodes, foodSources);
            }
        }
        
        private ISet<Edge> MakeEdgesForNode(int col, Node node, List<Node> rowNodes, List<Node> previousRowNodes)
        {
            Logger.Debug("[MakeEdgesForNode] col: {0}, rowNodes.Count: {1}, prevRowNodes.Count: {2}", col, rowNodes.Count, previousRowNodes.Count);
            var edges = new HashSet<Edge>();
            if (previousRowNodes != null && previousRowNodes.Count > col)
            {
                Logger.Debug("Enter previousRowNodeCheck");
                edges.Add(new Edge(node, previousRowNodes[col]));
            }
            if (col >= rowNodes.Count && rowNodes.Count >= 1)
            {
                Logger.Debug("Enter rowNodeCheck");
                edges.Add(new Edge(node, rowNodes[col - 1]));
            }
            return edges;
        }

        private Node GetNodeFoodSource(int x, int y, HashSet<FoodSourceNode> foodSources)
        {
            foreach (FoodSourceNode foodSource in foodSources)
            {
                if (foodSource.X == x && foodSource.Y == y)
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
