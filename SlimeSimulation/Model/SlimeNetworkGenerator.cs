using SlimeSimulation.Model;
using System.Collections.Generic;
using NLog;

namespace SlimeSimulation.View {
    internal class SlimeNetworkGenerator {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        internal SlimeNetwork generate() {
            logger.Info("[generate] Starting.");
            return MultilpleLoopSlimeNetwork();
        }

        private SlimeNetwork SimpleSlimeNetwork() {
            logger.Info("Returning SimpleSlimeNetwork");
            List<Edge> edges = new List<Edge>();
            List<Node> nodes = new List<Node>();
            FoodSourceNode source = new FoodSourceNode(1, 50, 50);
            Node a = new Node(2, 50, 200);
            Edge srca = new Edge(source, a, 15);
            Node b = new Node(3, 200, 50);
            Edge srcb = new Edge(source, b, 15);
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
            List<Loop> loops = new List<Loop>() { loop };
            SlimeNetwork slimeNetwork = new SlimeNetwork(nodes, foodSources, edges, loops);
            return slimeNetwork;
        }

        public SlimeNetwork MultilpleLoopSlimeNetwork() {
            logger.Info("Returning MultilpleLoopSlimeNetwork");
            FoodSourceNode src = new FoodSourceNode(0, 15, 15);
            Node a = new Node(1, 100, 15);
            Node b = new Node(2, 15, 100);
            Node c = new Node(3, 100, 100);
            FoodSourceNode sink = new FoodSourceNode(4, 200, 100);

            int connectivity = 1;
            Edge srca = new Edge(src, a, connectivity);
            Edge srcb = new Edge(src, b, connectivity);
            Edge ac = new Edge(a, c, connectivity);
            Edge bc = new Edge(b, c, connectivity);
            Edge asink = new Edge(a, sink, connectivity);
            Edge csink = new Edge(c, sink, connectivity);

            List<Edge> aloopEdges = new List<Edge>() { srca, srcb, ac, bc };
            List<Node> aloopNodes = new List<Node>() { src, a, b, c };
            Loop aloop = new Loop(aloopNodes, aloopEdges);

            List<Edge> endloopEdges = new List<Edge>() { asink, ac, csink };
            List<Node> endloopNodes = new List<Node>() { a, c, sink };
            Loop endloop = new Loop(endloopNodes, endloopEdges);

            List<Loop> loops = new List<Loop>() { aloop, endloop };
            List<FoodSourceNode> foodSources = new List<FoodSourceNode>() { src, sink };
            List<Node> allnodes = new List<Node>() { src, a, b, c, sink };
            List<Edge> alledges = new List<Edge>() { srca, srcb, ac, bc, asink, csink };
            SlimeNetwork slimeNetwork = new SlimeNetwork(allnodes, foodSources, alledges, loops);
            /*
             * src ---- a ____
             * |        |     \
             * b ------ c ----- sink
             */

            return slimeNetwork;
        }
    }
}
