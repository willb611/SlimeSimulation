using System;
using Gtk;
using Newtonsoft.Json;
using NLog;
using SlimeSimulation.Controller.Factories;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.Model.Simulation.Persistence;
using SlimeSimulation.View;
using SlimeSimulation.View.Windows;

namespace SlimeSimulation.Controller.WindowController
{
    public class ApplicationStartWindowController : AbstractSimulationControllerStarter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static ApplicationStartWindow _applicationStartWindow;

        private readonly SimulationLoader _simulationLoader = new SimulationLoader();

        public override void OnClickCallback(Widget widget, ButtonPressEventArgs args)
        {
            Logger.Debug("[OnClickCallback] Clicked, doing nothing");
        }

        public override void Render()
        {
            using (var gtkLifecycleController = GtkLifecycleController.Instance)
            {
                using (AbstractWindow = new ApplicationStartWindow(this))
                {
                    _applicationStartWindow = (ApplicationStartWindow)AbstractWindow;
                    gtkLifecycleController.Display(AbstractWindow);
                    Logger.Debug("[Render] Left main GTK loop ? ");
                }
            }
        }

        public void LoadPreviousSimulationButtonClicked(string saveLocation)
        {
            Application.Invoke(delegate
            {
                try
                {
                    var simulationSave = _simulationLoader.LoadSimulationFromFile(saveLocation);
                    var simulationControllerFactory = new SimulationControllerFactory(GtkLifecycleController.Instance);
                    var simulationController = simulationControllerFactory.MakeSimulationController(this, simulationSave);
                    AbstractWindow.Hide();
                    Logger.Debug("[LoadPreviousSimulationButtonClicked] Loaded. Using config: {0}",
                        JsonConvert.SerializeObject(simulationSave.SimulationConfiguration, SerializationSettings.JsonSerializerSettings));
                    simulationController.RunSimulation();
                }
                catch (Exception e)
                {
                    var errorMsg = "Unable to load previous simulation due to exception: " + e;
                    Logger.Error(errorMsg);
                }
            });
        }

        public void StartNewSimulationButtonClicked()
        {
            Application.Invoke(delegate
            {
                AbstractWindow.Hide();
                new NewSimulationStarterWindowController(this).Render();
            });
        }

        public void Display()
        {
            AbstractWindow.Display();
        }

        public void StartNewSimulationFromFileDescriptionButtonClicked()
        {
            Application.Invoke(delegate
            {
                AbstractWindow.Hide();
                new NewSimulationFromFileDescriptionWindowController(this).Render();
            });
        }
    }
}
