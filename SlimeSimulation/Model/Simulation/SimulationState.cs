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

    public SimulationState(SlimeNetwork network)
    {
      if (network == null)
      {
        throw new ArgumentNullException("Cannot create state with null network");
      }
      this.SlimeNetwork = network;
    }

    public SimulationState(SlimeNetwork network, FlowResult flowResult) : this(network)
    {
      this.FlowResult = flowResult;
    }

    public override string ToString()
    {
      return base.ToString() + ", hash: " + base.GetHashCode();
    }
  }
}