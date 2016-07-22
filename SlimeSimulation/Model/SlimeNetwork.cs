using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model {
    public class SlimeNetwork {
        List<Node> nodes;
        List<FoodSourceNode> foodSources;
        List<Edge> edges;
        List<Loop> loops;
    }
}
