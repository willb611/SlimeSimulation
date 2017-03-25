using System.Collections.Generic;
using Newtonsoft.Json;
using NLog;
using SlimeSimulation.Model.Simulation.Persistence;

namespace SlimeSimulation.Model.Generation
{
    public abstract class GraphWithFoodSourcesGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public abstract GraphWithFoodSources Generate();

        public ISet<Edge> GenerateEdges(List<List<Node>> nodesAs2DArray, int rowLimit, int colLimit, int edgeConnectionType)
        {
            Logger.Debug("[GenerateEdges] EdgeConnectionType: {0}, rowLimit: {1}, colLimit: {2}",
                edgeConnectionType, rowLimit, colLimit);
            ISet<Edge> edges = new HashSet<Edge>();
            var previousRowNodes = new List<Node>();
            for (int row = 0; row < rowLimit; row++)
            {
                var rowNodes = new List<Node>();
                for (int col = 0; col < colLimit; col++)
                {
                    Node node = nodesAs2DArray[row][col];
                    rowNodes.Add(node);
                    Logger.Debug("At row {0}, col {1} got node {2}", row, col, node);
                }
                edges.UnionWith(CreateEdgesBetweenNodesInOrder(rowNodes));
                edges.UnionWith(CreateEdgesBetweenRowsAtSameIndex(rowNodes, previousRowNodes));
                if (edgeConnectionType == EdgeConnectionShape.EdgeConnectionShapeSquareWithDiamonds)
                {
                    if (row % 2 == 1)
                    {
                        edges.UnionWith(CreateEdgesLikeSnakeFromBottomToTop(rowNodes, previousRowNodes));
                    }
                    else
                    {
                        edges.UnionWith(CreateEdgesLikeSnakeFromTopToBottom(rowNodes, previousRowNodes));
                    }
                } else if (edgeConnectionType == EdgeConnectionShape.EdgeConnectionShapeSquareWithCrossedDiagonals)
                {
                    edges.UnionWith(CreateEdgesLikeSnakeFromBottomToTop(rowNodes, previousRowNodes));
                    edges.UnionWith(CreateEdgesLikeSnakeFromTopToBottom(rowNodes, previousRowNodes));
                }
                previousRowNodes = rowNodes;
            }
            Logger.Debug("[GenerateEdges] returning result size {0}", edges.Count);
            Logger.Debug("[GenerateEdges] result: {0}", JsonConvert.SerializeObject(edges, SerializationSettings.JsonSerializerSettings));
            return edges;
        }

        internal ISet<Edge> CreateEdgesLikeSnakeFromTopToBottom(List<Node> top, List<Node> bottom)
        {
            Logger.Debug("[CreateEdgesLikeSnakeFromTopToBottom] Entered with top.Count: {0}, bottom.Count: {1}",
                top.Count, bottom.Count);
            var result = new HashSet<Edge>();
            for (int startIndex = 0; startIndex + 1 < top.Count 
                                    && startIndex + 1 < bottom.Count; startIndex++)
            {
                Logger.Debug("[CreateEdgesLikeSnakeFromTopToBottom] StartIndex: {0}", startIndex);
                bool shouldStartFromTop = (startIndex % 2 == 0);
                if (shouldStartFromTop)
                {
                    var edge = new Edge(top[startIndex], bottom[startIndex + 1]);
                    Logger.Debug("[CreateEdgesLikeSnakeFromTopToBottom] Starting from top created edge {0}", edge);
                    result.Add(edge);
                }
                else
                {
                    var edge = new Edge(bottom[startIndex], top[startIndex + 1]);
                    Logger.Debug("[CreateEdgesLikeSnakeFromTopToBottom] Starting from bottom created edge {0}", edge);
                    result.Add(edge);
                }
            }
            Logger.Debug("[CreateEdgesLikeSnakeFromTopToBottom] Returning result size: {0}", result.Count);
            return result;
        }

        internal ISet<Edge> CreateEdgesLikeSnakeFromBottomToTop(List<Node> bottom, List<Node> top)
        {
            return CreateEdgesLikeSnakeFromTopToBottom(top, bottom);
        }

        internal ISet<Edge> CreateEdgesBetweenNodesInOrder(List<Node> row)
        {
            if (row == null)
            {
                Logger.Warn("[CreateEdgesBetweenNodesInOrder] Given null row");
                return new HashSet<Edge>();
            }
            var result = new HashSet<Edge>();
            for (int i = 1; i < row.Count; i++)
            {
                if (row[i] == null || row[i-1] == null)
                {
                    continue;
                }
                Edge e = new Edge(row[i-1], row[i]);
                Logger.Debug("[CreateEdgesBetweenNodesInOrder] At index {0} created edge {1}", i, e);
                result.Add(e);
            }
            Logger.Debug("[CreateEdgesBetweenNodesInOrder] Returning result size: {0}", result.Count);
            return result;
        }

        internal ISet<Edge> CreateEdgesBetweenRowsAtSameIndex(List<Node> row, List<Node> otherRow)
        {
            if (row == null || otherRow == null)
            {
                Logger.Warn("[CreateEdgesBetweenRowsAtSameIndex] Given null row");
                return new HashSet<Edge>();
            }
            var result = new HashSet<Edge>();
            for (int i = 0; i < row.Count && i < otherRow.Count; i++)
            {
                if (row[i] == null || otherRow[i] == null)
                {
                    continue;
                }
                Edge e = new Edge(row[i], otherRow[i]);
                Logger.Debug("[CreateEdgesBetweenRowsAtSameIndex] At index {1} Created edge: {0}", e, i);
                result.Add(e);
            }
            Logger.Debug("[CreateEdgesBetweenRowsAtSameIndex] Returning result size: {0}", result.Count);
            return result;
        }
    }
}
