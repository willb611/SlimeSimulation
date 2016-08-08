using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Controller.SimulationUpdaters;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Controller.SimulationUpdaters.Tests
{
    [TestClass()]
    public class SlimeNetworkAdaptionCalculatorTests
    {
        [TestMethod()]
        public void FunctionOfFlowTest()
        {
            double feedbackParameter = 2;
            var calculator = new SlimeNetworkAdaptionCalculator(feedbackParameter);
            double expected = 4 / 5.0;
            double actual = calculator.FunctionOfFlow(2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetUpdatedFlowTest()
        {
            double feedbackParameter = 2;
            double connectivity = 0.5;
            Node a = new Node(1, 1, 1);
            Node b = new Node(2, 2, 2);
            Edge edge = new Edge(a, b, connectivity);

            double flow = 2;
            double expected = 0.8;
            var calculator = new SlimeNetworkAdaptionCalculator(feedbackParameter);
            double actual = calculator.NextConnectivityForEdge(edge, flow);

            Assert.AreEqual(expected, actual, 0.000001);
        }
    }
}
