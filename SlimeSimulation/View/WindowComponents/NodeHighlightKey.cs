using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdk;
using Gtk;
using Cairo;

namespace SlimeSimulation.View {
    public class NodeHighlightKey {

        public Gtk.Widget GetVisualKey() {
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

        private DrawingArea GetBoxColour(RGB color) {
            return new ColorArea(color);
        }
    }

    public class ColorArea : Gtk.DrawingArea {
        private RGB color;

        public ColorArea(RGB color) {
            this.color = color;
        }

        protected override bool OnExposeEvent(EventExpose args) {
            using (Context context = Gdk.CairoHelper.Create(args.Window)) {
                Gdk.Rectangle allocation = this.Allocation;
                DrawRectangle(context, allocation.Width, allocation.Height);
            }
            return true;
        }

        private void DrawRectangle(Cairo.Context graphic, int width, int height) {
            graphic.Save();

            graphic.SetSourceRGB(color.R, color.G, color.B);
            graphic.Rectangle(0, 0, width, height);
            graphic.Fill();
            graphic.Stroke();

            graphic.Restore();
        }
    }
}
