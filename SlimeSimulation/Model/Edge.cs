using System;

namespace SlimeSimulation.Model
{
    public class Edge : IEquatable<Edge>, IComparable<Edge>
    {
        private readonly Node _a;
        private readonly Node _b;

        public Edge(Node a, Node b)
        {
            if (a.CompareTo(b) <= 0)
            {
                _a = a;
                _b = b;
            }
            else
            {
                _a = b;
                _b = a;
            }
        }

        public Node A => _a;
        public Node B => _b;

        public bool Contains(Node node)
        {
            return A.Equals(node) || B.Equals(node);
        }

        public Node GetOtherNode(Node node)
        {
            if (Equals(node, A))
            {
                return B;
            }
            if (Equals(node, B))
            {
                return A;
            }
            throw new ApplicationException("Node is not contained in this edge");
        }

        public int CompareTo(Edge other)
        {
            if (A.Equals(other.A))
            {
                return B.CompareTo(other.B);
            } else
            {
                return A.CompareTo(other.A);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Edge);
        }

        public bool Equals(Edge other)
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
            return A.GetHashCode() * 17 + B.GetHashCode();
        }

        public override string ToString()
        {
            return "Edge{a=" + _a + ", b=" + _b + "}";
        }
    }
}
