using System;
using Gtk;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent
{
    internal class SimulationStepButton : Button
    {
        private readonly SimulationStepWindowControllerTemplate _controller;

        public SimulationStepButton(SimulationStepWindowControllerTemplate controller) : base(new Label("Next Simulation Step"))
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
