using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using NLog;
using SlimeSimulation.LinearEquations;
using System.Linq;
using System.Threading.Tasks;
using SlimeSimulation.Model.Simulation;
using SlimeSimulation.Controller.SimulationUpdaters;
using SlimeSimulation.View;

namespace SlimeSimulation.Controller
{
    public class MainController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly int flowAmount;
        private SlimeNetworkGenerator slimeNetworkGenerator = new LatticeSlimeNetworkGenerator(13, 0.05);
        private SimulationUpdater simulationUpdater;

        private MainView mainView;
        private bool simulationDoingStep = false;
        private SimulationState state;

        public MainController(int flowAmount, SimulationUpdater simulationUpdater)
        {
            this.flowAmount = flowAmount;
            this.simulationUpdater = simulationUpdater;
        }

        public void RunSimulation()
        {
            try
            {
                var slimeNetwork = slimeNetworkGenerator.Generate();
                state = new SimulationState(slimeNetwork);
                using (mainView = new MainView())
                {
                    logger.Debug("[RunSimulation] Using before ");
                    UpdateDisplayFromState(state);
                    logger.Debug("[RunSimulation] Using after ");
                }
                logger.Info("[RunSimulation] Finished!");
            }
            catch (Exception e)
            {
				logger.Fatal(e, "[RunSimulation] Unexpected exception: " + e, e.Data);
            }
        }

        public void DoNextSimulationStep()
        {
            if (simulationDoingStep)
            {
                logger.Debug("[DoNextSimulationStep] Not starting next step as it's already in progress");
            }
            else
            {
                logger.Debug("[DoNextSimulationStep] Stepping");
                simulationDoingStep = true;
                var nextState = simulationUpdater.GetNextState(state, flowAmount);
                UpdateControllerState(nextState);
            }
        }

        async private void UpdateControllerState(Task<SimulationState> stateParam)
        {
            try
            {
                state = await stateParam;
                logger.Debug("[UpdateControllerState] State: {0} about to update view", state);
                Gtk.Application.Invoke(delegate
                {
                    logger.Debug("[UpdateControllerState] Invoking from main thread ");
                    UpdateDisplayFromState(state);
                });
                simulationDoingStep = false;
            }
            catch (Exception e)
            {
                logger.Error(e, "[UpdateControllerState] Error: ");
            }
        }

        private void UpdateDisplayFromState(SimulationState state)
        {
            if (state.FlowResult == null)
            {
                DisplayConnectivityInNetwork(state.SlimeNetwork);
            }
            else
            {
                DisplayFlowResult(state.FlowResult);
            }
        }

        private void DisplayFlowResult(FlowResult flowResult)
        {
            new FlowResultController(this, mainView, flowResult).Render();
        }

        private void DisplayConnectivityInNetwork(SlimeNetwork network)
        {
            new FlowNetworkGraphController(this, mainView, network.Edges).Render();
        }
    }
}
