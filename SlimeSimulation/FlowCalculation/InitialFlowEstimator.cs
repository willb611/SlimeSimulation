using System;
using System.Collections.Generic;
using SlimeSimulation.Model;
using NLog;
using System.Linq;

namespace SlimeSimulation.FlowCalculation {
    public class InitialFlowEstimator {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public FlowOnEdges EstimateFlowForEdges(Graph graph, Node source, Node sink, int flow) {
            FlowOnEdges flowOnEdges = new FlowOnEdges(graph.Edges);
            Dictionary<Node, double> inputFlowAtNode = new Dictionary<Node, double>();
            inputFlowAtNode.Add(source, flow);
            HashSet<Node> visited = new HashSet<Node>();
            foreach (Node nodeToVisit in Bfs.DoBfsAndGetOrderNodesWereVisitedIn(graph, source)) {
                logger.Debug("Visiting node: " + nodeToVisit);
                var connectedEdges = new List<Edge>(graph.EdgesConnectedToNode(nodeToVisit));
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
