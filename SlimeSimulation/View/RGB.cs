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
        public static readonly Rgb Orange = new Rgb(255, 130, 0);

        public double R { get; private set; }
        public double G { get; private set; }
        public double B { get; private set; }

        private readonly double _r;
        private readonly double _g;
        private readonly double _b;

        public Rgb(int r, int g, int b)
        {
            _r = r;
            _g = g;
            _b = b;
            R = Map(r);
            G = Map(g);
            B = Map(b);
        }

        private double Map(double valueUpTo255)
        {
            return valueUpTo255/255;
        }
        
        public override string ToString()
        {
            return "Rgb{_r=" + _r + ", _g=" + _g + ", _b=" + _b + "}";
        }
    }
}
