using SlimeSimulation.Model;
using System.Collections.Generic;
using NLog;
using System;

namespace SlimeSimulation.FlowCalculation
{
  public class FlowResult
  {
    private static Logger logger = LogManager.GetCurrentClassLogger();

    private readonly ISet<Edge> edges;
    private readonly Node source, sink;
    private readonly int flowAmount;
    private readonly FlowOnEdges flowOnEdges;

    public FlowResult(ISet<Edge> edges, Node source, Node sink, int flowAmount,
      FlowOnEdges flowOnEdges)
    {
      this.source = source;
      this.sink = sink;
      this.flowAmount = flowAmount;
      this.edges = edges;
      this.flowOnEdges = flowOnEdges;
      logger.Info("[constructor] Creating flowResult for flow: " + flowAmount + ", and numer of edges: " + edges.Count);
      logger.Info("[constructor] And source {0}, and Sink {1}", source, sink);
    }

    internal double GetMaximumFlowOnEdge()
    {
      return flowOnEdges.GetMaximumFlowOnAnyEdge();
    }

    internal ISet<Edge> Edges
    {
      get { return edges; }
    }

    internal Node Source
    {
      get { return source; }
    }

    internal void Validate()
    {
      var acceptedError = 0.00001;
      double sourceFlow = GetFlowOnNode(Source);
      logger.Info("[ValidateFlowResult] flow on source {0}: {1}", Source, sourceFlow);
      double sinkFlow = GetFlowOnNode(Sink);
      logger.Info("[ValidateFlowResult] flow on Sink {0}: {1}", Sink, sinkFlow);
      logger.Info("[ValidateFlowResult] Original flow: {0}", FlowAmount);
      if (flowAmount - Math.Abs(sourceFlow) < acceptedError && flowAmount - Math.Abs(sinkFlow) < acceptedError)
      {
        logger.Info("[ValidateFlowResult] VALID!");
      }
      else
      {
        logger.Error("[ValidateFlowResult] INVALID!");
      }
    }

    internal Node Sink
    {
      get { return sink; }
    }

    public int FlowAmount
    {
      get { return flowAmount; }
    }

    public double FlowOnEdge(Edge edge)
    {
      return flowOnEdges.GetFlowOnEdge(edge);
    }

    internal double GetFlowOnNode(Node node)
    {
      Graph graph = new Model.Graph(Edges);
      double sum = 0;
      foreach (Edge edge in graph.EdgesConnectedToNode(node))
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
      logger.Trace("Flow into node {0} is: {1}", node, sum);
      return sum;
    }
  }
}