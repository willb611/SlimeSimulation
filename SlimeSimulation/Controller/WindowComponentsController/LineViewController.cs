using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;

namespace SlimeSimulation.Controller
{
  public abstract class LineViewController
  {
    public abstract double GetLineWeightForEdge(Edge edge);
    public abstract double GetMaximumLineWeight();
  }

  internal class FlowResultLineViewController : LineViewController
  {
    private FlowResult flowResult;
    private readonly double maxLineWidth;

    public FlowResultLineViewController(FlowResult flowResult)
    {
      this.flowResult = flowResult;
      maxLineWidth = flowResult.GetMaximumFlowOnEdge();
    }

    public override double GetLineWeightForEdge(Edge edge)
    {
      return Math.Abs(flowResult.FlowOnEdge(edge));
    }

    public override double GetMaximumLineWeight()
    {
      return maxLineWidth;
    }
  }

  internal class ConnectivityLineViewController : LineViewController
  {
    private List<Edge> edges;
    private readonly double max;

    public ConnectivityLineViewController(List<Edge> edges)
    {
      this.edges = edges;
      var max = 0.0;
      foreach (Edge edge in edges)
      {
        max = Math.Max(edge.Connectivity, max);
      }
      this.max = max;
    }

    public override double GetLineWeightForEdge(Edge edge)
    {
      return edge.Connectivity;
    }

    public override double GetMaximumLineWeight()
    {
      return max;
    }
  }
}