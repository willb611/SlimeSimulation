using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.View.WindowComponent.SimulationCreationComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.View.WindowComponent.SimulationCreationComponent.Tests
{
    [TestClass()]
    public class FileToLoadFromInputComponentTests
    {
        [TestMethod()]
        public void FileToLoadFromInputComponentTest()
        {
            string text = "example words";
            FileToLoadFromInputComponent fileToLoadFromInputComponent = new FileToLoadFromInputComponent(text);

            Assert.AreEqual(text, fileToLoadFromInputComponent.ReadInput());
        }
    }
}
