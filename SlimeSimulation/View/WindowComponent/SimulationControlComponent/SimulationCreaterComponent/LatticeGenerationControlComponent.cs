using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using SlimeSimulation.Configuration;
using SlimeSimulation.StdLibHelpers;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationCreaterComponent
{
    public class LatticeGenerationControlComponent : VBox
    {
        private TextView _latticeGeneratorRowSizeTextView;
        private TextView _latticeGeneratorProbabiltyOfNewFoodTextView;
        private TextView _latticeGeneratorMinimumFoodSourcesTextView;

        private List<string> errorComponents;
        private readonly Dictionary<TextView, Label> _textViewLabelMapping = new Dictionary<TextView, Label>();

        public LatticeGenerationControlComponent(LatticeGraphWithFoodSourcesGenerationConfig defaultConfig)
        {
            Add(RowSizeBox(defaultConfig));
            Add(ProbabilityNewNodeIsFoodBox(defaultConfig));
            Add(MinimumNumberFoodSourcesBox(defaultConfig));
        }

        private Widget MinimumNumberFoodSourcesBox(LatticeGraphWithFoodSourcesGenerationConfig defaultConfig)
        {
            Label description = new Label("Minimum number of food sources in the network");
            TextView textView = new TextView();
            textView.Buffer.Text = defaultConfig.MinimumFoodSources.ToString();
            _latticeGeneratorMinimumFoodSourcesTextView = textView;
            return MakeHbox(description, textView);
        }

        private Widget ProbabilityNewNodeIsFoodBox(LatticeGraphWithFoodSourcesGenerationConfig defaultConfig)
        {
            Label description = new Label("Probability new nodes in network are food sources");
            TextView textView = new TextView();
            textView.Buffer.Text = defaultConfig.ProbabilityNewNodeIsFoodSource.ToString();
            _latticeGeneratorProbabiltyOfNewFoodTextView = textView;
            return MakeHbox(description, textView);
        }

        private Widget RowSizeBox(LatticeGraphWithFoodSourcesGenerationConfig defaultConfig)
        {
            Label description = new Label("Number of rows in lattice to generate");
            TextView textView = new TextView();
            textView.Buffer.Text = defaultConfig.Size.ToString();
            _latticeGeneratorRowSizeTextView = textView;
            return MakeHbox(description, textView);
        }

        private Widget MakeHbox(Label description, TextView textView)
        {
            HBox hBox = new HBox();
            hBox.Add(description);
            hBox.Add(textView);
            _textViewLabelMapping[textView] = description;
            return hBox;
        }

        public LatticeGraphWithFoodSourcesGenerationConfig ReadGenerationConfig()
        {
            _textViewLabelMapping.Clear();
            double? probabilityNewNodeIsFood = _latticeGeneratorProbabiltyOfNewFoodTextView.ExtractDoubleFromView();
            AddToErrorsIfNull(probabilityNewNodeIsFood, _latticeGeneratorProbabiltyOfNewFoodTextView);
            int? minFoodSources = _latticeGeneratorMinimumFoodSourcesTextView.ExtractIntFromView();
            AddToErrorsIfNull(minFoodSources, _latticeGeneratorMinimumFoodSourcesTextView);
            int? rowSize = _latticeGeneratorRowSizeTextView.ExtractIntFromView();
            AddToErrorsIfNull(rowSize, _latticeGeneratorRowSizeTextView);

            if (probabilityNewNodeIsFood.HasValue && minFoodSources.HasValue && rowSize.HasValue)
            {
                return new LatticeGraphWithFoodSourcesGenerationConfig(rowSize.Value,
                    probabilityNewNodeIsFood.Value, minFoodSources.Value);
            } else
            {
                return null;
            }
        }

        private void AddToErrorsIfNull(double? nullableValue, TextView componentWhichReturnedValue)
        {
            if (nullableValue == null)
            {
                errorComponents.Add(_textViewLabelMapping[componentWhichReturnedValue].Text);
            }
        }

        internal IEnumerable<string> ErrorMessages()
        {
            return errorComponents;
        }
    }
}
