using Cairo;
using Gdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.View.WindowComponent
{
    public class ColorArea : Gtk.DrawingArea
    {
        private RGB color;

        public ColorArea(RGB color)
        {
            this.color = color;
        }

        protected override bool OnExposeEvent(EventExpose args)
        {
            using (Context context = Gdk.CairoHelper.Create(args.Window))
            {
                Gdk.Rectangle allocation = this.Allocation;
                DrawRectangle(context, allocation.Width, allocation.Height);
            }
            return true;
        }

        private void DrawRectangle(Cairo.Context graphic, int width, int height)
        {
            graphic.Save();

            graphic.SetSourceRGB(color.R, color.G, color.B);
            graphic.Rectangle(0, 0, width, height);
            graphic.Fill();
            graphic.Stroke();

            graphic.Restore();
        }
    }
}
