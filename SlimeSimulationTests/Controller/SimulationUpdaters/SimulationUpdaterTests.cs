using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.Controller.SimulationUpdaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model.Generation;
using SlimeSimulation.Model.Simulation;

namespace SlimeSimulation.Controller.SimulationUpdaters.Tests
{
    [TestClass()]
    public class SimulationUpdaterTests
    {
        [TestMethod()]
        public void TaskUpdateNetworkUsingFlowInStateTest_ShouldUpdateNetwork()
        {
            var network = new LatticeGraphWithFoodSourcesGenerator().Generate();
            var slime = new SlimeNetworkGenerator().FromGraphWithFoodSources(network);
            var flowResult = new FlowCalculator().CalculateFlow(slime, slime.FoodSources.First(),
                slime.FoodSources.Last(), 1);
            var stateWithFlow = new SimulationState(slime, flowResult, network, 0, 0);

            var updatedNetwork = new SimulationUpdater().TaskUpdateNetworkUsingFlowInState(stateWithFlow);
            updatedNetwork.Wait(10000);
            var result = updatedNetwork.Result;
            Assert.IsNull(result.FlowResult, "Updating slime network with flow result should remove flow result");
            Assert.AreNotEqual(slime, result.SlimeNetwork,
                "Slime network should change after updating it from flow result");
        }

        [TestMethod()]
        public void TaskCalculateFlow_ShouldGetFlow()
        {
            var network = new LatticeGraphWithFoodSourcesGenerator().Generate();
            var slime = new SlimeNetworkGenerator().FromGraphWithFoodSources(network);
            var state = new SimulationState(slime, true, network);

            var updatedNetwork = new SimulationUpdater().TaskCalculateFlow(state);
            updatedNetwork.Wait(10000);
            var result = updatedNetwork.Result;
            Assert.IsNotNull(result.FlowResult, "Calculate flow task should return a state with a flow result");
            Assert.AreEqual(slime, result.SlimeNetwork,
                "Slime network should NOT change after updating it from flow result");
        }

        [TestMethod()]
        public void TaskCalculateFlowAndUpdateNetwork_ShouldUpdateNetwork()
        {
            var network = new LatticeGraphWithFoodSourcesGenerator().Generate();
            var slime = new SlimeNetworkGenerator().FromGraphWithFoodSources(network);
            var state = new SimulationState(slime, true, network);

            var updatedNetwork = new SimulationUpdater().TaskCalculateFlowAndUpdateNetwork(state);
            updatedNetwork.Wait(10000);
            var result = updatedNetwork.Result;
            Assert.IsNull(result.FlowResult, "Calculate flow task should return a state without a flow result");
            Assert.AreNotEqual(slime, result.SlimeNetwork,
                "Slime network should be different after updating it from flow result");
        }
    }
}
