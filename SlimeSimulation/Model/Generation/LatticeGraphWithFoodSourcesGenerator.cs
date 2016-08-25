using SlimeSimulation.Model;
using System.Collections.Generic;
using System;
using NLog;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Configuration;

namespace SlimeSimulation.Model.Generation
{
    public class LatticeGraphWithFoodSourcesGenerator : IGraphWithFoodSourcesGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Random _random = new Random();

        private bool _columnOffset = true;
        private HashSet<Node> _nodes = new HashSet<Node>();
        private HashSet<Edge> _edges = new HashSet<Edge>();
        private HashSet<FoodSourceNode> _foodSources = new HashSet<FoodSourceNode>();
        
        private readonly LatticeGraphWithFoodSourcesGenerationConfig _config;
        private readonly int _rows;
        private readonly int _columns;

        private bool _used = false;

        public LatticeGraphWithFoodSourcesGenerator(LatticeGraphWithFoodSourcesGenerationConfig config)
        {
            _config = config;
            _rows = config.Size;
            _columns = config.Size;
        }

        private void Reset()
        {
            _columnOffset = true;
            _nodes = new HashSet<Node>();
            _edges = new HashSet<Edge>();
            _foodSources = new HashSet<FoodSourceNode>();
        }

        public GraphWithFoodSources Generate()
        {
            if (_used)
            {
                throw new ApplicationException("Generator cannot be used more than once");
            }
            _used = true;
            Logger.Info("[generate] Generating lattice with rows: {0}, columns: {1}",
                _rows, _columns);
            Reset();
            var id = 1;
            var previousRowNodes = new List<Node>();
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
            for (var row = 1; row <= _rows; row++)
            {
                var rowNodes = new List<Node>();
                for (var col = 1; col <= _columns; col++)
                {
                    if (PointIsSkipped(row, col, _rows))
                    {
                        continue;
                    }
                    var node = MakeNextNode(ref id, row, col, _foodSources, _columnOffset);
                    rowNodes.Add(node);
                    _nodes.Add(node);
                    MakeEdgesForNode(row, col, node, previousRowNodes);
                }
                previousRowNodes = rowNodes;
                _columnOffset = !_columnOffset;
            }
            EnsureFoodSourcesByReplacingNodesWithFoodSourceNodes();
            return new GraphWithFoodSources(_edges, _nodes, _foodSources);
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
            var rowIndex = row - 1;
            var colIndex = col - 1;
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
                var left = previousRowNodes[colIndex - 1];
                var leftSlimeEdge = new Edge(node, left);
                _edges.Add(leftSlimeEdge);
            }

            if (!PointIsSkipped(rowIndex, colIndex + 1, _rows))
            {
                var right = previousRowNodes[colIndex];
                var rightSlimeEdge = new Edge(node, right);
                _edges.Add(rightSlimeEdge);
            }
        }

        private void MakeEdgesForEvenRow(int rowIndex, int colIndex, Node node, List<Node> previousRowNodes)
        {
            if (PointIsSkipped(rowIndex + 1, colIndex + 1, _rows))
            {
                return;
            }
            var left = previousRowNodes[colIndex];
            var leftSlimeEdge = new Edge(node, left);
            _edges.Add(leftSlimeEdge);

            if (colIndex + 1 < _columns)
            {
                var right = previousRowNodes[colIndex + 1];
                var rightSlimeEdge = new Edge(node, right);
                _edges.Add(rightSlimeEdge);
            }
        }

        private void EnsureFoodSourcesByReplacingNodesWithFoodSourceNodes()
        {
            var nodesList = new List<Node>(_nodes);
            while (_foodSources.Count < _config.MinimumFoodSources)
            {
                var index = _random.Next(_nodes.Count - 1);
                while (nodesList[index].IsFoodSource())
                {
                    index = _random.Next(_nodes.Count - 1);
                }
                var nodeToReplace = nodesList[index];
                var replacement = new FoodSourceNode(nodeToReplace.Id, nodeToReplace.X, nodeToReplace.Y);
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
            var x = 2 * col;
            if (columnOffset)
            {
                x++;
            }
            var y = row * 2;
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
