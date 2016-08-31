using SlimeSimulation.Controller.WindowController;

namespace SlimeSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            var applicationStarter = new NewSimulationStarterWindowController();
            applicationStarter.Render();
        }
    }
}
