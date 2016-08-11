using NLog;
using System;
using System.Collections.Generic;

namespace SlimeSimulation.Model
{
    public class Node : IEquatable<Node>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly int id;
        private readonly double x, y;

        public Node(int id, double x, double y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }

        public int Id {
            get { return id; }
        }

        public double X {
            get { return x; }
        }

        public double Y {
            get { return y; }
        }

        public virtual bool IsFoodSource()
        {
            return false;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Node);
        }

        public bool Equals(Node other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }
            else if (Object.ReferenceEquals(other, this))
            {
                return true;
            }

            if (this.GetType() != other.GetType())
            {
                return false;
            }

            if (id != other.Id)
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
            return (id.GetHashCode() * 17 + x.GetHashCode()) * 17
                   + y.GetHashCode();
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
            return this.GetType() + "{id=" + id + ", x=" + x + ", y=" + y + "}";
        }

        internal void ReplaceWithGivenNodeInEdges(Node replacement, HashSet<Edge> edges)
        {
            if (logger.IsTraceEnabled)
            {
                logger.Trace("[ReplaceWithGivenNodeInEdges] Replacing {0}, with: {1}",
                    this, replacement);
            }
            foreach (Edge edge in GetEdgesAdjacent(edges))
            {
                Node otherNode = edge.GetOtherNode(this);
                Edge replacementEdge = new Edge(replacement, otherNode, edge.Connectivity);
                edges.Remove(edge);
                edges.Add(replacementEdge);
            }
            if (logger.IsTraceEnabled)
            {
                logger.Trace("[EnsureFoodSources] Finished. resulting edges: " + LogHelper.CollectionToString(edges));
            }
        }
    }
}
