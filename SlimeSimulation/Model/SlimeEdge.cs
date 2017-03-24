using System;
using Newtonsoft.Json;
using NLog;

namespace SlimeSimulation.Model
{
    public class SlimeEdge : Edge
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Edge Edge { get; }
        public double Connectivity { get; }
        public const double Tolerance = 0.000001;

        [JsonConstructor]
        public SlimeEdge(Edge edge, double connectivity) : base(edge.A, edge.B)
        {
            Edge = edge;
            Connectivity = connectivity;
        }

        public SlimeEdge(Node a, Node b, double connectivity) : this(new Edge(a, b), connectivity)
        {
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
            return Math.Abs(Connectivity) < Tolerance;
        }

        public double Length()
        {
            double dist = 0;
            var xdelta = Math.Abs(A.X - B.X);
            var ydelta = Math.Abs(A.Y - B.Y);
            if (xdelta < Tolerance || ydelta < Tolerance)
            {
                dist += xdelta + ydelta;
            }
            else
            {
                dist += Math.Sqrt(xdelta * xdelta + ydelta * ydelta);
            }
            return dist;
        }
    }
}
