using SlimeSimulation.FlowCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model {
    internal class Bfs {

        public static List<Node> DoBfsAndGetOrderNodesWereVisitedIn(Graph graph, Node node) {
            List<Node> visitOrder = new List<Node>();
            DoBfsAndStoreOrderVisitedInList(node, graph, ref visitOrder);
            return visitOrder;
        }

        private static void DoBfsAndStoreOrderVisitedInList(Node source, Graph graph, ref List<Node> orderVisited) {
            Queue<Node> nodesToVisit = new Queue<Node>();
            nodesToVisit.Enqueue(source);
            orderVisited.Add(source);
            while (nodesToVisit.Count > 0) {
                Node node = nodesToVisit.Dequeue();
                VisitNeighbours(ref nodesToVisit, ref orderVisited, graph.Neighbours(node));
            }
        }

        private static void VisitNeighbours(ref Queue<Node> nodesToVisit, ref List<Node> orderVisited, IEnumerable<Node> neighbours) {
            foreach (Node neighbour in neighbours) {
                if (!orderVisited.Contains(neighbour)) {
                    orderVisited.Add(neighbour);
                    nodesToVisit.Enqueue(neighbour);
                }
            }
        }
    }
}
