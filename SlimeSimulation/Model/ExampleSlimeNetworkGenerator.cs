using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace SlimeSimulation.Model {
    class ExampleSlimeNetworkGenerator {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private SlimeNetwork SimpleSlimeNetwork() {
            logger.Info("Returning SimpleSlimeNetwork");
            HashSet<Edge> edges = new HashSet<Edge>();
            HashSet<Node> nodes = new HashSet<Node>();
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

            HashSet<FoodSourceNode> foodSources = new HashSet<FoodSourceNode>() { source, sink };
            Loop loop = new Loop(nodes, edges);
            HashSet<Loop> loops = new HashSet<Loop>() { loop };
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

            HashSet<Edge> aloopEdges = new HashSet<Edge>() { srca, srcb, ac, bc };
            HashSet<Node> aloopNodes = new HashSet<Node>() { src, a, b, c };
            Loop aloop = new Loop(aloopNodes, aloopEdges);

            HashSet<Edge> endloopEdges = new HashSet<Edge>() { asink, ac, csink };
            HashSet<Node> endloopNodes = new HashSet<Node>() { a, c, sink };
            Loop endloop = new Loop(endloopNodes, endloopEdges);

            HashSet<Loop> loops = new HashSet<Loop>() { aloop, endloop };
            HashSet<FoodSourceNode> foodSources = new HashSet<FoodSourceNode>() { src, sink };
            HashSet<Node> allnodes = new HashSet<Node>() { src, a, b, c, sink };
            HashSet<Edge> alledges = new HashSet<Edge>() { srca, srcb, ac, bc, asink, csink };
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
