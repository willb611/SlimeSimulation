using Gdk;
using NLog;

namespace SlimeSimulation.View
{
    public class Rgb
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static readonly Rgb Blue = new Rgb(0, 0, 255);
        public static readonly Rgb Red = new Rgb(255, 0, 0);
        public static readonly Rgb Black = new Rgb(0, 0, 0);
        public static readonly Rgb Yellow = new Rgb(255, 255, 0);
        public ushort R { get; private set; }
        public ushort G { get; private set; }
        public ushort B { get; private set; }

        public Rgb(int r, int g, int b)
        {
            R = (ushort)r;
            G = (ushort)g;
            B = (ushort)b;
        }

        public Color AsGdkColor()
        {
            Color color = new Color((byte)R, (byte)G, (byte)B);
            Logger.Debug("Got gdk color: {0}, {1}, {2}", color.Blue, color.Green, color.Red);
            return color;
        }
    }
}
