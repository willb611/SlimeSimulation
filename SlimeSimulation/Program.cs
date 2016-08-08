using SlimeSimulation.Controller;

namespace SlimeSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            var flowAmount = 3;
            var feedbackParameter = 1.8;
            var controller = new MainController(flowAmount, feedbackParameter);
            controller.RunSimulation();
        }
    }
}
