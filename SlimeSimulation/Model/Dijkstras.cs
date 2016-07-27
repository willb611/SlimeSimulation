using SlimeSimulation.FlowCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model {
    public class Dijkstras {
        public static SortedDictionary<int, List<Node>> GetShortestPathToNodes(Node source, Graph graph) {
            var distanceToNodes = new Dictionary<Node, int>();
            foreach (Node node in graph.Nodes) {
                distanceToNodes[node] = int.MaxValue;
            }
            Queue<Step> steps = new Queue<Step>();
            steps.Enqueue(new Step(source, 0));
            while (steps.Any()) {
                Step step = steps.Dequeue();
                Node destination = step.Destination;
                if (step.DistanceAtEnd < distanceToNodes[destination]) {
                    distanceToNodes[destination] = step.DistanceAtEnd;
                    foreach (Node neighbour in graph.Neighbours(destination)) {
                        steps.Enqueue(new Step(neighbour, step.DistanceAtEnd + 1));
                    }
                }
            }
            return BuildDistanceMapping(distanceToNodes);
        }

        private static SortedDictionary<int, List<Node>> BuildDistanceMapping(Dictionary<Node, int> distanceToNodes) {
            SortedDictionary<int, List<Node>> distanceMapping = new SortedDictionary<int, List<Node>>();
            foreach (Node key in distanceToNodes.Keys) {
                int dist = distanceToNodes[key];
                if (distanceMapping.ContainsKey(dist)) {
                    distanceMapping[dist].Add(key);
                } else {
                    List<Node> nodesAtDistance = new List<Node>() { key };
                    distanceMapping[dist] = nodesAtDistance;
                }
            }
            return distanceMapping;
        }

        public class Step {
            private int distanceAtEnd;
            private Node destination;

            public Step(Node destination, int distanceTravelledAtDestination) {
                this.destination = destination;
                distanceAtEnd = distanceTravelledAtDestination;
            }

            public Node Destination {
                get {
                    return destination;
                }
            }

            public int DistanceAtEnd {
                get {
                    return distanceAtEnd;
                }
            }
        }
    }
}
