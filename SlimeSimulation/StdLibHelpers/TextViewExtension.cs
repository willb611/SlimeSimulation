using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.StdLibHelpers
{
    public static class TextViewExtension
    {
        public static double? ExtractDoubleFromView(this TextView source)
        {
            double result;
            var success = double.TryParse(source.Buffer.Text, out result);
            if (success)
            {
                return result;
            }
            return null;
        }

        public static int? ExtractIntFromView(this TextView source)
        {
            int result;
            var success = int.TryParse(source.Buffer.Text, out result);
            if (success)
            {
                return result;
            }
            return null;
        }
    }
}
