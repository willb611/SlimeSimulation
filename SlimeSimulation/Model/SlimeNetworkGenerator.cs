using SlimeSimulation.Model;
using System.Collections.Generic;
using System;
using NLog;

namespace SlimeSimulation.View {
    internal class SlimeNetworkGenerator {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        internal SlimeNetwork generate() {
            logger.Info("[generate] Starting.");
            return new ExampleSlimeNetworkGenerator().MultilpleLoopSlimeNetwork();
        }

        private SlimeNetwork GenerateLatticeNetwork(int columns, int rows) {
            int id = 1;
            bool columnOffset = true;
            List<Node> nodes = new List<Node>();
            List<Edge> edges = new List<Edge>();

            throw new NotImplementedException("TODO");
        }
    }
}
