using SlimeSimulation.FlowCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model.Simulation
{
    public class SimulationState
    {
        public SlimeNetwork SlimeNetwork { get; private set; }
        public FlowResult FlowResult { get; private set; }
        public bool HasFinishedExpanding { get; private set; }

        public SimulationState(SlimeNetwork network, bool hasFinishedExpanding)
        {
            if (network == null)
            {
                throw new ArgumentNullException(nameof(network));
            }
            this.SlimeNetwork = network;
            HasFinishedExpanding = hasFinishedExpanding;
        }

        public SimulationState(SlimeNetwork network, FlowResult flowResult) : this(network, true)
        {
            this.FlowResult = flowResult;
        }

        public override string ToString()
        {
            return base.ToString() + ", hash: " + base.GetHashCode();
        }
    }
}
