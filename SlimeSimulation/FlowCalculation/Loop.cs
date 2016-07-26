using System.Collections.Generic;
using System;

namespace SlimeSimulation.Model {
    public class Loop {
        List<Node> nodes;
        List<Edge> edges;

        public Loop(Loop toCopy) {
            this.nodes = toCopy.Nodes;
            this.edges = toCopy.Edges;
        }

        public Loop(List<Node> nodes, List<Edge> edges) {
            this.nodes = nodes;
            this.edges = edges;
        }

        public bool Contains(Node node) {
            return nodes.Contains(node);
        }

        public List<Node> Nodes {
            get {
                return nodes;
            }
        }

        public List<Edge> Edges {
            get {
                return edges;
            }
        }

        public virtual IEnumerable<Edge> Clockwise {
            get {
                throw new NotImplementedException("Base class loop doesn't have direction of flow");
            }
        }

        public virtual IEnumerable<Edge> AntiClockwise {
            get {
                throw new NotImplementedException("Base class loop doesn't have direction of flow");
            }
        }

        internal LoopWithDirectionOfFlow GetWithDirections(Node first, Node last) {
            if (first.Equals(last)) {
                throw new ArgumentException("First equals last");
            } else {
                List<Edge> adjacentEdges = first.GetEdgesAdjacent(edges);
                if (adjacentEdges.Count != 2) {
                    throw new ApplicationException("Should always be 2 adjacent edges from any node in a loop");
                }
                List<Edge> clockwise = PathTo(last, first, adjacentEdges[0], new List<Edge>());
                List<Edge> antiClockwise = PathTo(last, first, adjacentEdges[1], new List<Edge>());
                return new LoopWithDirectionOfFlow(this, clockwise, antiClockwise);
            }
        }

        internal List<Edge> PathTo(Node destination, Node start, Edge startingEdge, List<Edge> edgesInPath) {
            edgesInPath.Add(startingEdge);
            Node otherNodeInStartingEdge = startingEdge.GetOtherNode(start);
            if (otherNodeInStartingEdge == destination) {
                return edgesInPath;
            } else {
                List<Edge> adjacentEdges = otherNodeInStartingEdge.GetEdgesAdjacent(edges);
                if (adjacentEdges.Count != 2) {
                    throw new ApplicationException("Should always be 2 adjacent edges from any node in a loop");
                }
                if (adjacentEdges[0] == startingEdge) {
                    return PathTo(destination, otherNodeInStartingEdge, adjacentEdges[1], edgesInPath);
                } else if (adjacentEdges[1] == startingEdge) {
                    return PathTo(destination, otherNodeInStartingEdge, adjacentEdges[0], edgesInPath);
                } else {
                    throw new ApplicationException("Unexpected, adjacent edges did not contain the starting edge");
                }
            }
        }
    }
}
