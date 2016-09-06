using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using NLog;
using SlimeSimulation.View.Windows.Templates;

namespace SlimeSimulation.Controller.WindowController.Templates
{
    public abstract class AbstractSimulationControllerStarter : AbstractWindowController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void DisplayError(string error)
        {
            Logger.Error(error);
            Application.Invoke(delegate
            {
                MessageDialog errorDialog = new MessageDialog(AbstractWindow.Window, DialogFlags.DestroyWithParent,
                    MessageType.Error, ButtonsType.Ok,
                    "Unexpected error. Simulation tried to do a step when an step was in progress.")
                {
                    Title = "Unexpected error"
                };
                errorDialog.Run();
                errorDialog.Destroy();
            });
        }

        public void RegainControlFromFinishedSimulation(SimulationController controller)
        {
            controller.Dispose();
            Logger.Info("[FinishSimulation] Finished one simulation");
            AbstractWindow.Display();
        }
    }
}
