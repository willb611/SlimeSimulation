using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model.Tests {
    [TestClass()]
    public class EdgeTests {
        [TestMethod()]
        public void Equals_WhenComparingSameObject_ShouldReturnTrue() {
            Node a = new Node(1, 2, 2);
            Assert.AreEqual(a, a);
        }
    }
}
