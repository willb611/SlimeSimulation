using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using Newtonsoft.Json;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller;
using SlimeSimulation.Model.Simulation;
using SlimeSimulation.Model.Simulation.Persistence;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent
{
    public class SimulationSaveComponent : Button
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string SaveLocationPrefix = "save";
        private const string SaveLocationFileExtension = ".sim";
        private const string CurrentDateTimeSafeForFilenameFormat = "yyyy.MM.dd-H.mm.ss";

        private readonly SimulationController _simulationController;
        private readonly Window _parentWindow;
        private readonly SimulationSaver _simulationSaver = new SimulationSaver();

        private SimulationState SimulationState => _simulationController.GetSimulationState();
        private SimulationControlInterfaceValues InterfaceValues => _simulationController.SimulationControlBoxConfig;
        private SimulationConfiguration SimulationConfiguration => _simulationController.Configuration;

        public SimulationSaveComponent(SimulationController simulationController, Window window) : base("Save simulation to file")
        {
            _simulationController = simulationController;
            Clicked += OnClicked;
            _parentWindow = window;
        }

        private void OnClicked(object sender, EventArgs eventArgs)
        {
            var currentState = new SimulationSave(SimulationState, InterfaceValues, SimulationConfiguration);
            DateTime currentDateTime = DateTime.Now;
            var dateTimeString = currentDateTime.ToString(CurrentDateTimeSafeForFilenameFormat);
            var saveLocation = SaveLocationPrefix + dateTimeString + SaveLocationFileExtension;
            var exception = _simulationSaver.SaveSimulation(currentState, saveLocation);
            if (exception == null)
            {
                DisplaySaveSuccess(saveLocation);
            }
            else
            {
                DisplaySaveError($"Unable to save simulation to given file location {saveLocation} due to exception {exception}");
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

