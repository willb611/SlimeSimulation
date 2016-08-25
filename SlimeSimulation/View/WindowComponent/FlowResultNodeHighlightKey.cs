using Gtk;
using SlimeSimulation.Controller.WindowComponentController;

namespace SlimeSimulation.View.WindowComponent
{
    public class FlowResultNodeHighlightKey
    {
        public Widget GetVisualKey()
        {
            VBox key = new VBox(true, 10);

            key.Add(new Label("Node colour key"));

            var sourcePart = new HBox(true, 10);
            sourcePart.Add(new Label("Source"));
            sourcePart.Add(GetBoxColour(FlowResultNodeViewController.SourceColour));

            HBox sinkPart = new HBox(true, 10);
            sinkPart.Add(new Label("Sink"));
            sinkPart.Add(GetBoxColour(FlowResultNodeViewController.SinkColour));

            HBox normalPart = new HBox(true, 10);
            normalPart.Add(new Label("Normal node"));
            normalPart.Add(GetBoxColour(FlowResultNodeViewController.NormalNodeColour));

            key.Add(sourcePart);
            key.Add(sinkPart);
            key.Add(normalPart);
            return key;
        }

        private DrawingArea GetBoxColour(Rgb color)
        {
            return new ColorArea(color);
        }
    }

}
