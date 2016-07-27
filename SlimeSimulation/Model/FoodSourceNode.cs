using System;

namespace SlimeSimulation.Model {
    public class FoodSourceNode : Node {
        public FoodSourceNode(int id, double x, double y) : base(id, x, y) {
        }

        public override bool IsFoodSource() {
            return true;
        }

        public override bool Equals(object obj) {
            return this.Equals(obj as FoodSourceNode);
        }
        public bool Equals(FoodSourceNode other) {
            if (Object.ReferenceEquals(other, null)) {
                return false;
            } else if (Object.ReferenceEquals(other, this)) {
                return true;
            }

            if (this.GetType() != other.GetType()) {
                return false;
            }

            if (Id != other.Id) {
                return false;
            } else {
                return X == other.X && Y == other.Y;
            }
        }

        public override int GetHashCode() {
            return ((Id.GetHashCode() * 17 + X.GetHashCode()) * 17
                    + Y.GetHashCode()) * 17 + 3;
        }
    }
}
