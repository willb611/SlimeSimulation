using System.Collections.Generic;
using System;

namespace SlimeSimulation.Model {
    public class Loop {
        List<Node> nodes;
        List<Edge> edges;

        public Loop(Loop toCopy) {
            this.nodes = new List<Node>(toCopy.Nodes);
            this.edges = new List<Edge>(toCopy.Edges);
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

        public virtual List<Edge> Clockwise {
            get {
                throw new NotImplementedException("Base class loop doesn't have direction of flow");
            }
        }

        public virtual List<Edge> AntiClockwise {
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
                List<Edge> clockwise = PathTo(last, first, adjacentEdges[0]);
                List<Edge> antiClockwise = PathTo(last, first, adjacentEdges[1]);
                return new LoopWithDirectionOfFlow(this, clockwise, antiClockwise);
            }
        }

        internal List<Edge> PathTo(Node destination, Node start, Edge startingEdge) {
            if (null == destination) {
                throw new ArgumentNullException("Given null destination");
            } else if (null == start) {
                throw new ArgumentNullException("Given null start");
            } else if (destination.Equals(start)) {
                throw new ArgumentException("Destination equals start");
            } else if (!startingEdge.Contains(start)) {
                throw new ArgumentException("Starting edge doesnt contain the start");
            }
            List<Edge> result = new List<Edge>();
            return PathToWithList(destination, start, startingEdge, ref result);
        }

        internal List<Edge> PathToWithList(Node destination, Node start, Edge startingEdge, ref List<Edge> edgesInPath) {
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
                    return PathToWithList(destination, otherNodeInStartingEdge, adjacentEdges[1], ref edgesInPath);
                } else if (adjacentEdges[1] == startingEdge) {
                    return PathToWithList(destination, otherNodeInStartingEdge, adjacentEdges[0], ref edgesInPath);
                } else {
                    throw new ApplicationException("Unexpected, adjacent edges did not contain the starting edge");
                }
            }
        }

        public override bool Equals(object obj) {
            return this.Equals(obj as Loop);
        }
        public bool Equals(Loop other) {
            if (Object.ReferenceEquals(other, null)) {
                return false;
            } else if (Object.ReferenceEquals(other, this)) {
                return true;
            }

            if (this.GetType() != other.GetType()) {
                return false;
            }

            return nodes.Equals(other.Nodes) && edges.Equals(other.Edges);
        }

        public override int GetHashCode() {
            return edges.GetHashCode() * 17 + nodes.GetHashCode();
        }

        public override string ToString() {
            return "Loop{nodes.Count=" + nodes.Count + ", edges.Count=" + edges.Count + "}";
        }
    }
}
