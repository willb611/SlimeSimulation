using System;
using Gtk;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent
{
    internal class SimulationStepButton : Button
    {
        private readonly SimulationStepWindowController _controller;

        public SimulationStepButton(SimulationStepWindowController controller) : base(new Label("Next Simulation Step"))
        {
            _controller = controller;
            Clicked += OnClicked;
        }

        private void OnClicked(object sender, EventArgs eventArgs)
        {
            _controller.OnStepCompleted();
        }
    }
}
