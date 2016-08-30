using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using SlimeSimulation.Configuration;

namespace SlimeSimulation.View.WindowComponent.SimulationStateDisplayComponent
{
    public class ShouldStepFromAllSourcesAtOnceControlComponent : CheckButton
    {
        private readonly SimulationControlInterfaceValues _simulationControlInterfaceValues;

        private const string DescriptionText =
            "Should each step average flow from each source in the network to a sink in the network?";

        public ShouldStepFromAllSourcesAtOnceControlComponent(SimulationControlInterfaceValues simulationControlInterfaceValues) : base(DescriptionText)
        {
            _simulationControlInterfaceValues = simulationControlInterfaceValues;
            Active = _simulationControlInterfaceValues.ShouldStepFromAllSourcesAtOnce;
            Toggled += ShouldStepFromAllSourcesAtOnceDisplayComponent_Toggled;
        }

        private void ShouldStepFromAllSourcesAtOnceDisplayComponent_Toggled(object sender, EventArgs e)
        {
            _simulationControlInterfaceValues.ShouldStepFromAllSourcesAtOnce =
                !_simulationControlInterfaceValues.ShouldStepFromAllSourcesAtOnce;
        }
    }
}
