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

        public static Rgb SourceColour {
            get { return Rgb.Blue; }
        }

        public static Rgb SinkColour {
            get { return Rgb.Red; }
        }

        public static Rgb NormalNodeColour {
            get { return Rgb.Black; }
        }

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

    public class ConnectivityNodeViewController : NodeViewController
    {
        public static Rgb FoodColour {
            get { return Rgb.Blue; }
        }

        public static Rgb NormalNodeColour {
            get { return Rgb.Black; }
        }

        public override Rgb GetColourForNode(Node node)
        {
            if (node.IsFoodSource())
            {
                return FoodColour;
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
