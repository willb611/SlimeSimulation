using System.Collections.Generic;
using System.Linq;
using NLog;

namespace SlimeSimulation.Model {
    public class LoopWithDirectionOfFlow : Loop {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly List<Edge> clockwise;
        private readonly List<Edge> antiClockwise;

        public LoopWithDirectionOfFlow(Loop other, List<Edge> clockwise, List<Edge> antiClockwise) : base(other) {
            this.clockwise = clockwise;
            this.antiClockwise = antiClockwise;
            if (clockwise.Count == 0) {
                logger.Warn("Clockwise loop has 0 edges in it");
            }
            if (antiClockwise.Count == 0) {
                logger.Warn("antiClockwise loop has 0 edges in it");
            }
            logger.Debug("Finished construction of " + this);
        }

        public override List<Edge> AntiClockwise {
            get {
                return antiClockwise;
            }
        }

        public override List<Edge> Clockwise {
            get {
                return clockwise;
            }
        }

        public override string ToString() {
            return base.ToString() + ",LoopWithDirectionOfFlow{clockwise=" + LogHelper.ListToString(Clockwise)
                + ",anticlockwise=" + LogHelper.ListToString(AntiClockwise) + "}";
        }
    }
}
