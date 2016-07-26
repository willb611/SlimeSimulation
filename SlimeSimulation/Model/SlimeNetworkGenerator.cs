using System;
using SlimeSimulation.Model;
using System.Collections.Generic;
using NLog;

namespace SlimeSimulation.View {
    internal class SlimeNetworkGenerator {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        internal SlimeNetwork generate() {
            logger.Info("[generate] Starting.");
            List<Edge> edges = new List<Edge>();
            List<Node> nodes = new List<Node>();
            FoodSourceNode source = new FoodSourceNode(1, 50, 50);
            Node a = new Node(2, 50, 200);
            Edge srca = new Edge(source, a, 200);
            Node b = new Node(3, 200, 50);
            Edge srcb = new Edge(source, b, 200);
            FoodSourceNode sink = new FoodSourceNode(4, 200, 200);
            Edge asink = new Edge(a, sink, 2);
            Edge bsink = new Edge(b, sink, 2);
            edges.Add(srca);
            edges.Add(srcb);
            edges.Add(bsink);
            edges.Add(asink);
            nodes.Add(a);
            nodes.Add(b);
            nodes.Add(source);
            nodes.Add(sink);
            /*
             * b      -  sink
             * |        |
             * source -  a
             */

            List<FoodSourceNode> foodSources = new List<FoodSourceNode>(2) { source, sink };
            Loop loop = new Loop(nodes, edges);
            List<Loop> loops = new List<Loop>();
            SlimeNetwork slimeNetwork = new SlimeNetwork(nodes, foodSources, edges, loops);
            return slimeNetwork;
        }
    }
}
