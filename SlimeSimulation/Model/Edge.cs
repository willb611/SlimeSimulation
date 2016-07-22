namespace SlimeSimulation.Model {
    internal class Edge {
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
    }
}
