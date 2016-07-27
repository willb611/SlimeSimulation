using System.Collections.Generic;
using System.Linq;
using NLog;

namespace SlimeSimulation.Model {
    public class LoopWithDirectionOfFlow : Loop {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ISet<Edge> clockwise;
        private readonly ISet<Edge> antiClockwise;

        public LoopWithDirectionOfFlow(Loop other, ISet<Edge> clockwise, ISet<Edge> antiClockwise) : base(other) {
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

        public override ISet<Edge> AntiClockwise {
            get {
                return antiClockwise;
            }
        }

        public override ISet<Edge> Clockwise {
            get {
                return clockwise;
            }
        }

        public override string ToString() {
            return base.ToString() + ",LoopWithDirectionOfFlow{clockwise=" + LogHelper.CollectionToString(Clockwise)
                + ",anticlockwise=" + LogHelper.CollectionToString(AntiClockwise) + "}";
        }
    }
}
