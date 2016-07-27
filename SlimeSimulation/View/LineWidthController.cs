using SlimeSimulation.Model;

namespace SlimeSimulation.View {
    public abstract class LineWeightController {
        public abstract double GetLineWeightForEdge(Edge edge);
        public abstract double GetMaximumLineWeight();
    }
}
