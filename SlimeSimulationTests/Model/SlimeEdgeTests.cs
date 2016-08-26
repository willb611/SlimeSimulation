using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Model.Tests
{
    [TestClass()]
    public class SlimeEdgeTests
    {
        [TestMethod()]
        public void SlimeEdgeIsDisconnected()
        {
            var a = new Node(1, 1, 1);
            var b = new Node(2, 2, 2);
            SlimeEdge.ShouldAllowDisconnection = true;
            var disconnectedAb = new SlimeEdge(a, b, 0);

            Assert.IsTrue(disconnectedAb.IsDisconnected());

            var connectedAb = new SlimeEdge(a, b, 0.00001);
            Assert.IsFalse(connectedAb.IsDisconnected());
        }
    }
}
