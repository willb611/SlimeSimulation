using System;

namespace SlimeSimulation.Model {
    public class Edge {
        private readonly double connectivity;
        private Node a, b;

        public Edge(Node a, Node b, double connectivity) {
            this.a = a;
            this.b = b;
            this.connectivity = connectivity;
        }
        
        public double Connectivity {
            get {
                return connectivity;
            }
        }

        public double Resistance {
            get {
                if (connectivity == 0) {
                    return double.MaxValue;
                } else {
                    return 1 / connectivity;
                }
            }
        }

        public Node A {
            get {
                return a;
            }
        }

        public Node B {
            get {
                return b;
            }
        }

        public bool Contains(Node node) {
            return A == node || B == node;
        }

        public Node GetOtherNode(Node node) {
            if (node == A) {
                return B;
            } else if (node == B) {
                return A;
            } else {
                throw new ApplicationException("Node is not contained in this edge");
            }
        }

        public override bool Equals(object obj) {
            return this.Equals(obj as Edge);
        }

        public bool Equals(Edge other) {
            if (Object.ReferenceEquals(other, null)) {
                return false;
            } else if (Object.ReferenceEquals(other, this)) {
                return true;
            }

            if (this.GetType() != other.GetType()) {
                return false;
            }

            if (connectivity != other.Connectivity) {
                return false;
            } else {
                return A == other.A && B == other.B;
            }
        }

        public override int GetHashCode() {
            return (connectivity.GetHashCode() * 17 + A.GetHashCode()) * 17
                    + B.GetHashCode();
        }
    }
}
