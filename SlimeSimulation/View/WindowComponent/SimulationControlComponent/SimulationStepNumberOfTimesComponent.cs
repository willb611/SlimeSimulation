using System;
using Gtk;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.StdLibHelpers;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent
{
    public class SimulationStepNumberOfTimesComponent : VBox
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly SimulationStepAbstractWindowController _simulationStepAbstractWindowController;
        private readonly Window _parentWindow;
        private readonly SimulationControlInterfaceValues _simulationControlInterfaceValues;
        
        internal readonly TextView _numberOfTimesToStepTextView;
        internal CheckButton _shouldSaveEveryNSteps;
        internal TextView _nStepsToSaveAt;

        public SimulationStepNumberOfTimesComponent(SimulationStepAbstractWindowController simulationStepAbstractWindowController,
            Window enclosingWindow, SimulationControlInterfaceValues simulationControlInterfaceValues) : base(true, 10)
        {
            _simulationControlInterfaceValues = simulationControlInterfaceValues;
            _parentWindow = enclosingWindow;
            _simulationStepAbstractWindowController = simulationStepAbstractWindowController;
            _numberOfTimesToStepTextView = new TextView {Buffer = {Text = simulationControlInterfaceValues.NumberOfStepsToRun.ToString()}};
            
            Add(StepButtonAndAmountInput());
            Add(SaveEveryStepAmountInput(simulationControlInterfaceValues.ShouldSaveEveryNSteps, simulationControlInterfaceValues.IntervalAtWhichToSaveSimulationWhileRunningSteps));
        }

        private Widget SaveEveryStepAmountInput(bool initialShouldSaveEveryNSteps, int initialIntervalAtWhichToSaveSimulationWhileRunningSteps)
        {
            _shouldSaveEveryNSteps = new CheckButton("Should save simulation after stepping a certain amount");
            _shouldSaveEveryNSteps.Active = initialShouldSaveEveryNSteps;
            _nStepsToSaveAt = new TextView { Buffer = { Text = initialIntervalAtWhichToSaveSimulationWhileRunningSteps.ToString() } };

            return new HBox() { _shouldSaveEveryNSteps, new Label("interval to save steps at"), _nStepsToSaveAt };
        }

        private HBox StepButtonAndAmountInput()
        {
            var doStepsButton = new Button(new Label("Run number of steps"));
            doStepsButton.Clicked += DoStepsButtonOnClicked;
            return new HBox {new Label("Number of steps to run"), _numberOfTimesToStepTextView, doStepsButton};
        }

        private void DoStepsButtonOnClicked(object sender, EventArgs eventArgs)
        {
            if (_simulationControlInterfaceValues.ShouldFlowResultsBeDisplayed)
            {
                MessageDialog confirmSkipFlowResultsDialog = new MessageDialog(_parentWindow, DialogFlags.DestroyWithParent, MessageType.Question, ButtonsType.OkCancel,
                    "Flow results are set to be displayed, running this will disable show flow results. Continue?");
                confirmSkipFlowResultsDialog.Title = "Ok to disable showing flow results?";
                ResponseType response = (ResponseType)confirmSkipFlowResultsDialog.Run();
                if (response == ResponseType.DeleteEvent || response == ResponseType.Cancel)
                {
                    confirmSkipFlowResultsDialog.Destroy();
                    Logger.Debug("[DoStepsButtonOnClicked] Returning as user was not ok with skipping flow result windows");
                    return;
                }
                Logger.Debug("[DoStepsButtonOnClicked] Skip flow results was not enabled, but user confirmed ok to disable flow results");
                _simulationControlInterfaceValues.ShouldFlowResultsBeDisplayed = false;
            }
            TryToRunSteps();
        }

        private void TryToRunSteps()
        {
            var numberOfSteps = _numberOfTimesToStepTextView.ExtractIntFromView();
            if (numberOfSteps.HasValue)
            {
                if (_shouldSaveEveryNSteps.Active)
                {
                    RunNumberOfStepsSavingAtIntervals(numberOfSteps.Value);
                }
                else
                {
                    UpdateInterfaceValuePersistence(numberOfSteps.Value);
                    _simulationStepAbstractWindowController.RunNumberOfSteps(numberOfSteps.Value);
                }
            }
            else
            {
                var error = $"Given input \"{_numberOfTimesToStepTextView.Buffer.Text}\" was not a number";
                DisplayError(error);
            }
        }

        private void RunNumberOfStepsSavingAtIntervals(int numberOfSteps)
        {
            var simulationSaveInterval = _nStepsToSaveAt.ExtractIntFromView();
            if (simulationSaveInterval.HasValue)
            {
                UpdateInterfaceValuePersistence(numberOfSteps, simulationSaveInterval.Value);
                _simulationStepAbstractWindowController.RunNumberOfStepsSavingEvery(numberOfSteps,
                    simulationSaveInterval.Value);
            }
            else
            {
                var error = $"Given save interval for steps \"{_nStepsToSaveAt.Buffer.Text}\" wasn't a number.";
                DisplayError(error);
            }
        }

        private void UpdateInterfaceValuePersistence(int numberOfStepsToRun)
        {
            _simulationControlInterfaceValues.NumberOfStepsToRun = numberOfStepsToRun;
            _simulationControlInterfaceValues.ShouldSaveEveryNSteps = false;
        }
        private void UpdateInterfaceValuePersistence(int numberOfStepsToRun, int intervalAtWhichToSaveSimulationWhileRunning)
        {
            _simulationControlInterfaceValues.NumberOfStepsToRun = numberOfStepsToRun;
            _simulationControlInterfaceValues.ShouldSaveEveryNSteps = true;
            _simulationControlInterfaceValues.IntervalAtWhichToSaveSimulationWhileRunningSteps =
                intervalAtWhichToSaveSimulationWhileRunning;
        }

        private void DisplayError(string error)
        {
            Logger.Debug(error);
            var errorDialogBox = new MessageDialog(_parentWindow, DialogFlags.DestroyWithParent,
                MessageType.Error, ButtonsType.Close,
                error);
            errorDialogBox.Run();
            errorDialogBox.Destroy();
        }
    }
}
