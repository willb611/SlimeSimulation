using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Model.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace SlimeSimulation.Model.Generation.Tests
{
    [TestClass()]
    public class EdgeConnectionShapeTests
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [TestMethod()]
        public void IndexInDescriptionArrayForValueTest()
        {
            string[] descriptions = EdgeConnectionShape.DescriptionsForEdgeConnectionTypes;
            for (int i = 0; i < descriptions.Length; i++)
            {
                Logger.Info("Testing for i: {0}, value: {1}", i, descriptions[i]);
                var valueOfDescription = EdgeConnectionShape.GetValueForDescription(descriptions[i]);
                Assert.AreEqual(i, EdgeConnectionShape.IndexInDescriptionArrayForValue(valueOfDescription));
            }
        }
    }
}
