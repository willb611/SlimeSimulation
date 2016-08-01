using SlimeSimulation.Controller;

namespace SlimeSimulation {
    class Program {
        static void Main(string[] args) {
            var controller = new MainController();
            controller.RunSimulation();
        }
    }
}
