namespace SlimeSimulation.Model
{
    public class Route
    {
        private readonly Node _source, _sink;
        public Node Source => _source;
        public Node Sink => _sink;

        public Route(Node source, Node sink)
        {
            _source = source;
            _sink = sink;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Route);
        }

        public bool Equals(Route other)
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

            return Source.Equals(other.Source) && Sink.Equals(other.Sink);
        }

        public override int GetHashCode()
        {
            return _source.GetHashCode()*17 + _sink.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString() + "Source={" + Source + "},Sink={" + Sink + "}";
        }
    }
}
