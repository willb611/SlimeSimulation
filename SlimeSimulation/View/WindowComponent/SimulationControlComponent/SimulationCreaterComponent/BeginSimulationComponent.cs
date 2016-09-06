using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationCreaterComponent
{
    public class BeginSimulationComponent : Button
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private ApplicationStartWindow _enclosingWindow;
        private ApplicationStartWindowController _enclosingWindowController;

        public BeginSimulationComponent(ApplicationStartWindow enclosingWindow,
            ApplicationStartWindowController controller)
            : base (new Label("Begin simulation")) {
            _enclosingWindow = enclosingWindow;
            Clicked += BeginSimulationComponent_Clicked;
        }

        private void BeginSimulationComponent_Clicked(object sender, EventArgs e)
        {
            Logger.Debug("[BeginSimulationComponent_Clicked] Entered");
            var config = _enclosingWindow.GetConfigFromViews();
            if (config == null)
            {
                Logger.Info("[BeginSimulationComponent_Clicked] Not starting simulation due to invalid parameters");
            }
            else
            {
                _enclosingWindowController.StartSimulation(config);
            }
        }
    }
}
