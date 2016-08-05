using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Controller
{
  public abstract class NodeViewController
  {
    public abstract RGB GetColourForNode(Node node);
    public abstract int GetSizeForNode(Node node);

    public const int NON_FOOD_SOURCE_POINT_SIZE = 3;
    public const int FOOD_SOURCE_POINT_SIZE = 15;
  }

  public class FlowResultNodeViewController : NodeViewController
  {
    private FlowResult flowResult;

    public FlowResultNodeViewController(FlowResult result)
    {
      flowResult = result;
    }

    public static RGB SourceColour
    {
      get { return RGB.BLUE; }
    }

    public static RGB SinkColour
    {
      get { return RGB.RED; }
    }

    public static RGB NormalNodeColour
    {
      get { return RGB.BLACK; }
    }

    public override RGB GetColourForNode(Node node)
    {
      if (flowResult.Source.Equals(node))
      {
        return SourceColour;
      }
      else if (flowResult.Sink.Equals(node))
      {
        return SinkColour;
      }
      else
      {
        return NormalNodeColour;
      }
    }

    public override int GetSizeForNode(Node node)
    {
      if (flowResult.Source.Equals(node) || flowResult.Sink.Equals(node))
      {
        return FOOD_SOURCE_POINT_SIZE;
      }
      else
      {
        return NON_FOOD_SOURCE_POINT_SIZE;
      }
    }
  }

  public class ConnectivityNodeViewController : NodeViewController
  {
    public static RGB FoodColour
    {
      get { return RGB.BLUE; }
    }

    public static RGB NormalNodeColour
    {
      get { return RGB.BLACK; }
    }

    public override RGB GetColourForNode(Node node)
    {
      if (node.IsFoodSource())
      {
        return FoodColour;
      }
      else
      {
        return NormalNodeColour;
      }
    }

    public override int GetSizeForNode(Node node)
    {
      if (node.IsFoodSource())
      {
        return FOOD_SOURCE_POINT_SIZE;
      }
      else
      {
        return NON_FOOD_SOURCE_POINT_SIZE;
      }
    }
  }
}