using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.FlowCalculation {
    public class LoopDirectionFinder {
        public ISet<LoopWithDirectionOfFlow> GetLoopsWithDirectionForFlow(ISet<Loop> loops, Node source, Node sink, Graph graph) {
            ISet<LoopWithDirectionOfFlow> loopsWithDirections = new HashSet<LoopWithDirectionOfFlow>();
            List<Node> visitOrderDoingBfsFromGraphSink = Bfs.DoBfsAndGetOrderNodesWereVisitedIn(graph, sink);
            SortedDictionary<int, ISet<Node>> distanceFromGraphSource = Dijkstras.GetShortestPathToNodes(source, graph);
            foreach (Loop loop in loops) {
                Node first = GetSourceForLoop(loop, distanceFromGraphSource, visitOrderDoingBfsFromGraphSink);
                Node last = GetSinkInLoop(first, visitOrderDoingBfsFromGraphSink, loop);
                loopsWithDirections.Add(loop.GetWithDirections(first, last));
            }
            return loopsWithDirections;
        }

        // Get node closest to graph source contained in loop. If multiple, get the one which is furthest from the graph sink, if draw choose undefined.
        private Node GetSourceForLoop(Loop loop, SortedDictionary<int, ISet<Node>> distanceFromGraphSource, List<Node> visitOrderFromGraphSink) {
            if (distanceFromGraphSource.Count < 1) {
                throw new ArgumentException("Expect at least 1 node in the list");
            }
            ISet<Node> candidates = new HashSet<Node>();
            foreach (ISet<Node> nodesSomeDistanceAway in distanceFromGraphSource.Values) {
                candidates = new HashSet<Node>(nodesSomeDistanceAway.Intersect(loop.Nodes));
                if (candidates.Count() >= 1) {
                    break;
                }
            }
            if (candidates.Count() == 1) {
                return candidates.First();
            } else if (candidates.Count() > 1) {
                List<Node> reverseVisitOrderFromSink = new List<Node>(visitOrderFromGraphSink);
                reverseVisitOrderFromSink.Reverse();
                return FirstNodeInSecondListWhichExistsInFirst(candidates, reverseVisitOrderFromSink);
            } else {
                throw new ArgumentException("No nodes in the loop were found in the distanceFromGraphSource list");
            }
        }

        // Get nodes x distance away from source. If multiple, choose the one which appears first in visitOrderFromGraphSink
        private Node GetSinkInLoop(Node source, List<Node> visitOrderFromGraphSink, Loop loop) {
            Graph loopGraph = new Graph(loop.Edges, loop.Nodes);
            SortedDictionary<int, ISet<Node>> distanceFromSource = Dijkstras.GetShortestPathToNodes(source, loopGraph);
            var lastNodes = distanceFromSource.Last().Value;
            if (lastNodes.Count == 1) {
                return lastNodes.First();
            } else {
                return FirstNodeInSecondListWhichExistsInFirst(lastNodes, visitOrderFromGraphSink);
            }
        }

        private T FirstNodeInSecondListWhichExistsInFirst<T>(ICollection<T> a, List<T> b) {
            foreach (T ele in b) {
                if (a.Contains(ele)) {
                    return ele;
                }
            }
            throw new ArgumentException("No element found in both lists");
        }
    }
}
