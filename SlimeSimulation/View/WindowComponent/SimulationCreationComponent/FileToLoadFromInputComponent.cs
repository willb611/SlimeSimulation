using Gtk;

namespace SlimeSimulation.View.WindowComponent.SimulationCreationComponent
{
    public class FileToLoadFromInputComponent : HBox
    {
        private readonly TextView _inputTextView;

        public FileToLoadFromInputComponent() : this("exampleFile.txt") { }
        public FileToLoadFromInputComponent(string defaultFileName)
        {
            _inputTextView = new TextView();
            _inputTextView.Buffer.Text = defaultFileName;

            Add(new Label("File to use as description of simulation"));
            Add(_inputTextView);
        }

        public string ReadInput()
        {
            return _inputTextView.Buffer.Text;
        }
    }
}
