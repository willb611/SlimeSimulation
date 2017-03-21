using System.Linq;
using Gtk;
using SlimeSimulation.Model.Generation;

namespace SlimeSimulation.View.WindowComponent.SimulationCreationComponent
{
    public class GeneratorShapeInputComponent : HBox
    {
        private readonly ComboBox _generatorInputComboBox;

        public GeneratorShapeInputComponent()
        {
            _generatorInputComboBox = new ComboBox(GraphGeneratorFactory.Descriptions);
            if (GraphGeneratorFactory.Descriptions != null && GraphGeneratorFactory.Descriptions.Any())
            {
                _generatorInputComboBox.Active = 0;
            }
            var generatorInputBoxDescriptionLabel = new Label("Shape of graph generated");

            Add(generatorInputBoxDescriptionLabel);
            Add(_generatorInputComboBox);
        }

        public int GetGeneratorTypeAsInt()
        {
            return GraphGeneratorFactory.GetValueForDescription(_generatorInputComboBox.ActiveText);
        }
    }
}
