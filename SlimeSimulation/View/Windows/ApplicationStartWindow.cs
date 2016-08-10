using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using SlimeSimulation.Controller;
using SlimeSimulation.View.Factories;
using NLog;
using SlimeSimulation.Configuration;

namespace SlimeSimulation.View.Windows
{
    class ApplicationStartWindow : WindowTemplate
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ApplicationStartController controller;
        private Button beginSimulationButton;

        private TextView latticeGenerator_RowSizeTextView;
        private TextView latticeGenerator_ProbabiltyOfNewFoodTextView;
        private TextView latticeGenerator_MinimumFoodSourcesTextView;

        private TextView feedbackParamTextView;
        private TextView flowAmountTextView;

        private Label errorLabel;

        private SimulationConfiguration DEFAULT_CONFIG = new SimulationConfiguration();
        private Dictionary<TextView, Label> textViewLabelMapping = new Dictionary<TextView, Label>();

        private List<String> errors = new List<string>();


        public ApplicationStartWindow(string windowTitle, ApplicationStartController controller) : base(windowTitle, controller)
        {
            this.controller = controller;
        }

        protected override void AddToWindow(Window window)
        {
            VBox container = new VBox();
            container.Add(FlowBox());
            container.Add(FeedbackParameterBox());
            container.Add(MatrixGenerationProperties());
            container.Add(BeginSimulationButton());
            container.Add(ErrorLabel());
            window.Add(container);
            beginSimulationButton.Clicked += BeginSimulationButton_Clicked;
        }
        
        private void BeginSimulationButton_Clicked(object obj, EventArgs args)
        {
            logger.Debug("[BeginSimulationButton_Clicked] Entered");
            var config = GetConfigFromViews();
            if (config == null)
            {
                logger.Info("[BeginSimulationButton_Clicked] Not starting simulation due to invalid parameters");
            }
            else
            {
                controller.StartSimulation(config);
            }
        }

        private Widget ErrorLabel()
        {
            if (errorLabel == null)
            {
                errorLabel = new Label();
            }
            HBox box = new HBox();
            box.Add(errorLabel);
            return box;
        }


        private SimulationConfiguration GetConfigFromViews()
        {
            errors = new List<string>();
            double? feedbackParam = ExtractDoubleFromView(feedbackParamTextView);
            int? flowAmount = ExtractIntFromView(flowAmountTextView);
            double? probabilityNewNodeIsFood = ExtractDoubleFromView(latticeGenerator_ProbabiltyOfNewFoodTextView);
            int? minFoodSources = ExtractIntFromView(latticeGenerator_MinimumFoodSourcesTextView);
            int? rowSize = ExtractIntFromView(latticeGenerator_RowSizeTextView);
            if (feedbackParam.HasValue && flowAmount.HasValue && 
                probabilityNewNodeIsFood.HasValue && minFoodSources.HasValue && rowSize.HasValue)
            {
                try
                {
                    var generationConfig = new LatticeSlimeNetworkGenerationConfig(rowSize.Value, 
                        probabilityNewNodeIsFood.Value, minFoodSources.Value);
                    return new SimulationConfiguration(generationConfig, flowAmount.Value, feedbackParam.Value);
                } catch (ArgumentException e)
                {
                    string errorMsg = "Invalid parameter: " + e.Message;
                    logger.Info(errorMsg);
                    DisplayError(errorMsg);
                }
            }
            DisplayErrors();
            return null;
        }

        private Button BeginSimulationButton()
        {
            if (beginSimulationButton == null)
            {
                beginSimulationButton = new Button(new Label("Begin simulation"));
            }
            return beginSimulationButton;
        }
        
        private Widget MatrixGenerationProperties()
        {
            VBox vBox = new VBox();
            vBox.Add(RowSizeBox());
            vBox.Add(ProbabilityNewNodeIsFoodBox());
            vBox.Add(MinimumNumberFoodSourcesBox());
            return vBox;
        }

        private Widget MinimumNumberFoodSourcesBox()
        {
            Label description = new Label("Minimum number of food sources in the network");
            TextView textView = new TextView();
            textView.Buffer.Text = DEFAULT_CONFIG.GenerationConfig.MinimumFoodSources.ToString();
            latticeGenerator_MinimumFoodSourcesTextView = textView;
            return HBox(description, textView);
        }

        private Widget ProbabilityNewNodeIsFoodBox()
        {
            Label description = new Label("Probability new nodes in network are food sources");
            TextView textView = new TextView();
            textView.Buffer.Text = DEFAULT_CONFIG.GenerationConfig.ProbabilityNewNodeIsFoodSource.ToString();
            latticeGenerator_ProbabiltyOfNewFoodTextView = textView;
            return HBox(description, textView);
        }

        private Widget RowSizeBox()
        {
            Label description = new Label("Number of rows in lattice to generate");
            TextView textView = new TextView();
            textView.Buffer.Text = DEFAULT_CONFIG.GenerationConfig.Size.ToString();
            latticeGenerator_RowSizeTextView = textView;
            return HBox(description, textView);
        }

        private Widget FeedbackParameterBox()
        {
            Label description = new Label("Feedback parameter for updating slime simulation at each step");
            TextView textView = new TextView();
            textView.Buffer.Text = DEFAULT_CONFIG.FeedbackParam.ToString();
            feedbackParamTextView = textView;
            return HBox(description, textView);
        }

        private HBox FlowBox()
        {
            Label description = new Label("Flow through system per iteration");
            TextView textView = new TextView();
            textView.Buffer.Text = DEFAULT_CONFIG.FlowAmount.ToString();
            flowAmountTextView = textView;
            return HBox(description, textView);
        }

        private HBox HBox(Label description, TextView textView)
        {
            HBox hBox = new HBox();
            hBox.Add(description);
            hBox.Add(textView);
            textViewLabelMapping[textView] = description;
            return hBox;
        }
        
        public double? ExtractDoubleFromView(TextView view)
        {
            double result;
            var success = double.TryParse(view.Buffer.Text.ToString(), out result);
            if (success)
            {
                return result;
            }
            else
            {
                HighlightErrorOn(view);
                return null;
            }
        }

        private int? ExtractIntFromView(TextView view)
        {
            int result;
            var success = int.TryParse(view.Buffer.Text.ToString(), out result);
            if (success)
            {
                return result;
            }
            else
            {
                HighlightErrorOn(view);
                return null;
            }
        }

        private void HighlightErrorOn(TextView view)
        {
            DisplayError("Not valid value for input box matching: " + textViewLabelMapping[view]);
        }

        private void DisplayError(string error)
        {
            errors.Add(error);
        }
        private void DisplayErrors()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string error in errors)
            {
                if (sb.Length > 0)
                {
                    sb.Append(Environment.NewLine);
                }
                sb.Append(error);
            }
            errorLabel.Text = sb.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            else if (disposing)
            {
                base.Dispose(true);
                beginSimulationButton.Dispose();
                errorLabel.Dispose();
            }
            disposed = true;
            logger.Debug("[Dispose : bool] finished from within " + this);
        }
    }
}
