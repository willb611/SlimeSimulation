using System;

namespace SlimeSimulation.Model
{
    public class Edge
    {
        private readonly Node _a;
        private readonly Node _b;

        public Edge(Node a, Node b)
        {
            _a = a;
            _b = b;
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

            return A.Equals(other.A) && B.Equals(other.B);
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
