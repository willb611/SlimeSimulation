namespace SlimeSimulation.Model {
    internal class FoodSourceNode : Node {
        public FoodSourceNode(int id, double x, double y) : base(id, x, y) {
        }

        public override bool IsFoodSource() {
            return true;
        }
    }
}
