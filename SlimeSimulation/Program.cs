using SlimeSimulation.View;

namespace SlimeSimulation {
    class Program {
        static void Main(string[] args) {
            var flowAmount = 3;
            var controller = new MainController(flowAmount);
            controller.RunSimulation();
        }
    }
}
