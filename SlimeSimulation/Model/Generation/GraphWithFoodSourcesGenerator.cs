using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NLog;

namespace SlimeSimulation.Model.Generation
{
    public abstract class GraphWithFoodSourcesGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static readonly int EdgeConnectionTypeSquare = 1;
        public static readonly int EdgeConnectionTypeSquareWithDiamonds = 2;
        public static int DefaultEdgeConnectionType => EdgeConnectionTypeSquareWithDiamonds;

        public abstract GraphWithFoodSources Generate();

        public ISet<Edge> GenerateEdges(List<List<Node>> nodesAs2DArray, int rowLimit, int colLimit, int edgeConnectionType)
        {
            Logger.Debug("[CreateGraphFromDescription] EdgeConnectionType: {0}", edgeConnectionType);
            ISet<Edge> edges = new HashSet<Edge>();
            var previousRowNodes = new List<Node>();
            for (int row = 0; row < rowLimit; row++)
            {
                var rowNodes = new List<Node>();
                for (int col = 0; col < colLimit; col++)
                {
                    Node node = nodesAs2DArray[row][col];
                    rowNodes.Add(node);
                }
                edges.UnionWith(CreateEdgesBetweenNodesInOrder(rowNodes));
                edges.UnionWith(CreateEdgesBetweenRowsAtSameIndex(rowNodes, previousRowNodes));
                if (edgeConnectionType == EdgeConnectionTypeSquareWithDiamonds)
                {
                    if (row % 2 == 0)
                    {
                        edges.UnionWith(CreateEdgesLikeSnakeFromBottomToTop(rowNodes, previousRowNodes));
                    }
                    else
                    {
                        edges.UnionWith(CreateEdgesLikeSnakeFromTopToBottom(rowNodes, previousRowNodes));
                    }
                }
                previousRowNodes = rowNodes;
            }
            return edges;
        }

        internal ISet<Edge> CreateEdgesLikeSnakeFromTopToBottom(List<Node> row, List<Node> otherRow)
        {
            Logger.Debug("[CreateEdgesLikeSnakeFromTopToBottom] Entered with row.Count: {0}, otherRow.Count: {1}", 
                row.Count, otherRow.Count);
            var result = new HashSet<Edge>();
            for (int startIndex = 0; startIndex + 1 < row.Count 
                                    && startIndex + 1 < otherRow.Count; startIndex++)
            {
                bool shouldStartFromTop = startIndex % 2 == 0;
                if (shouldStartFromTop)
                {
                    result.Add(new Edge(row[startIndex], otherRow[startIndex + 1]));
                }
                else
                {
                    result.Add(new Edge(otherRow[startIndex], row[startIndex + 1]));
                }
            }
            Logger.Debug("[CreateEdgesLikeSnakeFromTopToBottom] Returning result size: {0}", result.Count);
            return result;
        }

        internal ISet<Edge> CreateEdgesLikeSnakeFromBottomToTop(List<Node> row, List<Node> otherRow)
        {
            return CreateEdgesLikeSnakeFromTopToBottom(otherRow, row);
        }

        internal ISet<Edge> CreateEdgesBetweenNodesInOrder(List<Node> row)
        {
            if (row == null)
            {
                Logger.Warn("[JoinCellsInRow] Given null row");
                return new HashSet<Edge>();
            }
            var result = new HashSet<Edge>();
            for (int i = 1; i < row.Count; i++)
            {
                Edge e = new Edge(row[i-1], row[i]);
                result.Add(e);
            }
            return result;
        }

        internal ISet<Edge> CreateEdgesBetweenRowsAtSameIndex(List<Node> row, List<Node> otherRow)
        {
            if (row == null || otherRow == null)
            {
                Logger.Warn("[CreateEdgesConnectingRowsVertically] Given null row");
                return new HashSet<Edge>();
            }
            var result = new HashSet<Edge>();
            for (int i = 0; i < row.Count && i < otherRow.Count; i++)
            {
                Edge e = new Edge(row[i], otherRow[i]);
                result.Add(e);
            }
            return result;
        }
    }
}
