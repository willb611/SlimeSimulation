using System.Collections.Generic;
using System.Linq;
using NLog;
using SlimeSimulation.Model;
using SlimeSimulation.StdLibHelpers;

namespace SlimeSimulation.Algorithms.RouteSelection
{
    public class EnumerateBySourcesRouteSelector : IRouteSelector
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private List<FoodSourceNode> _foodSources;
        private IEnumerator<FoodSourceNode> _foodSourceEnumerator;

        public Route SelectRoute(SlimeNetwork network)
        {
            if (FoodSourcesHaveChanged(network))
            {
                UpdateEnumerator(network);
            }
            var source = SelectSource(network);
            var sink = SelectSink(network);
            int iterations = 0;
            while (network.InvalidSourceSink(source, sink))
            {
                source = SelectSource(network);
                sink = SelectSink(network);
                iterations++;
            }
            Logger.Info($"[SelectRoute] Took {iterations} attempts to find a valid source and sink combination");
            return new Route(source, sink);
        }

        private bool FoodSourcesHaveChanged(SlimeNetwork network)
        {
            if (_foodSources == null)
            {
                return true;
            }
            if (_foodSources.Count != network.FoodSources.Count)
            {
                Logger.Debug("[FoodSourcesHaveChanged] Food source count is different in network, returning false");
                return false;
            }
            var result = network.FoodSources.All(food => _foodSources.Contains(food));
            if (!result)
            {
                Logger.Debug("[FoodSourcesHaveChanged] Food source count is equal to network, but exact elements dont match so returning false");
            }
            return result;
        }

        private Node SelectSink(SlimeNetwork network)
        {
            return network.FoodSources.PickRandom();
        }

        private Node SelectSource(SlimeNetwork network)
        {
            return AdvanceAndGetFoodSourceEnumerator(network).Current;
        }
        private IEnumerator<FoodSourceNode> AdvanceAndGetFoodSourceEnumerator(SlimeNetwork network)
        {
            while (_foodSourceEnumerator == null || !_foodSourceEnumerator.MoveNext())
            {
                _foodSourceEnumerator?.Dispose();
                UpdateEnumerator(network);
                Logger.Debug("[AdvanceAndGetFoodSourceEnumerator] Entered method");
            }
            return _foodSourceEnumerator;
        }

        private void UpdateEnumerator(SlimeNetwork network)
        {
            _foodSources = network.FoodSources.GetEnumerator().AsList();
            _foodSourceEnumerator = _foodSources.GetEnumerator();
        }
    }
}
