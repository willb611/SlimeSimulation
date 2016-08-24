using SlimeSimulation.Model;
using System.Collections.Generic;
using NLog;
using System;

namespace SlimeSimulation.FlowCalculation
{
    public class FlowResult
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ISet<SlimeEdge> _edges;
        private readonly Node _source, _sink;
        private readonly int _flowAmount;
        private readonly FlowOnEdges _flowOnEdges;

        public FlowResult(ISet<SlimeEdge> edges, Node source, Node sink, int flowAmount,
          FlowOnEdges flowOnEdges)
        {
            this._source = source;
            this._sink = sink;
            this._flowAmount = flowAmount;
            this._edges = edges;
            this._flowOnEdges = flowOnEdges;
            Logger.Info("[constructor] Creating flowResult for flow: " + flowAmount + ", and numer of edges: " + edges.Count);
            Logger.Info("[constructor] And source {0}, and Sink {1}", source, sink);
        }

        internal double GetMaximumFlowOnEdge()
        {
            return _flowOnEdges.GetMaximumFlowOnAnyEdge();
        }

        internal ISet<SlimeEdge> Edges {
            get { return _edges; }
        }

        internal Node Source {
            get { return _source; }
        }

        internal void Validate()
        {
            var acceptedError = 0.00001;
            double sourceFlow = GetFlowOnNode(Source);
            Logger.Info("[ValidateFlowResult] flow on source {0}: {1}", Source, sourceFlow);
            double sinkFlow = GetFlowOnNode(Sink);
            Logger.Info("[ValidateFlowResult] flow on Sink {0}: {1}", Sink, sinkFlow);
            Logger.Info("[ValidateFlowResult] Original flow: {0}", FlowAmount);
            if (_flowAmount - Math.Abs(sourceFlow) < acceptedError && _flowAmount - Math.Abs(sinkFlow) < acceptedError)
            {
                Logger.Info("[ValidateFlowResult] VALID!");
            }
            else
            {
                Logger.Error("[ValidateFlowResult] INVALID!");
            }
        }

        internal Node Sink {
            get { return _sink; }
        }

        public int FlowAmount {
            get { return _flowAmount; }
        }

        public double FlowOnEdge(SlimeEdge slimeEdge)
        {
            return _flowOnEdges.GetFlowOnEdge(slimeEdge);
        }

        internal double GetFlowOnNode(Node node)
        {
            Graph graph = new Model.Graph(Edges);
            double sum = 0;
            foreach (SlimeEdge edge in graph.EdgesConnectedToNode(node))
            {
                var flow = FlowOnEdge(edge);
                if (edge.A == node)
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
