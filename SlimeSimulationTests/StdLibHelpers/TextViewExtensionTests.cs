using Gtk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.StdLibHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.StdLibHelpers.Tests
{
    [TestClass()]
    public class TextViewExtensionTests
    {
        [TestMethod()]
        public void ExtractDoubleFromViewTest()
        {
            var expected = 15.2;
            var textView = new TextView();
            textView.Buffer.Text = expected.ToString();

            var actual = textView.ExtractDoubleFromView();
            Assert.AreEqual(expected, actual);

            textView.Buffer.Text = "1234fsdfdsdf";
            Assert.IsNull(textView.ExtractDoubleFromView());
        }
    }
}