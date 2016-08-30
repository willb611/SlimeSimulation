using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController;

namespace SlimeSimulation.View.Windows
{
    internal class ShouldFlowResultsBeDisplayedControlComponent : CheckButton
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly SlimeNetworkWindowController _controller;

        public ShouldFlowResultsBeDisplayedControlComponent(SlimeNetworkWindowController controller) : base("Should simulation step result (flow graph) be displayed?")
        {
            _controller = controller;
            Setup();
        }

        private void Setup()
        {
            Active = _controller.WillFlowResultsBeDisplayed;
            Toggled += delegate
            {
                _controller.ToggleAreFlowResultsDisplayed(Active);
            };
            Logger.Debug("[ShouldSimulationStepResultsBeDisplayedInput] Setting initial value to: " + _controller.WillFlowResultsBeDisplayed);
        }
        
        public void ReDraw()
        {
            Active = _controller.WillFlowResultsBeDisplayed;
        }
    }
}
