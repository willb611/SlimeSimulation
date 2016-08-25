namespace SlimeSimulation.Model
{
    public class FoodSourceNode : Node
    {
        public FoodSourceNode(int id, double x, double y) : base(id, x, y)
        {
        }

        public override bool IsFoodSource()
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FoodSourceNode);
        }

        public bool Equals(FoodSourceNode other)
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

            if (Id != other.Id)
            {
                return false;
            }
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return ((Id.GetHashCode() * 17 + X.GetHashCode()) * 17
                    + Y.GetHashCode()) * 17 + 3;
        }
    }
}
