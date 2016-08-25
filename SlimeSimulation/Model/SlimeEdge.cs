using System;

namespace SlimeSimulation.Model
{
    public class SlimeEdge : Edge
    {
        private readonly double _connectivity;

        public SlimeEdge(Edge edge, double connectivity) : base(edge.A, edge.B)
        {
            _connectivity = connectivity;
        }

        public SlimeEdge(Node a, Node b, double connectivity) : this(new Edge(a, b), connectivity)
        {
        }

        public double Connectivity => _connectivity;

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
        
        public override bool Equals(object obj)
        {
            return Equals(obj as SlimeEdge);
        }

        public bool Equals(SlimeEdge other)
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

            if (_connectivity != other.Connectivity)
            {
                return false;
            }
            else
            {
                return base.Equals(other);
            }
        }

        public override int GetHashCode()
        {
            return _connectivity.GetHashCode() * 17 + base.GetHashCode();
        }

        public override string ToString()
        {
            return "SlimeEdge{connectivity=" + _connectivity + ", Edge=" + base.ToString() + "}";
        }
    }
}
