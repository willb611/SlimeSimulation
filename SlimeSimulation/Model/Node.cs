using System;
using System.Collections.Generic;
using NLog;

namespace SlimeSimulation.Model
{
    public class Node : IEquatable<Node>, IComparable<Node>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly int _id;
        private readonly double _x, _y;
        public virtual bool IsFoodSource => false;

        public Node(int id, double x, double y)
        {
            _id = id;
            _x = x;
            _y = y;
        }

        public int Id => _id;
        public double X => _x;
        public double Y => _y;

        public int CompareTo(Node other)
        {
            if (Id.Equals(other.Id))
            {
                if (X.Equals(other.X))
                {
                    return Y.CompareTo(other.Y);
                }
                else
                {
                    return X.CompareTo(other.X);
                }
            }
            else
            {
                return Id.CompareTo(other.Id);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Node);
        }

        public bool Equals(Node other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }

            return CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            return (_id.GetHashCode() * 17 + _x.GetHashCode()) * 17
                   + _y.GetHashCode();
        }

        internal List<Edge> GetEdgesAdjacent(ISet<Edge> edges)
        {
            ISet<Edge> result = new HashSet<Edge>();
            foreach (Edge edge in edges)
            {
                if (edge.Contains(this))
                {
                    result.Add(edge);
                }
            }
            return new List<Edge>(result);
        }

        public override string ToString()
        {
            return GetType() + "{id=" + _id + ", x=" + _x + ", y=" + _y + "}";
        }

        internal void ReplaceWithGivenNodeInEdges(Node replacement, HashSet<Edge> edges)
        {
            if (Logger.IsTraceEnabled)
            {
                Logger.Trace("[ReplaceWithGivenNodeInEdges] Replacing {0}, with: {1}",
                    this, replacement);
            }
            foreach (Edge edge in GetEdgesAdjacent(edges))
            {
                Node otherNode = edge.GetOtherNode(this);
                Edge replacementSlimeEdge = new Edge(replacement, otherNode);
                edges.Remove(edge);
                edges.Add(replacementSlimeEdge);
            }
            if (Logger.IsTraceEnabled)
            {
                Logger.Trace("[EnsureFoodSources] Finished. resulting edges: " + LogHelper.CollectionToString(edges));
            }
        }
    }
}
