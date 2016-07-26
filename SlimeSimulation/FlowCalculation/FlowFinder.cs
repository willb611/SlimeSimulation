using System;
using System.Collections.Generic;
using SlimeSimulation.Model;

namespace SlimeSimulation.FlowCalculation {
    public class FlowFinder {
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
            List<Node> visitOrder = GetOrderVisitedDoingBfsFrom(graph, source);
            for (int i = 0; i < visitOrder.Count; i++) {
                var nodeToVisit = visitOrder[i];
                var unvisitedNeighbours = graph.Neighbours(nodeToVisit);
                unvisitedNeighbours.RemoveAll(node => visitOrder.IndexOf(node) < i);
                SplitFlowIntoNeighbours(inputFlowAtNode[nodeToVisit], unvisitedNeighbours, ref inputFlowAtNode);
            }
            return flowOnEdges;
        }

        private void SplitFlowIntoNeighbours(double inputFlow, List<Node> neighbours, ref Dictionary<Node, double> inputFlowAtNode) {
            if (inputFlow == 0) {
                return;
            }
            double part = inputFlow / neighbours.Count;
            foreach (Node node in neighbours) {
                if (inputFlowAtNode.ContainsKey(node)) {
                    inputFlowAtNode[node] = inputFlowAtNode[node] + part;
                } else {
                    inputFlowAtNode[node] = part;
                }
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
