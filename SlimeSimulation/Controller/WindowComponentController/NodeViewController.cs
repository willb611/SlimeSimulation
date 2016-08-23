using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;
using SlimeSimulation.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Controller.WindowsComponentController
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
            else if (_flowResult.Sink.Equals(node))
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
            if (_flowResult.Source.Equals(node) || _flowResult.Sink.Equals(node))
            {
                return FoodSourcePointSize;
            }
            else
            {
                return NonFoodSourcePointSize;
            }
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
            else
            {
                return NormalNodeColour;
            }
        }

        public override int GetSizeForNode(Node node)
        {
            if (node.IsFoodSource())
            {
                return FoodSourcePointSize;
            }
            else
            {
                return NonFoodSourcePointSize;
            }
        }
    }
}
