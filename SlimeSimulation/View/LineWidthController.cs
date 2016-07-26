using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;

namespace SlimeSimulation.View {
    public abstract class LineWidthController {
        public abstract double GetLineWidthForEdge(Edge edge);
        public abstract double GetMaximumLineWidth();
    }
}
