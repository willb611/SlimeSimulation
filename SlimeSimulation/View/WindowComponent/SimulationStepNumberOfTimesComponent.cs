using System;
using GLib;
using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent
{
    internal class SimulationStepNumberOfTimesComponent : HBox
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly SimulationStepWindowController _simulationStepWindowController;
        private readonly TextView _numberOfTimesToStepTextView;
        private readonly Window _parentWindow;

        public SimulationStepNumberOfTimesComponent(SimulationStepWindowController simulationStepWindowController, Window enclosingWindow) : base(true, 10)
        {
            _parentWindow = enclosingWindow;
            _simulationStepWindowController = simulationStepWindowController;
            _numberOfTimesToStepTextView = new TextView {Buffer = {Text = "1"}};
            var doStepsButton = new Button(new Label("Run number of steps"));
            doStepsButton.Clicked += DoStepsButtonOnClicked;

            Add(new Label("Number of steps to run"));
            Add(_numberOfTimesToStepTextView);
            Add(doStepsButton);
        }

        private void DoStepsButtonOnClicked(object sender, EventArgs eventArgs)
        {
            int numberOfSteps;
            var textRead = _numberOfTimesToStepTextView.Buffer.Text;
            var success = int.TryParse(textRead, out numberOfSteps);
            if (success)
            {
                _simulationStepWindowController.RunNumberOfSteps(numberOfSteps);
            }
            else
            {
                var error = $"Given input \"{textRead}\" was not a number";
                Logger.Debug(error);
                var errorDialogBox = new MessageDialog(_parentWindow, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Close,
                    error);
                errorDialogBox.Run();
                errorDialogBox.Destroy();
            }
        }
    }
}
