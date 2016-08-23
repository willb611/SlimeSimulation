using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;
using NLog;

namespace SlimeSimulation.FlowCalculation
{
    public class Pressures
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly double[] _pressures;
        private readonly List<Node> _nodes;

        public Pressures(double[] pressures, List<Node> nodes)
        {
            this._pressures = pressures;
            this._nodes = nodes;
            Logger.Debug("[constructor] Given pressures array size: {0}, so total number of nodes: {1}",
              pressures.Length, pressures.Length + 1);
        }

        public double PressureAt(int index)
        {
            double result = 0;
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("Index must be positive. Given: " + index);
            }
            else if (index < _pressures.Length)
            {
                result = _pressures[index];
            }
            else if (index == _pressures.Length)
            {
                // Pressure at node n is set to 0
            }
            else
            {
                throw new ArgumentOutOfRangeException("Index must be <= " + _pressures.Length
                                                      + ". Given: " + index);
            }
            Logger.Trace("[PressureAt] For index {0} returning {1}", index, result);
            return result;
        }

        public double PressureAt(Node node)
        {
            if (_nodes.Contains(node))
            {
                return PressureAt(_nodes.IndexOf(node));
            }
            else
            {
                throw new ArgumentException("Pressure unkown for node: " + node);
            }
        }

        public double this[int i] {
            get { return PressureAt(i); }
        }
    }
}
