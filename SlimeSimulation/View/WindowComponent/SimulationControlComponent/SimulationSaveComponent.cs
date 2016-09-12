using System;
using Gtk;
using NLog;
using SlimeSimulation.Controller;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent
{
    public class SimulationSaveComponent : Button
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly SimulationController _simulationController;
        private readonly Window _parentWindow;

        public SimulationSaveComponent(SimulationController simulationController, Window window) : base("Save simulation to file")
        {
            _simulationController = simulationController;
            Clicked += OnClicked;
            _parentWindow = window;
        }

        private void OnClicked(object sender, EventArgs eventArgs)
        {
            var exception = _simulationController.SaveSimulation();
            if (exception == null)
            {
                DisplaySaveSuccess(_simulationController.LastAttemptedSaveLocation);
            }
            else
            {
                DisplaySaveError($"Unable to save simulation to given file location {_simulationController.LastAttemptedSaveLocation} due to exception {exception}");
            }
        }

        private void DisplaySaveError(string errorMessage)
        {
            MessageDialog messageDialog = new MessageDialog(_parentWindow, DialogFlags.DestroyWithParent,
                MessageType.Error, ButtonsType.Ok, errorMessage);
            messageDialog.Title = "Unable to save";
            messageDialog.Run();
            messageDialog.Destroy();
        }

        private void DisplaySaveSuccess(string filepath)
        {
            MessageDialog messageDialog = new MessageDialog(_parentWindow, DialogFlags.DestroyWithParent,
                MessageType.Info, ButtonsType.Ok,
                string.Format("Saving simulation to {0} was successful", filepath));
            messageDialog.Title = "Save success";
            messageDialog.Run();
            messageDialog.Destroy();
        }
    }
}

