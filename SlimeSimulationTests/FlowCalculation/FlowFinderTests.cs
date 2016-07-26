using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.FlowCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;

namespace SlimeSimulation.FlowCalculation.Tests {
    [TestClass()]
    public class FlowFinderTests {
        [TestMethod()]
        public void GetLoopsWithDirectionForFlow_WhenSimpleLoop_ShouldWork() {
            /*
            List<Loop> loops = new List<Loop>();
            List<Edge> asideEdge = new List<Edge>();
            asideEdge.Add(srca);
            asideEdge.Add(asink);
            List<Edge> bsideEdge = new List<Edge>();
            bsideEdge.Add();
            LoopWithDirectionOfFlow bSideLoop = new LoopWithDirectionOfFlow(nodes, edges, )
            
             * b      -  sink
             * |        |
             * source -  a
             

            */
            Assert.Fail();
        }

        [TestMethod()]
        public void EstimateFlowForEdgesTest() {
            double ACCEPTED_ERROR = 0.01;
            var calculator = new FlowCalculator();
            double flowAmount = 2;
            List<Edge> edges = new List<Edge>();
            List<Node> nodes = new List<Node>();
            Node source = new Node(1, 1, 1);
            Node a = new Node(2, 1, 2);
            Edge srca = new Edge(source, a, 2);
            Node b = new Node(3, 2, 1);
            Edge srcb = new Edge(source, b, 2);
            Node sink = new Node(4, 2, 2);
            Edge asink = new Edge(a, sink, 2);
            Edge bsink = new Edge(b, sink, 2);
            edges.Add(srca);
            edges.Add(srcb);
            edges.Add(bsink);
            edges.Add(asink);
            nodes.Add(a);
            nodes.Add(b);
            nodes.Add(source);
            nodes.Add(sink);
            /*
             * b      -  sink
             * |        |
             * source -  a
             */
            FlowFinder flowFinder = new FlowFinder();
            FlowOnEdges flowOnEdges = flowFinder.EstimateFlowForEdges(edges, source, sink, (int) flowAmount);

            double flowASrc = flowOnEdges.GetFlowOnEdge(srca);
            double flowBSrc = flowOnEdges.GetFlowOnEdge(srcb);
            Assert.IsTrue(flowAmount - (flowASrc + flowBSrc) < ACCEPTED_ERROR);

            double flowASink = flowOnEdges.GetFlowOnEdge(asink);
            double flowBSink = flowOnEdges.GetFlowOnEdge(bsink);
            Assert.IsTrue(flowAmount - (flowASink + flowBSink) < ACCEPTED_ERROR);
        }
    }
}
