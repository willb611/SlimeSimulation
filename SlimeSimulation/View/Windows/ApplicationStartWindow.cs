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
using SlimeSimulation.StdLibHelpers;

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

        private TextView _feedbackParamTextView;
        private FlowAmountControlComponent _flowAmountControlComponent;

        private Label _errorLabel;


        private List<string> _errors = new List<string>();
        private LatticeGenerationControlComponent _latticeGenerationControlComponent;
        private FeedbackParameterControlComponent _slimeNetworkAdaptionComponent;

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
            container.Add(_slimeNetworkAdaptionComponent);
            container.Add(_latticeGenerationControlComponent);
            container.Add(_shouldAllowDisconnectionCheckButton);
            container.Add(_beginSimulationButton);
            container.Add(ErrorDisplayComponent());
            window.Add(container);
            _beginSimulationButton.Clicked += BeginSimulationButton_Clicked;
            window.Unmaximize();
        }

        private VBox MakeComponents()
        {
            _slimeNetworkAdaptionComponent = new FeedbackParameterControlComponent(_defaultConfig.SlimeNetworkAdaptionCalculatorConfig);
            _flowAmountControlComponent = new FlowAmountControlComponent(_defaultConfig.FlowAmount);
            _shouldAllowDisconnectionCheckButton = new ShouldAllowSlimeDisconnectionButton(_defaultConfig.ShouldAllowDisconnection);
            _beginSimulationButton = new BeginSimulationComponent();
            _errorLabel = new Label();
            _latticeGenerationControlComponent =
                new LatticeGenerationControlComponent(_defaultConfig.GenerationConfig);
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
            var slimeNetworkAdaptionConfig = _slimeNetworkAdaptionComponent.ReadConfiguration();
            if (slimeNetworkAdaptionConfig == null)
            {
                DisplayAllErrors(_slimeNetworkAdaptionComponent.Errors());
            }
            double? flowAmount = _flowAmountControlComponent.TextView.ExtractDoubleFromView();
            HighglightErrorIfNull(flowAmount, _flowAmountControlComponent.TextView);
            bool shouldAllowDisconnection = _shouldAllowDisconnectionCheckButton.Active;
            if (slimeNetworkAdaptionConfig != null && flowAmount.HasValue)
            {
                try
                {
                    var generationConfig = _latticeGenerationControlComponent.ReadGenerationConfig();
                    if (generationConfig == null)
                    {
                        DisplayAllErrors(_latticeGenerationControlComponent.ErrorMessages());
                    }
                    else
                    {
                        return new SimulationConfiguration(generationConfig, flowAmount.Value,
                            slimeNetworkAdaptionConfig, shouldAllowDisconnection);
                    }
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

        private void DisplayAllErrors(List<string> errors)
        {
            foreach (var errorMessage in errors)
            {
                DisplayError(errorMessage);
            }
        }

        private void HighglightErrorIfNull(double? nullable, TextView parameterToLogErrorOn)
        {
            if (nullable == null)
            {
                HighlightErrorOn(parameterToLogErrorOn);
            }
        }
        private void HighglightErrorIfNull(int? nullable, TextView parameterToLogErrorOn)
        {
            if (nullable == null)
            {
                HighlightErrorOn(parameterToLogErrorOn);
            }
        }

        private HBox MakeHbox(Label description, TextView textView)
        {
            HBox hBox = new HBox();
            hBox.Add(description);
            hBox.Add(textView);
            _textViewLabelMapping[textView] = description;
            return hBox;
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
