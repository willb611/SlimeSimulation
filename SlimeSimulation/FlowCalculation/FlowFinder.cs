using System;
using System.Collections.Generic;
using SlimeSimulation.Model;
using NLog;
using System.Linq;

namespace SlimeSimulation.FlowCalculation {
    public class FlowFinder {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public List<LoopWithDirectionOfFlow> GetLoopsWithDirectionForFlow(List<Loop> loops, Node source, Node sink, Graph graph) {
            List<LoopWithDirectionOfFlow> loopsWithDirections = new List<LoopWithDirectionOfFlow>();
            List<Node> visitOrderDoingBfsFromGraphSink = GetOrderVisitedDoingBfsFrom(graph, sink);
            SortedDictionary<int, List<Node>> distanceFromGraphSource = Dijkstras.GetShortestPathToNodes(source, graph);
            foreach (Loop loop in loops) {
                Node first = GetSourceForLoop(loop, distanceFromGraphSource, visitOrderDoingBfsFromGraphSink);
                Node last = GetSinkInLoop(first, visitOrderDoingBfsFromGraphSink, loop);
                loopsWithDirections.Add(loop.GetWithDirections(first, last));
            }
            return loopsWithDirections;
        }

        // Get node closest to graph source contained in loop. If multiple, get the one which is furthest from the graph sink, if draw choose undefined.
        private Node GetSourceForLoop(Loop loop, SortedDictionary<int, List<Node>> distanceFromGraphSource, List<Node> visitOrderFromGraphSink) {
            if (distanceFromGraphSource.Count < 1) {
                throw new ArgumentException("Expect at least 1 node in the list");
            }
            List<Node> candidates = new List<Node>();
            foreach (List<Node> nodesSomeDistanceAway in distanceFromGraphSource.Values) {
                candidates = new List<Node>(nodesSomeDistanceAway.Intersect(loop.Nodes));
                if (candidates.Count() >= 1) {
                    break;
                }
            }
            if (candidates.Count() == 1) {
                return candidates.First();
            } else if (candidates.Count() > 1) {
                List<Node> reverseVisitOrderFromSink = new List<Node>(visitOrderFromGraphSink);
                reverseVisitOrderFromSink.Reverse();
                return FirstNodeInSecondListWhichExistsInFirstList(candidates, reverseVisitOrderFromSink);
            } else {
                throw new ArgumentException("No nodes in the loop were found in the distanceFromGraphSource list");
            }
        }

        // Get nodes x distance away from source. If multiple, choose the one which appears first in visitOrderFromGraphSink
        private Node GetSinkInLoop(Node source, List<Node> visitOrderFromGraphSink, Loop loop) {
            Graph loopGraph = new Graph(loop.Edges, loop.Nodes);
            SortedDictionary<int, List<Node>> distanceFromSource = Dijkstras.GetShortestPathToNodes(source, loopGraph);
            var lastNodes = distanceFromSource.Last().Value;
            if (lastNodes.Count == 1) {
                return lastNodes.First();
            } else {
                return FirstNodeInSecondListWhichExistsInFirstList(lastNodes, visitOrderFromGraphSink);
            }
        }

        private T FirstNodeInSecondListWhichExistsInFirstList<T>(List<T> a, List<T> b) {
            foreach (T ele in b) {
                if (a.Contains(ele)) {
                    return ele;
                }
            }
            throw new ArgumentException("No element found in both lists");
        }

        private List<Node> GetOrderVisitedDoingBfsFrom(Graph graph, Node node) {
            List<Node> visitOrder = new List<Node>();
            Bfs.DoBfsAndStoreOrderVisitedInList(node, graph, ref visitOrder);
            return visitOrder;
        }
        
        public FlowOnEdges EstimateFlowForEdges(Graph graph, Node source, Node sink, int flow) {
            FlowOnEdges flowOnEdges = new FlowOnEdges(graph.Edges);
            Dictionary<Node, double> inputFlowAtNode = new Dictionary<Node, double>();
            inputFlowAtNode.Add(source, flow);
            HashSet<Node> visited = new HashSet<Node>();
            foreach (Node nodeToVisit in GetOrderVisitedDoingBfsFrom(graph, source)) {
                logger.Debug("Visiting node: " + nodeToVisit);
                var connectedEdges = graph.EdgesConnectedToNode(nodeToVisit);
                connectedEdges.RemoveAll(edge => visited.Contains(edge.A) || visited.Contains(edge.B));
                SplitFlowIntoNeighbours(nodeToVisit, connectedEdges, ref inputFlowAtNode, ref flowOnEdges);
                visited.Add(nodeToVisit);
            }
            return flowOnEdges;
        }

        internal void SplitFlowIntoNeighbours(Node nodeToVisit, List<Edge> connectedEdges, 
            ref Dictionary<Node, double> inputFlowAtNode, ref FlowOnEdges flowOnEdges) {
            double inputFlow = inputFlowAtNode[nodeToVisit];
            logger.Debug("For node being visited, splitting flow amount " + inputFlow);
            if (inputFlow == 0) {
                return;
            }
            double part = inputFlow / connectedEdges.Count;
            foreach (Edge edge in connectedEdges) {
                logger.Debug("[SplitFlowIntoNeighbours] Putting some flow into edge: " + edge);
                Node otherNode = edge.GetOtherNode(nodeToVisit);
                if (inputFlowAtNode.ContainsKey(otherNode)) {
                    inputFlowAtNode[otherNode] = inputFlowAtNode[otherNode] + part;
                } else {
                    inputFlowAtNode[otherNode] = part;
                }
                flowOnEdges.IncreaseFlowOnEdgeBy(edge, part);
            }
        }

        private class NodeWithInputFlow {
            private readonly Node node;
            private readonly double inputFlow;

            public NodeWithInputFlow(Node node, double inputFlow) {
                this.node = node;
                this.inputFlow = inputFlow;
            }

            public Node GetNode() {
                return node;
            }
            public double GetInputFlow() {
                return inputFlow;
            }
        }
    }
}
