using System;

namespace SlimeSimulation.Model
{
    public class SlimeEdge
    {
        private readonly double _connectivity;
        private readonly Edge _edge;

        public SlimeEdge(Node a, Node b, double connectivity)
        {
            _edge = new Edge(a, b);
            this._connectivity = connectivity;
        }

        public double Connectivity => _connectivity;
        public Node A => _edge.A;
        public Node B => _edge.B;

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
            return this.Equals(obj as SlimeEdge);
        }

        public bool Equals(SlimeEdge other)
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
                return _edge.Equals(other._edge);
            }
        }

        public override int GetHashCode()
        {
            return _connectivity.GetHashCode() * 17 + _edge.GetHashCode();
        }

        public override string ToString()
        {
            return "SlimeEdge{connectivity=" + _connectivity + ", Edge=" + _edge + "}";
        }
    }
}
