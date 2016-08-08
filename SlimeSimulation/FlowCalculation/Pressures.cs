using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;
using NLog;

namespace SlimeSimulation.FlowCalculation.LinearEquations
{
    public class Pressures
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private double[] pressures;
        private List<Node> nodes;

        public Pressures(double[] pressures, List<Node> nodes)
        {
            this.pressures = pressures;
            this.nodes = nodes;
            logger.Debug("[constructor] Given pressures array size: {0}, so total number of nodes: {1}",
              pressures.Length, pressures.Length + 1);
        }

        public double PressureAt(int index)
        {
            double result = 0;
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("Index must be positive. Given: " + index);
            }
            else if (index < pressures.Length)
            {
                result = pressures[index];
            }
            else if (index == pressures.Length)
            {
                // Pressure at node n is set to 0
            }
            else
            {
                throw new ArgumentOutOfRangeException("Index must be <= " + pressures.Length
                                                      + ". Given: " + index);
            }
            logger.Trace("[PressureAt] For index {0} returning {1}", index, result);
            return result;
        }

        public double PressureAt(Node node)
        {
            if (nodes.Contains(node))
            {
                return PressureAt(nodes.IndexOf(node));
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
