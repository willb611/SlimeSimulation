using System;
using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController;

namespace SlimeSimulation.View.Windows
{
    internal class NewSimulationFromDescriptionComponent : Button
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly LoadFromDescriptionWindow _enclosingWindow;
        private readonly NewSimulationFromFileDescriptionWindowController _enclosingWindowController;

        public NewSimulationFromDescriptionComponent(LoadFromDescriptionWindow enclosingWindow, NewSimulationFromFileDescriptionWindowController controller)
            : base (new Label("Begin simulation")) {
            _enclosingWindow = enclosingWindow;
            _enclosingWindowController = controller;
            Clicked += CreateNewSimulationFromFileDescriptionComponent_Clicked;
        }

        private void CreateNewSimulationFromFileDescriptionComponent_Clicked(object sender, EventArgs e)
        {
            Logger.Debug("[BeginSimulationComponent_Clicked] Entered");
            var config = _enclosingWindow.GetConfigFromViews();
            if (config != null)
            {
                _enclosingWindowController.StartSimulationWithDescriptionFromFile(config);
            }
            else
            {
                Logger.Warn("[CreateNewSimulationFromFileDescriptionComponent_Clicked] Not starting simulation due to invalid parameters");
            }
        }
    }
}
