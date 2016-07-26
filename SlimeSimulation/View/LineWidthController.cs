using SlimeSimulation.Model;

namespace SlimeSimulation.View {
    public abstract class LineWidthController {
        public abstract double GetLineWidthForEdge(Edge edge);
        public abstract double GetMaximumLineWidth();
    }
}
