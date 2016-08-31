using Gtk;
using SlimeSimulation.Controller.WindowComponentController;

namespace SlimeSimulation.View.WindowComponent
{
    public class SlimeWindowNodeHighlightKey : VBox
    {

        public SlimeWindowNodeHighlightKey() : base(true, 10)
        {
            Add(new Label("Node colour key"));
            
            var sinkPart = new HBox(true, 10);
            sinkPart.Add(new Label("Sink"));
            sinkPart.Add(new ColorArea(FlowResultNodeViewController.SinkColour));

            var normalPart = new HBox(true, 10);
            normalPart.Add(new Label("Normal node"));
            normalPart.Add(new ColorArea(FlowResultNodeViewController.NormalNodeColour));

            Add(SlimeColourKey());
            Add(NonSlimeColourKey());
        }

        private VBox NonSlimeColourKey()
        {
            var keyFoodSourceComponent = new HBox(true, 10);
            keyFoodSourceComponent.Add(new Label("Normal food source"));
            keyFoodSourceComponent.Add(new ColorArea(SlimeNodeViewController.FoodSourceColour));

            var keyNormalNodeComponent = new HBox(true, 10);
            keyNormalNodeComponent.Add(new Label("Normal node"));
            keyNormalNodeComponent.Add(new ColorArea(SlimeNodeViewController.NormalNodeColour));

            return new VBox(false, 10) { keyFoodSourceComponent, keyNormalNodeComponent };
        }

        private VBox SlimeColourKey()
        {
            var slimeKeyFoodSourceComponent = new HBox(true, 10);
            slimeKeyFoodSourceComponent.Add(new Label("Slime covered food source"));
            slimeKeyFoodSourceComponent.Add(new ColorArea(SlimeNodeViewController.SlimeFoodSourceColour));
            
            var slimeKeyNormalNodeComponent = new HBox(true, 10);
            slimeKeyNormalNodeComponent.Add(new Label("Slime covered node"));
            slimeKeyNormalNodeComponent.Add(new ColorArea(SlimeNodeViewController.SlimeNodeColour));

            return new VBox(false, 10) {slimeKeyFoodSourceComponent, slimeKeyNormalNodeComponent};
        }
    }
}
