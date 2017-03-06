using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Algorithms.FlowCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;

namespace SlimeSimulation.Algorithms.FlowCalculation.Tests
{
    [TestClass()]
    public class FlowOnEdgesTests
    {
        [TestMethod()]
        public void FlowOnEdges_ChangeFlowOnEdgeChangesIt()
        {
            var a = new Node(1,1,1);
            var b = new Node(2,2,2);
            var edge = new SlimeEdge(a, b, 0);
            
            var fe = new FlowOnEdges(new List<Edge>() {edge});
            Assert.AreEqual(0, fe.GetFlowOnEdge(edge));

            var delta = 0.01;
            fe.IncreaseFlowOnEdgeBy(edge, delta);
            Assert.AreEqual(delta, fe.GetFlowOnEdge(edge));
        }
    }
}