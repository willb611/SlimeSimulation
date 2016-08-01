using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.View {
    public class RGB {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static readonly RGB BLUE = new RGB(0, 0, 255);
        public static readonly RGB RED = new RGB(255, 0, 0);
        public static readonly RGB BLACK = new RGB(0, 0, 0);
        public ushort R {
            get; private set;
        }
        public ushort G {
            get; private set;
        }
        public ushort B {
            get; private set;
        }
        public RGB(int r, int g, int b) {
            R = (ushort) r;
            G = (ushort) g;
            B = (ushort) b;
        }

        public Gdk.Color AsGdkColor() {
            Gdk.Color color =  new Gdk.Color((byte)R, (byte) G, (byte) B);
            logger.Debug("Got gdk color: {0}, {1}, {2}", color.Blue, color.Green, color.Red);
            return color;
        }
    }
}
