using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdk;
using Gtk;
using Cairo;
using SlimeSimulation.Controller;
using SlimeSimulation.Controller.WindowComponentController;
using SlimeSimulation.View.WindowComponent;

namespace SlimeSimulation.View.WindowComponent
{
    public class NodeHighlightKey
    {
        public Widget GetVisualKey()
        {
            VBox key = new VBox(true, 10);

            HBox sourcePart = new HBox(true, 10);
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
