using Cairo;
using Gdk;
using Gtk;
using Rectangle = Gdk.Rectangle;

namespace SlimeSimulation.View.WindowComponent
{
    public class ColorArea : DrawingArea
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
                Rectangle allocation = Allocation;
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
