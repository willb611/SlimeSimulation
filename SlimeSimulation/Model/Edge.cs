using System;

namespace SlimeSimulation.Model
{
    public class Edge
    {
        private readonly double _connectivity;
        private readonly Node _a;
        private readonly Node _b;

        public Edge(Node a, Node b, double connectivity)
        {
            this._a = a;
            this._b = b;
            this._connectivity = connectivity;
        }

        public double Connectivity {
            get { return _connectivity; }
        }

        public double Resistance {
            get {
                if (_connectivity == 0)
                {
                    return double.MaxValue;
                }
                else
                {
                    return 1 / _connectivity;
                }
            }
        }

        public Node A {
            get { return _a; }
        }

        public Node B {
            get { return _b; }
        }

        public bool Contains(Node node)
        {
            return A == node || B == node;
        }

        public Node GetOtherNode(Node node)
        {
            if (node == A)
            {
                return B;
            }
            else if (node == B)
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

            if (_connectivity != other.Connectivity)
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
            return (_connectivity.GetHashCode() * 17 + A.GetHashCode()) * 17
                   + B.GetHashCode();
        }

        public override string ToString()
        {
            return "Edge{connectivity=" + _connectivity + ", a=" + _a + ", b=" + _b + "}";
        }
    }
}
