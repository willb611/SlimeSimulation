using SlimeSimulation.Model;
using System.Collections.Generic;
using System;
using NLog;
using SlimeSimulation.FlowCalculation;

namespace SlimeSimulation.Model {
    public class LatticeSlimeNetworkGenerator {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private double PROBABILITY_NEW_NODE_IS_FOOD = 0.05;
        private Random random = new Random();
        private double STARTING_CONNECTIVITY = 1;

        private bool columnOffset = true;
        private HashSet<Node> nodes = new HashSet<Node>();
        private HashSet<Edge> edges = new HashSet<Edge>();
        private List<List<Node>> nodeArray = new List<List<Node>>();
        private HashSet<FoodSourceNode> foodSources = new HashSet<FoodSourceNode>();

        private int columns, rows;

        public SlimeNetwork Generate(int size) {
            if (size < 3) {
                throw new ArgumentException("SIze must be > 3. Given: " + size);
            }
            logger.Info("[generate] Generating lattice size: " + size);
            return GenerateLatticeNetwork(size, size);
        }

        private void Reset(int rows, int columns) {
            nodeArray = new List<List<Node>>();
            columnOffset = true;
            nodes = new HashSet<Node>();
            edges = new HashSet<Edge>();
            foodSources = new HashSet<FoodSourceNode>();
            this.columns = columns;
            this.rows = rows;
        }

        private SlimeNetwork GenerateLatticeNetwork(int columns, int rows) {
            if (rows * columns < 2) {
                throw new ArgumentException("Given rows: " + rows + ", columns: " + columns + ". Needed col*rows > " + 2);
            } else if (rows < 0) {
                throw new ArgumentException("Cannot provide generate lattice with negative amount of rows. Given: " + rows);
            }
            Reset(rows, columns);
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
            for (int row = 1; row <= rows; row++) {
                List<Node> rowNodes = new List<Node>();
                for (int col = 1; col <= columns; col++) {
                    if (PointIsSkipped(row, col, rows)) {
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


        private bool PointIsSkipped(int row, int col, int maxRows) {
            if (row == 1 && col == maxRows) {
                return true;
            } else if (row == maxRows) {
                if ((Even(maxRows) && col == 1)
                    || (!Even(row) && col == maxRows)) {
                    return true;
                }
            }
            return false;
        }

        private void MakeEdgesForNode(int row, int col, Node node, List<Node> previousRowNodes) {
            int rowIndex = row - 1;
            int colIndex = col - 1;
            if (rowIndex > 0) {
                if (Even(rowIndex)) {
                    MakeEdgesForEvenRow(rowIndex, colIndex, node, previousRowNodes);
                } else {
                    MakeEdgesForOddRow(rowIndex, colIndex, node, previousRowNodes);
                }
            }
        }

        private void MakeEdgesForOddRow(int rowIndex, int colIndex, Node node, List<Node> previousRowNodes) {
            if (PointIsSkipped(rowIndex + 1, colIndex + 1, rows)) {
                return;
            }
            if (colIndex > 0) {
                Node left = previousRowNodes[colIndex - 1];
                Edge leftEdge = new Edge(node, left, STARTING_CONNECTIVITY);
                edges.Add(leftEdge);
            }

            if (!PointIsSkipped(rowIndex, colIndex + 1, rows)) {
                Node right = previousRowNodes[colIndex];
                Edge rightEdge = new Edge(node, right, STARTING_CONNECTIVITY);
                edges.Add(rightEdge);
            }
        }
        private void MakeEdgesForEvenRow(int rowIndex, int colIndex, Node node, List<Node> previousRowNodes) {
            if (PointIsSkipped(rowIndex + 1, colIndex + 1, rows)) {
                return;
            }
            Node left = previousRowNodes[colIndex];
            Edge leftEdge = new Edge(node, left, STARTING_CONNECTIVITY);
            edges.Add(leftEdge);

            if (colIndex + 1 < columns) {
                Node right = previousRowNodes[colIndex + 1];
                Edge rightEdge = new Edge(node, right, STARTING_CONNECTIVITY);
                edges.Add(rightEdge);
            }
        }

        private void EnsureFoodSourcesByReplacingNodesWithFoodSourceNodes() {
            List<Node> nodesList = new List<Node>(nodes);
            while (foodSources.Count < 2) {
                int index = random.Next(nodes.Count - 1);
                while (nodesList[index].IsFoodSource()) {
                    index = random.Next(nodes.Count - 1);
                }
                Node nodeToReplace = nodesList[index];
                FoodSourceNode replacement = new FoodSourceNode(nodeToReplace.Id, nodeToReplace.X, nodeToReplace.Y);
                foodSources.Add(replacement);
                nodes.Remove(nodeToReplace);
                nodes.Add(replacement);
                nodesList[index] = replacement;
                UpdateEdgesWithReplacement(edges, nodeToReplace, replacement);
                logger.Trace("[EnsureFoodSources] Replacing " + nodeToReplace + ", with: " + replacement);
            }
            logger.Trace("[EnsureFoodSources] Finished. resulting edges: " + LogHelper.CollectionToString(edges));
        }
        internal void UpdateEdgesWithReplacement(HashSet<Edge> edges, Node nodeToReplace, Node replacement) {
            foreach (Edge edge in nodeToReplace.GetEdgesAdjacent(edges)) {
                Node otherNode = edge.GetOtherNode(nodeToReplace);
                Edge replacementEdge = new Edge(replacement, otherNode, edge.Connectivity);
                edges.Remove(edge);
                edges.Add(replacementEdge);
            }
            logger.Trace("[EnsureFoodSources] Finished. resulting edges: " + LogHelper.CollectionToString(edges));
        }

        private bool Even(int i) {
            return i % 2 == 0;
        }

        private Node MakeNextNode(ref int id, int row, int col, HashSet<FoodSourceNode> foodSources, bool columnOffset) {
            int x = 2 * col;
            if (columnOffset) {
                x++;
            }
            int y = row * 2;
            Node node;
            if (IsFoodSource()) {
                node = new FoodSourceNode(id++, x, y);
                foodSources.Add(node as FoodSourceNode);
            } else {
                node = new Node(id++, x, y);
            }
            return node;
        }

        private bool IsFoodSource() {
            return random.NextDouble() <= PROBABILITY_NEW_NODE_IS_FOOD;
        }
    }
}
