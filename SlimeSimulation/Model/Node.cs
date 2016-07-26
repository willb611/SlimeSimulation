using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model {
    public class Node : IEquatable<Node> {
        private readonly int id;
        private readonly double x, y;
        
        public Node(int id, double x, double y) {
            this.id = id;
            this.x = x;
            this.y = y;
        }
        
        public int Id {
            get {
                return id;
            }
        }

        public double X {
            get {
                return x;
            }
        }

        public double Y {
            get {
                return y;
            }
        }

        public virtual bool IsFoodSource() {
            return false;
        }

        public override bool Equals(object obj) {
            return this.Equals(obj as Node);
        }

        public bool Equals(Node other) {
            if (Object.ReferenceEquals(other, null)) {
                return false;
            } else if (Object.ReferenceEquals(other, this)) {
                return true;
            }

            if (this.GetType() != other.GetType()) {
                return false;
            }

            if (id != other.Id) {
                return false;
            } else {
                return X == other.X && Y == other.Y;
            }
        }

        public override int GetHashCode() {
            return (id.GetHashCode() * 17 + x.GetHashCode()) * 17
                    + y.GetHashCode();
        }

        internal List<Edge> GetEdgesAdjacent(List<Edge> edges) {
            List<Edge> result = new List<Edge>();
            foreach (Edge edge in edges) {
                if (this == edge.A || this == edge.B) {
                    result.Add(edge);
                }
            }
            return result;
        }

        public override string ToString() {
            return this.GetType() + "{id=" + id + ", x=" + x + ", y=" + y + "}";
        }
    }
}
