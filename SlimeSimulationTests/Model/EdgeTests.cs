using Microsoft.VisualStudio.TestTools.UnitTesting;

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
