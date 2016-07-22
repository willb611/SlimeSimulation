using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model {
    public class Node {
        private readonly int id, x, y;
        
        public Node(int id, double x, double y) {
            this.id = id;
        }

        public int getId() {
            return id;
        }

        public int getX() {
            return x;
        }

        public int getY() {
            return y;
        }

        public virtual bool IsFoodSource() {
            return false;
        }
    }
}
