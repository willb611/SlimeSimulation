using System.Collections.Generic;
using System;

namespace SlimeSimulation.Model {
    public class Loop {
        ISet<Node> nodes;
        ISet<Edge> edges;

        public Loop(Loop toCopy) {
            this.nodes = new HashSet<Node>(toCopy.Nodes);
            this.edges = new HashSet<Edge>(toCopy.Edges);
        }

        public Loop(ISet<Node> nodes, ISet<Edge> edges) {
            this.nodes = nodes;
            this.edges = edges;
        }

        public bool Contains(Node node) {
            return nodes.Contains(node);
        }

        public ISet<Node> Nodes {
            get {
                return nodes;
            }
        }

        public ISet<Edge> Edges {
            get {
                return edges;
            }
        }

        public virtual ISet<Edge> Clockwise {
            get {
                throw new NotImplementedException("Base class loop doesn't have direction of flow");
            }
        }

        public virtual ISet<Edge> AntiClockwise {
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
                ISet<Edge> clockwise = PathTo(last, first, adjacentEdges[0]);
                ISet<Edge> antiClockwise = PathTo(last, first, adjacentEdges[1]);
                return new LoopWithDirectionOfFlow(this, clockwise, antiClockwise);
            }
        }

        internal ISet<Edge> PathTo(Node destination, Node start, Edge startingEdge) {
            if (null == destination) {
                throw new ArgumentNullException("Given null destination");
            } else if (null == start) {
                throw new ArgumentNullException("Given null start");
            } else if (destination.Equals(start)) {
                throw new ArgumentException("Destination equals start");
            } else if (!startingEdge.Contains(start)) {
                throw new ArgumentException("Starting edge doesnt contain the start");
            }
            ISet<Edge> result = new HashSet<Edge>();
            return PathToWithList(destination, start, startingEdge, ref result);
        }

        internal ISet<Edge> PathToWithList(Node destination, Node start, Edge startingEdge, ref ISet<Edge> edgesInPath) {
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
