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
        private readonly Rgb _color;

        public ColorArea(Rgb color)
        {
            _color = color;
        }

        protected override bool OnExposeEvent(EventExpose args)
        {
            using (Context context = CairoHelper.Create(args.Window))
            {
                Gdk.Rectangle allocation = Allocation;
                DrawRectangle(context, allocation.Width, allocation.Height);
            }
            return true;
        }

        private void DrawRectangle(Context graphic, int width, int height)
        {
            graphic.Save();

            graphic.SetSourceRGB(_color.R, _color.G, _color.B);
            graphic.Rectangle(0, 0, width, height);
            graphic.Fill();
            graphic.Stroke();

            graphic.Restore();
        }
    }
}
