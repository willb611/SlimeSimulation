using System;
using System.Collections.Generic;
using SlimeSimulation.Model;
using NLog;

namespace SlimeSimulation.FlowCalculation {
    public class FlowFinder {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public List<LoopWithDirectionOfFlow> GetLoopsWithDirectionForFlow(List<Loop> loops, Node source, Node sink, Graph graph) {
            List<LoopWithDirectionOfFlow> loopsWithDirections = new List<LoopWithDirectionOfFlow>();
            List<Node> sourceVisitOrder = GetOrderVisitedDoingBfsFrom(graph, source);
            List<Node> sinkVisitOrder = GetOrderVisitedDoingBfsFrom(graph, sink);
            foreach (Loop loop in loops) {
                Node first = GetFirstNodeInLoopFromEnumerable(loop, sourceVisitOrder);
                Node last = GetFirstNodeInLoopFromEnumerable(loop, sinkVisitOrder);
                loopsWithDirections.Add(loop.GetWithDirections(first, last));
            }
            return loopsWithDirections;
        }

        private List<Node> GetOrderVisitedDoingBfsFrom(Graph graph, Node node) {
            List<Node> visitOrder = new List<Node>();
            DoBfs(node, graph, ref visitOrder);
            return visitOrder;
        }

        private Node GetFirstNodeInLoopFromEnumerable(Loop loop, IEnumerable<Node> enumerable) {
            foreach (Node node in enumerable) {
                if (loop.Contains(node)) {
                    return node;
                }
            }
            throw new ApplicationException("Unable to find node in enumerable which matches loop: " + loop);
        }

        private void DoBfs(Node source, Graph graph, ref List<Node> orderVisited) {
            Queue<Node> nodesToVisit = new Queue<Node>();
            nodesToVisit.Enqueue(source);
            orderVisited.Add(source);
            while (nodesToVisit.Count > 0) {
                Node node = nodesToVisit.Dequeue();
                VisitNeighbours(ref nodesToVisit, ref orderVisited, graph.Neighbours(node));
            }
        }

        private void VisitNeighbours(ref Queue<Node> nodesToVisit, ref List<Node> orderVisited, IEnumerable<Node> neighbours) {
            foreach (Node neighbour in neighbours) {
                if (!orderVisited.Contains(neighbour)) {
                    orderVisited.Add(neighbour);
                    nodesToVisit.Enqueue(neighbour);
                }
            }
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
