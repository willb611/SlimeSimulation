using System.Collections.Generic;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;
using SlimeSimulation.View;

namespace SlimeSimulation.Controller.WindowComponentController
{
    public abstract class NodeViewController
    {
        public abstract Rgb GetColourForNode(Node node);
        public abstract int GetSizeForNode(Node node);

        public const int NonFoodSourcePointSize = 3;
        public const int FoodSourcePointSize = 15;
    }

    public class FlowResultNodeViewController : NodeViewController
    {
        private readonly FlowResult _flowResult;

        public FlowResultNodeViewController(FlowResult result)
        {
            _flowResult = result;
        }

        public static Rgb SourceColour => Rgb.Blue;
        public static Rgb SinkColour => Rgb.Red;
        public static Rgb NormalNodeColour => Rgb.Black;

        public override Rgb GetColourForNode(Node node)
        {
            if (_flowResult.Source.Equals(node))
            {
                return SourceColour;
            }
            if (_flowResult.Sink.Equals(node))
            {
                return SinkColour;
            }
            return NormalNodeColour;
        }

        public override int GetSizeForNode(Node node)
        {
            if (_flowResult.Source.Equals(node) || _flowResult.Sink.Equals(node))
            {
                return FoodSourcePointSize;
            }
            return NonFoodSourcePointSize;
        }
    }

    public class SlimeNodeViewController : NodeViewController
    {
        private readonly ICollection<Node> _slimeCoveredNodes;

        public SlimeNodeViewController(ICollection<Node> slimeCoveredNodes)
        {
            _slimeCoveredNodes = slimeCoveredNodes;
        }

        public static Rgb SlimeNodeColour => Rgb.Yellow;
        public static Rgb NormalNodeColour => Rgb.Black;

        public override Rgb GetColourForNode(Node node)
        {
            if (_slimeCoveredNodes.Contains(node))
            {
                return SlimeNodeColour;
            }
            return NormalNodeColour;
        }

        public override int GetSizeForNode(Node node)
        {
            if (node.IsFoodSource())
            {
                return FoodSourcePointSize;
            }
            return NonFoodSourcePointSize;
        }
    }
}
