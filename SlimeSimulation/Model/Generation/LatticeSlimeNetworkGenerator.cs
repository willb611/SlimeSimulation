using SlimeSimulation.Model;
using System.Collections.Generic;
using System;
using NLog;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Configuration;

namespace SlimeSimulation.Model.Generation
{
    public class LatticeSlimeNetworkGenerator : ISlimeNetworkGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Random _random = new Random();

        private bool _columnOffset = true;
        private HashSet<Node> _nodes = new HashSet<Node>();
        private HashSet<Edge> _edges = new HashSet<Edge>();
        private List<List<Node>> _nodeArray = new List<List<Node>>();
        private HashSet<FoodSourceNode> _foodSources = new HashSet<FoodSourceNode>();
        
        private readonly LatticeSlimeNetworkGenerationConfig _config;
        private readonly int _rows;
        private readonly int _columns;

        private bool _used = false;

        public LatticeSlimeNetworkGenerator(LatticeSlimeNetworkGenerationConfig config)
        {
            this._config = config;
            this._rows = config.Size;
            this._columns = config.Size;
        }

        private void Reset()
        {
            _nodeArray = new List<List<Node>>();
            _columnOffset = true;
            _nodes = new HashSet<Node>();
            _edges = new HashSet<Edge>();
            _foodSources = new HashSet<FoodSourceNode>();
        }

        public SlimeNetwork Generate()
        {
            if (_used)
            {
                throw new ApplicationException("Generator cannot be used more than once");
            }
            _used = true;
            Logger.Info("[generate] Generating lattice with rows: {0}, columns: {1}",
                _rows, _columns);
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
            for (int row = 1; row <= _rows; row++)
            {
                List<Node> rowNodes = new List<Node>();
                for (int col = 1; col <= _columns; col++)
                {
                    if (PointIsSkipped(row, col, _rows))
                    {
                        continue;
                    }
                    Node node = MakeNextNode(ref id, row, col, _foodSources, _columnOffset);
                    rowNodes.Add(node);
                    _nodes.Add(node);
                    MakeEdgesForNode(row, col, node, previousRowNodes);
                }
                previousRowNodes = rowNodes;
                _nodeArray.Add(rowNodes);
                _columnOffset = !_columnOffset;
            }
            EnsureFoodSourcesByReplacingNodesWithFoodSourceNodes();
            Graph graph = new Graph(_edges, _nodes);
            SlimeNetwork slimeSimulation = new SlimeNetwork(_nodes, _foodSources, _edges);
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
            if (PointIsSkipped(rowIndex + 1, colIndex + 1, _rows))
            {
                return;
            }
            if (colIndex > 0)
            {
                Node left = previousRowNodes[colIndex - 1];
                Edge leftEdge = new Edge(node, left, _config.StartingConnectivity);
                _edges.Add(leftEdge);
            }

            if (!PointIsSkipped(rowIndex, colIndex + 1, _rows))
            {
                Node right = previousRowNodes[colIndex];
                Edge rightEdge = new Edge(node, right, _config.StartingConnectivity);
                _edges.Add(rightEdge);
            }
        }

        private void MakeEdgesForEvenRow(int rowIndex, int colIndex, Node node, List<Node> previousRowNodes)
        {
            if (PointIsSkipped(rowIndex + 1, colIndex + 1, _rows))
            {
                return;
            }
            Node left = previousRowNodes[colIndex];
            Edge leftEdge = new Edge(node, left, _config.StartingConnectivity);
            _edges.Add(leftEdge);

            if (colIndex + 1 < _columns)
            {
                Node right = previousRowNodes[colIndex + 1];
                Edge rightEdge = new Edge(node, right, _config.StartingConnectivity);
                _edges.Add(rightEdge);
            }
        }

        private void EnsureFoodSourcesByReplacingNodesWithFoodSourceNodes()
        {
            List<Node> nodesList = new List<Node>(_nodes);
            while (_foodSources.Count < _config.MinimumFoodSources)
            {
                int index = _random.Next(_nodes.Count - 1);
                while (nodesList[index].IsFoodSource())
                {
                    index = _random.Next(_nodes.Count - 1);
                }
                Node nodeToReplace = nodesList[index];
                FoodSourceNode replacement = new FoodSourceNode(nodeToReplace.Id, nodeToReplace.X, nodeToReplace.Y);
                _foodSources.Add(replacement);
                _nodes.Remove(nodeToReplace);
                _nodes.Add(replacement);
                nodesList[index] = replacement;
                nodeToReplace.ReplaceWithGivenNodeInEdges(replacement, _edges);
            }
            if (Logger.IsTraceEnabled)
            {
                Logger.Trace("[EnsureFoodSourcesByReplacingNodesWithFoodSourceNodes] Finished. resulting edges: " +
                             LogHelper.CollectionToString(_edges));
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
            return _random.NextDouble() <= _config.ProbabilityNewNodeIsFoodSource;
        }
    }
}
