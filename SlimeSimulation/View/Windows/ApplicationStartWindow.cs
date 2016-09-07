using System;
using Gtk;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.View.Windows.Templates;

namespace SlimeSimulation.View.Windows
{
    public class ApplicationStartWindow : AbstractWindow
    {
        private readonly ApplicationStartWindowController _applicationStartWindowController;

        public ApplicationStartWindow(ApplicationStartWindowController applicationStartWindowController) 
            : base("Slime simulation start window", applicationStartWindowController)
        {
            _applicationStartWindowController = applicationStartWindowController;
        }

        protected override void AddToWindow(Window window)
        {
            Window.Resize(400, 400);
            Window.Unmaximize();

            VBox container = new VBox(true, 10);
            container.Add(StartNewSimulationComponent());
            container.Add(LoadPreviousSimulationComponent());
            var frame = new Frame {BorderWidth = 10};
            frame.Add(container);
            window.Add(frame);
        }

        private Widget LoadPreviousSimulationComponent()
        {
            var inputFileNameTextView = new TextView {Buffer = {Text = "save.sim"}};
            var button = LoadPreviousSimulationButton(inputFileNameTextView);

            return new HBox(true, 10) {inputFileNameTextView, button};
        }

        private Button LoadPreviousSimulationButton(TextView textView)
        {
            var loadPreviousSimulationButton = new Button("Load a previous simulation from the specified location");
            loadPreviousSimulationButton.Clicked += delegate (object sender, EventArgs args)
            {
                _applicationStartWindowController.LoadPreviousSimulationButtonClicked(textView.Buffer.Text);
            };
            return loadPreviousSimulationButton;
        }

        private Widget StartNewSimulationComponent()
        {
            var startNewSimulationButton = new Button("Start a new simulation");
            startNewSimulationButton.Clicked += delegate(object sender, EventArgs args)
            {
                _applicationStartWindowController.StartNewSimulationButtonClicked();
            };
            return startNewSimulationButton;
        }
    }
}
