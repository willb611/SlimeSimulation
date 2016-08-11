using SlimeSimulation.Model;
using System.Collections.Generic;
using System;
using NLog;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Configuration;

namespace SlimeSimulation.Model.Generation
{
    public class LatticeSlimeNetworkGenerator : SlimeNetworkGenerator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private Random random = new Random();

        private bool columnOffset = true;
        private HashSet<Node> nodes = new HashSet<Node>();
        private HashSet<Edge> edges = new HashSet<Edge>();
        private List<List<Node>> nodeArray = new List<List<Node>>();
        private HashSet<FoodSourceNode> foodSources = new HashSet<FoodSourceNode>();
        
        private LatticeSlimeNetworkGenerationConfig config;
        private int rows, columns;

        private bool _used = false;

        public LatticeSlimeNetworkGenerator(LatticeSlimeNetworkGenerationConfig config)
        {
            this.config = config;
            this.rows = config.Size;
            this.columns = config.Size;
        }

        private void Reset()
        {
            nodeArray = new List<List<Node>>();
            columnOffset = true;
            nodes = new HashSet<Node>();
            edges = new HashSet<Edge>();
            foodSources = new HashSet<FoodSourceNode>();
        }

        public SlimeNetwork Generate()
        {
            if (_used)
            {
                throw new ApplicationException("Generator cannot be used more than once");
            }
            _used = true;
            logger.Info("[generate] Generating lattice with rows: {0}, columns: {1}",
                rows, columns);
            Reset();
            int id = 1;
            List<Node> previousRowNodes = new List<Node>();
            /*Construct like:
             *   o    o    o   o
             *  / \  /  \ / \ /
             * o    o    o   o
             *  \  / \  / \ / \
             *   o    o    o   o
             *  / \  /  \ / \ /
             * o    o    o   o
             *
             * etc
             *
             * */
            for (int row = 1; row <= rows; row++)
            {
                List<Node> rowNodes = new List<Node>();
                for (int col = 1; col <= columns; col++)
                {
                    if (PointIsSkipped(row, col, rows))
                    {
                        continue;
                    }
                    Node node = MakeNextNode(ref id, row, col, foodSources, columnOffset);
                    rowNodes.Add(node);
                    nodes.Add(node);
                    MakeEdgesForNode(row, col, node, previousRowNodes);
                }
                previousRowNodes = rowNodes;
                nodeArray.Add(rowNodes);
                columnOffset = !columnOffset;
            }
            EnsureFoodSourcesByReplacingNodesWithFoodSourceNodes();
            Graph graph = new Graph(edges, nodes);
            SlimeNetwork slimeSimulation = new SlimeNetwork(nodes, foodSources, edges);
            return slimeSimulation;
        }


        private bool PointIsSkipped(int row, int col, int maxRows)
        {
            if (row == 1 && col == maxRows)
            {
                return true;
            }
            else if (row == maxRows)
            {
                if ((Even(maxRows) && col == 1)
                    || (!Even(row) && col == maxRows))
                {
                    return true;
                }
            }
            return false;
        }

        private void MakeEdgesForNode(int row, int col, Node node, List<Node> previousRowNodes)
        {
            int rowIndex = row - 1;
            int colIndex = col - 1;
            if (rowIndex > 0)
            {
                if (Even(rowIndex))
                {
                    MakeEdgesForEvenRow(rowIndex, colIndex, node, previousRowNodes);
                }
                else
                {
                    MakeEdgesForOddRow(rowIndex, colIndex, node, previousRowNodes);
                }
            }
        }

        private void MakeEdgesForOddRow(int rowIndex, int colIndex, Node node, List<Node> previousRowNodes)
        {
            if (PointIsSkipped(rowIndex + 1, colIndex + 1, rows))
            {
                return;
            }
            if (colIndex > 0)
            {
                Node left = previousRowNodes[colIndex - 1];
                Edge leftEdge = new Edge(node, left, config.StartingConnectivity);
                edges.Add(leftEdge);
            }

            if (!PointIsSkipped(rowIndex, colIndex + 1, rows))
            {
                Node right = previousRowNodes[colIndex];
                Edge rightEdge = new Edge(node, right, config.StartingConnectivity);
                edges.Add(rightEdge);
            }
        }

        private void MakeEdgesForEvenRow(int rowIndex, int colIndex, Node node, List<Node> previousRowNodes)
        {
            if (PointIsSkipped(rowIndex + 1, colIndex + 1, rows))
            {
                return;
            }
            Node left = previousRowNodes[colIndex];
            Edge leftEdge = new Edge(node, left, config.StartingConnectivity);
            edges.Add(leftEdge);

            if (colIndex + 1 < columns)
            {
                Node right = previousRowNodes[colIndex + 1];
                Edge rightEdge = new Edge(node, right, config.StartingConnectivity);
                edges.Add(rightEdge);
            }
        }

        private void EnsureFoodSourcesByReplacingNodesWithFoodSourceNodes()
        {
            List<Node> nodesList = new List<Node>(nodes);
            while (foodSources.Count < config.MinimumFoodSources)
            {
                int index = random.Next(nodes.Count - 1);
                while (nodesList[index].IsFoodSource())
                {
                    index = random.Next(nodes.Count - 1);
                }
                Node nodeToReplace = nodesList[index];
                FoodSourceNode replacement = new FoodSourceNode(nodeToReplace.Id, nodeToReplace.X, nodeToReplace.Y);
                foodSources.Add(replacement);
                nodes.Remove(nodeToReplace);
                nodes.Add(replacement);
                nodesList[index] = replacement;
                nodeToReplace.ReplaceWithGivenNodeInEdges(replacement, edges);
            }
            if (logger.IsTraceEnabled)
            {
                logger.Trace("[EnsureFoodSourcesByReplacingNodesWithFoodSourceNodes] Finished. resulting edges: " +
                             LogHelper.CollectionToString(edges));
            }
        }

        private bool Even(int i)
        {
            return i % 2 == 0;
        }

        private Node MakeNextNode(ref int id, int row, int col, HashSet<FoodSourceNode> foodSources, bool columnOffset)
        {
            int x = 2 * col;
            if (columnOffset)
            {
                x++;
            }
            int y = row * 2;
            Node node;
            if (IsFoodSource())
            {
                node = new FoodSourceNode(id++, x, y);
                foodSources.Add(node as FoodSourceNode);
            }
            else
            {
                node = new Node(id++, x, y);
            }
            return node;
        }

        private bool IsFoodSource()
        {
            return random.NextDouble() <= config.ProbabilityNewNodeIsFoodSource;
        }
    }
}
