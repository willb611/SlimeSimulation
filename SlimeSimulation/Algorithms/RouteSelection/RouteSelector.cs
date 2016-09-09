using SlimeSimulation.Model;

namespace SlimeSimulation.Algorithms.RouteSelection
{
    public interface IRouteSelector
    {
        Route SelectRoute(SlimeNetwork slimeNetwork);
    }
}
