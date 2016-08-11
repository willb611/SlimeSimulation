using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Configuration.Tests
{
    [TestClass()]
    public class LatticeSlimeNetworkGenerationConfigTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void NewInstance_SizeTwo_ShouldThrowException()
        {
            new LatticeSlimeNetworkGenerationConfig(2);
        }
    }
}
