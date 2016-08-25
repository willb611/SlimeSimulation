using NLog;
using System;
using System.Collections.Generic;

namespace SlimeSimulation.Model
{
    public class Node : IEquatable<Node>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly int _id;
        private readonly double _x, _y;

        public Node(int id, double x, double y)
        {
            _id = id;
            _x = x;
            _y = y;
        }

        public int Id {
            get { return _id; }
        }

        public double X {
            get { return _x; }
        }

        public double Y {
            get { return _y; }
        }

        public virtual bool IsFoodSource()
        {
            return false;
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
            else if (ReferenceEquals(other, this))
            {
                return true;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }

            if (_id != other.Id)
            {
                return false;
            }
            else
            {
                return X == other.X && Y == other.Y;
            }
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
                if (this == edge.A || this == edge.B)
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
