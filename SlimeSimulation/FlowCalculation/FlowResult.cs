using System;
using System.Collections.Generic;
using NLog;
using SlimeSimulation.Model;

namespace SlimeSimulation.FlowCalculation
{
    public class FlowResult
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly SlimeNetwork _network;
        private readonly Route _route;
        private readonly double _flowAmount;
        private readonly FlowOnEdges _flowOnEdges;

        internal Node Source => _route.Source;
        internal Node Sink => _route.Sink;
        public double FlowAmount => _flowAmount;
        internal ISet<Edge> Edges => (_network as Graph).EdgesInGraph;

        public FlowResult(SlimeNetwork network, Route route, double flowAmount,
          FlowOnEdges flowOnEdges)
        {
            _route = route;
            _flowAmount = flowAmount;
            _network = network;
            _flowOnEdges = flowOnEdges;
            Logger.Info("[constructor] Creating flowResult for flow: {0}, and numer of edges: {1} and route {2}", flowAmount, network.SlimeEdges.Count, route);
#if DEBUG
            Validate();
#endif
        }

        internal double GetMaximumFlowOnEdge()
        {
            return _flowOnEdges.GetMaximumFlowOnAnyEdge();
        }

        internal void Validate()
        {
            var acceptedError = 0.00001;
            double sourceFlow = GetFlowOnNode(Source);
            Logger.Info("[Validate] flow on source {0}: {1}", Source, sourceFlow);
            double sinkFlow = GetFlowOnNode(Sink);
            Logger.Info("[Validate] flow on Sink {0}: {1}", Sink, sinkFlow);
            Logger.Info("[Validate] Original flow: {0}", FlowAmount);
            if (_flowAmount - Math.Abs(sourceFlow) < acceptedError && _flowAmount - Math.Abs(sinkFlow) < acceptedError)
            {
                Logger.Info("[Validate] VALID!");
            }
            else
            {
                Logger.Error("[Validate] INVALID!");
            }
        }

        public double FlowOnEdge(Edge slimeEdge)
        {
            return _flowOnEdges.GetFlowOnEdge(slimeEdge);
        }

        internal double GetFlowOnNode(Node node)
        {
            double sum = 0;
            foreach (var edge in _network.EdgesConnectedToNode(node))
            {
                var slimeEdge = (SlimeEdge) edge;
                var flow = FlowOnEdge(slimeEdge);
                if (Equals(slimeEdge.A, node))
                {
                    sum += flow;
                }
                else
                {
                    sum -= flow;
                }
            }
            Logger.Trace("Flow into node {0} is: {1}", node, sum);
            return sum;
        }
    }
}
