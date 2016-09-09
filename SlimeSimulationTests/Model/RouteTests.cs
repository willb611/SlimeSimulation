using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SlimeSimulation.Model.Tests
{
    [TestClass()]
    public class RouteTests
    {
        [TestMethod()]
        public void EqualsTest()
        {
            var a  = new Node(1, 1, 1);
            var b = new Node(2, 2, 2);
            var c = new Node(3, 3, 3);

            var abRoute = new Route(a, b);
            var bcRoute = new Route(b, c);
            Assert.AreEqual(abRoute, abRoute);
            Assert.AreNotEqual(abRoute, bcRoute);

            Assert.AreEqual(abRoute.GetHashCode(), abRoute.GetHashCode());
            Assert.AreNotEqual(abRoute.GetHashCode(), bcRoute.GetHashCode());
        }
    }
}
