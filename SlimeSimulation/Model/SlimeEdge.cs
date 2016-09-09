using Newtonsoft.Json;

namespace SlimeSimulation.Model
{
    public class SlimeEdge : Edge
    {
        public Edge Edge { get; }
        public double Connectivity { get; }

        [JsonConstructor]
        public SlimeEdge(Edge edge, double connectivity) : base(edge.A, edge.B)
        {
            Edge = edge;
            Connectivity = connectivity;
        }

        public SlimeEdge(Node a, Node b, double connectivity) : this(new Edge(a, b), connectivity)
        {
        }

        public double Resistance {
            get
            {
                if (IsDisconnected())
                {
                    return double.MaxValue;
                }
                return 1 / Connectivity;
            }
        }
        
        public new bool Equals(object obj)
        {
            return Equals(obj as SlimeEdge);
        }

        public bool Equals(SlimeEdge other)
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

            if (Connectivity != other.Connectivity)
            {
                return false;
            }
            return base.Equals(other);
        }

        public new int GetHashCode()
        {
            return Connectivity.GetHashCode() * 17 + base.GetHashCode();
        }

        public override string ToString()
        {
            return "SlimeEdge{connectivity=" + Connectivity + ", Edge=" + base.ToString() + "}";
        }

        public bool IsDisconnected()
        {
            return Connectivity == 0;
        }
    }
}
