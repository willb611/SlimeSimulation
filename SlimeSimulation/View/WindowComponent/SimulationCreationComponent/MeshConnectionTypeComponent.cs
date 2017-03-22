using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using SlimeSimulation.Configuration;
using SlimeSimulation.Model.Generation;

namespace SlimeSimulation.View.WindowComponent.SimulationCreationComponent
{
    public class MeshConnectionTypeComponent : HBox
    {
        private readonly ComboBox _meshConnectionTypeComboBox;

        public MeshConnectionTypeComponent() : this(EdgeConnectionShape.DefaultEdgeConnectionType)
        {
        }

        public MeshConnectionTypeComponent(int edgeConnectionType)
        {
            string[] descriptions = EdgeConnectionShape.DescriptionsForEdgeConnectionTypes;
            _meshConnectionTypeComboBox = new ComboBox(descriptions);
            if (descriptions != null && descriptions.Any())
            {
                _meshConnectionTypeComboBox.Active = EdgeConnectionShape.IndexInDescriptionArrayForValue(edgeConnectionType);
            }
            var meshConnectionTypeDescriptionLabel = new Label("Resulting shape from node connections");

            Add(meshConnectionTypeDescriptionLabel);
            Add(_meshConnectionTypeComboBox);
        }

        public int GetEdgeConnectionTypeAsInt()
        {
            return EdgeConnectionShape.GetValueForDescription(_meshConnectionTypeComboBox.ActiveText);
        }
    }
}
