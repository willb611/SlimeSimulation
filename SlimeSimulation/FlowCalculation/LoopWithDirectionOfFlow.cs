using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;
using SlimeSimulation.FlowCalculation;

namespace SlimeSimulation.Model {
    public class LoopWithDirectionOfFlow : Loop {
        private readonly IEnumerable<Edge> clockwise;
        private readonly IEnumerable<Edge> antiClockwise;

        public LoopWithDirectionOfFlow(Loop other, IEnumerable<Edge> clockwise,
            IEnumerable<Edge> antiClockwise) : base(other) {
            this.clockwise = clockwise;
            this.antiClockwise = antiClockwise;
        }

        public override IEnumerable<Edge> AntiClockwise {
            get {
                return antiClockwise;
            }
        }

        public override IEnumerable<Edge> Clockwise {
            get {
                return clockwise;
            }
        }
    }
}
