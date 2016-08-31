using Gtk;
using SlimeSimulation.Controller.WindowComponentController;

namespace SlimeSimulation.View.WindowComponent
{
    public class FlowResultNodeHighlightKey : VBox
    {

        public FlowResultNodeHighlightKey() : base(true, 10)
        {
            Add(new Label("Node colour key"));

            var sourcePart = new HBox(true, 10);
            sourcePart.Add(new Label("Source"));
            sourcePart.Add(new ColorArea(FlowResultNodeViewController.SourceColour));

            var sinkPart = new HBox(true, 10);
            sinkPart.Add(new Label("Sink"));
            sinkPart.Add(new ColorArea(FlowResultNodeViewController.SinkColour));

            var normalPart = new HBox(true, 10);
            normalPart.Add(new Label("Normal node"));
            normalPart.Add(new ColorArea(FlowResultNodeViewController.NormalNodeColour));

            Add(sourcePart);
            Add(sinkPart);
            Add(normalPart);
        }
    }

}
