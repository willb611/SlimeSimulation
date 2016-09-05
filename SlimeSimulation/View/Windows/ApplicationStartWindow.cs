using System;
using System.Collections.Generic;
using System.Text;
using Gtk;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.View.Windows.Templates;
using SlimeSimulation.View.WindowComponent.SimulationControlComponent;
using SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationCreaterComponent;

namespace SlimeSimulation.View.Windows
{
    public class ApplicationStartWindow : WindowTemplate
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ApplicationStartWindowController _windowController;
        private readonly SimulationConfiguration _defaultConfig = new SimulationConfiguration();
        private readonly Dictionary<TextView, Label> _textViewLabelMapping = new Dictionary<TextView, Label>();

        private Button _beginSimulationButton;
        private CheckButton _shouldAllowDisconnectionCheckButton;

        private TextView _latticeGeneratorRowSizeTextView;
        private TextView _latticeGeneratorProbabiltyOfNewFoodTextView;
        private TextView _latticeGeneratorMinimumFoodSourcesTextView;

        private TextView _feedbackParamTextView;
        private FlowAmountControlComponent _flowAmountControlComponent;

        private Label _errorLabel;


        private List<string> _errors = new List<string>();


        public ApplicationStartWindow(string windowTitle, ApplicationStartWindowController windowController)
            : base(windowTitle, windowController)
        {
            _windowController = windowController;
            Window.Resize(600, 600);
        }

        protected override void AddToWindow(Window window)
        {
            var container = MakeComponents();
            container.Add(_flowAmountControlComponent);
            container.Add(FeedbackParameterBox());
            container.Add(MatrixGenerationProperties());
            container.Add(_shouldAllowDisconnectionCheckButton);
            container.Add(_beginSimulationButton);
            container.Add(ErrorDisplayComponent());
            window.Add(container);
            _beginSimulationButton.Clicked += BeginSimulationButton_Clicked;
            window.Unmaximize();
        }

        private VBox MakeComponents()
        {
            _flowAmountControlComponent = new FlowAmountControlComponent(_defaultConfig.FlowAmount);
            _shouldAllowDisconnectionCheckButton = new ShouldAllowSlimeDisconnectionButton(_defaultConfig.ShouldAllowDisconnection);
            _beginSimulationButton = new BeginSimulationComponent();
            _errorLabel = new Label();
            return new VBox();
        }

        private void BeginSimulationButton_Clicked(object obj, EventArgs args)
        {
            Logger.Debug("[BeginSimulationButton_Clicked] Entered");
            var config = GetConfigFromViews();
            if (config == null)
            {
                Logger.Info("[BeginSimulationButton_Clicked] Not starting simulation due to invalid parameters");
            }
            else
            {
                _windowController.StartSimulation(config);
            }
        }

        private Widget ErrorDisplayComponent()
        {
            HBox box = new HBox();
            box.Add(_errorLabel);
            return box;
        }


        private SimulationConfiguration GetConfigFromViews()
        {
            _errors = new List<string>();
            double? feedbackParam = ExtractDoubleFromView(_feedbackParamTextView);
            double? flowAmount = ExtractDoubleFromView(_flowAmountControlComponent.TextView);
            double? probabilityNewNodeIsFood = ExtractDoubleFromView(_latticeGeneratorProbabiltyOfNewFoodTextView);
            int? minFoodSources = ExtractIntFromView(_latticeGeneratorMinimumFoodSourcesTextView);
            int? rowSize = ExtractIntFromView(_latticeGeneratorRowSizeTextView);
            bool shouldAllowDisconnection = _shouldAllowDisconnectionCheckButton.Active;
            if (feedbackParam.HasValue && flowAmount.HasValue &&
                probabilityNewNodeIsFood.HasValue && minFoodSources.HasValue && rowSize.HasValue)
            {
                try
                {
                    var generationConfig = new LatticeGraphWithFoodSourcesGenerationConfig(rowSize.Value,
                        probabilityNewNodeIsFood.Value, minFoodSources.Value);
                    return new SimulationConfiguration(generationConfig, flowAmount.Value,
                        new SlimeNetworkAdaptionCalculatorConfig(feedbackParam.Value), shouldAllowDisconnection);
                }
                catch (ArgumentException e)
                {
                    string errorMsg = "Invalid parameter: " + e.Message;
                    Logger.Info(errorMsg);
                    DisplayError(errorMsg);
                }
            }
            DisplayErrors();
            return null;
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
            textView.Buffer.Text = _defaultConfig.GenerationConfig.MinimumFoodSources.ToString();
            _latticeGeneratorMinimumFoodSourcesTextView = textView;
            return HBox(description, textView);
        }

        private Widget ProbabilityNewNodeIsFoodBox()
        {
            Label description = new Label("Probability new nodes in network are food sources");
            TextView textView = new TextView();
            textView.Buffer.Text = _defaultConfig.GenerationConfig.ProbabilityNewNodeIsFoodSource.ToString();
            _latticeGeneratorProbabiltyOfNewFoodTextView = textView;
            return HBox(description, textView);
        }

        private Widget RowSizeBox()
        {
            Label description = new Label("Number of rows in lattice to generate");
            TextView textView = new TextView();
            textView.Buffer.Text = _defaultConfig.GenerationConfig.Size.ToString();
            _latticeGeneratorRowSizeTextView = textView;
            return HBox(description, textView);
        }

        private Widget FeedbackParameterBox()
        {
            Label description = new Label("Feedback parameter for updating slime simulation at each step");
            TextView textView = new TextView();
            textView.Buffer.Text = _defaultConfig.SlimeNetworkAdaptionCalculatorConfig.FeedbackParam.ToString();
            _feedbackParamTextView = textView;
            return HBox(description, textView);
        }

        private HBox HBox(Label description, TextView textView)
        {
            HBox hBox = new HBox();
            hBox.Add(description);
            hBox.Add(textView);
            _textViewLabelMapping[textView] = description;
            return hBox;
        }

        public double? ExtractDoubleFromView(TextView view)
        {
            double result;
            var success = double.TryParse(view.Buffer.Text, out result);
            if (success)
            {
                return result;
            }
            HighlightErrorOn(view);
            return null;
        }

        private int? ExtractIntFromView(TextView view)
        {
            int result;
            var success = int.TryParse(view.Buffer.Text, out result);
            if (success)
            {
                return result;
            }
            HighlightErrorOn(view);
            return null;
        }

        private void HighlightErrorOn(TextView view)
        {
            DisplayError("Not valid value for input box matching: "
                         + _textViewLabelMapping[view].Text);
        }

        private void DisplayError(string error)
        {
            _errors.Add(error);
        }

        private void DisplayErrors()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string error in _errors)
            {
                if (sb.Length > 0)
                {
                    sb.Append(Environment.NewLine);
                }
                sb.Append(error);
            }
            _errorLabel.Text = sb.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }
            if (disposing)
            {
                base.Dispose(true);
                _beginSimulationButton.Dispose();
                _errorLabel.Dispose();
            }
            Disposed = true;
            Logger.Debug("[Dispose : bool] finished from within " + this);
        }

        ~ApplicationStartWindow()
        {
            Dispose(false);
        }
    }
}
