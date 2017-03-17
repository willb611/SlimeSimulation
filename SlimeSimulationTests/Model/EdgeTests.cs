using SlimeSimulation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;

namespace SlimeSimulation.Model.Tests
{
    [TestClass()]
    public class EdgeTests
    {
        [TestMethod()]
        public void Equals_WhenComparingSameObject_ShouldReturnTrue()
        {
            Node a = new Node(1, 2, 2);
            Assert.AreEqual(a, a);
        }

        [TestMethod()]
        public void Equals_WhenOrderIsFlipped()
        {
            Node a = new Node(1, 1, 1);
            Node b = new Node(2, 2, 2);
            var ab = new Edge(a, b);
            var ba = new Edge(b, a);
            Assert.AreEqual(ab, ba, "Node order shouldn't matter when checking if edges are equal. ab.compareTo(ba): "
                + ab.CompareTo(ba));
            Assert.AreEqual(ab.GetHashCode(), ba.GetHashCode(), "If two edges are equal, hashcode should be equal too");
        }

        [TestMethod()]
        public void CompareTo()
        {
            Node a = new Node(1, 1, 1);
            Node b = new Node(2, 2, 2);
            Node c = new Node(3, 3, 3);
            var ab = new Edge(a, b);
            var ac = new Edge(a, c);
            Assert.IsTrue(ab.CompareTo(ac) < 0, "if node b is less than node c, then ab should be less than bc");
        }

        [TestMethod()]
        public void Construction_Test()
        {
            Node a = new Node(1, 1, 1);
            Node b = new Node(2, 2, 2);
            Assert.IsTrue(a.CompareTo(b) < 0, "Assuming how Node's compareTo method work");
            var ab = new Edge(b, a);
            Assert.AreEqual(a, ab.A, "A should be set to smallest of the two nodes in the constructor");
        }
    }
}
