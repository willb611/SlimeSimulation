using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            else if (Equals(node, B))
            {
                return A;
            }
            else
            {
                throw new ApplicationException("Node is not contained in this edge");
            }
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Edge);
        }

        public bool Equals(Edge other)
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

            else
            {
                return A.Equals(other.A) && B.Equals(other.B);
            }
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
