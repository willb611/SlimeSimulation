using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model.Tests {
    [TestClass()]
    public class LatticeSlimeNetworkGeneratorTests {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Generate_SizeTwo_ShouldThrowException() {
            var generator = new LatticeSlimeNetworkGenerator();
            generator.Generate(2);
        }

        [TestMethod()]
        public void Generate_SizeThree_ShouldWork() {
            var generator = new LatticeSlimeNetworkGenerator();
            for (int i = 3; i < 9; i++) {
                var network = generator.Generate(3);
                Assert.IsTrue(network.FoodSources.Count >= 2, "2 food sources should always be produced");
            }
        }
    }
}
