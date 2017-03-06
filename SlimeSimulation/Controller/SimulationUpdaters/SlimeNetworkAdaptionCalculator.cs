using System;
using System.Collections.Generic;
using NLog;
using SlimeSimulation.Algorithms.FlowCalculation;
using SlimeSimulation.Configuration;
using SlimeSimulation.Model;

namespace SlimeSimulation.Controller.SimulationUpdaters
{
    public class SlimeNetworkAdaptionCalculator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly double _feedbackParameter;
        private readonly double _timePerSimulationStep;
        private readonly bool _shouldAllowDisconnection;
        public double FeedbackUsedWhenUpdatingNetwork => _feedbackParameter;
        
        private static readonly bool DefaultShouldAllowDisconnection = new SimulationConfiguration().ShouldAllowDisconnection;

        public SlimeNetworkAdaptionCalculator() : this(new SlimeNetworkAdaptionCalculatorConfig())
        {
        }
        public SlimeNetworkAdaptionCalculator(SlimeNetworkAdaptionCalculatorConfig config)
            : this(config, DefaultShouldAllowDisconnection)
        {
        }
        public SlimeNetworkAdaptionCalculator(SlimeNetworkAdaptionCalculatorConfig config, bool shouldAllowDisconnection) {
            _feedbackParameter = config.FeedbackParam;
            _timePerSimulationStep = config.TimePerSimulationStep;
            _shouldAllowDisconnection    = shouldAllowDisconnection;
        }

        internal SlimeNetwork CalculateNextStep(SlimeNetwork slimeNetwork, FlowResult flowResult)
        {
            if (slimeNetwork == null)
            {
                throw new ArgumentNullException(nameof(slimeNetwork));
            } else if (flowResult == null)
            {
                throw new ArgumentNullException(nameof(flowResult));
            }
            ISet<SlimeEdge> edges = new HashSet<SlimeEdge>();
            foreach (SlimeEdge edge in slimeNetwork.SlimeEdges)
            {
                double connectivityInNextStepForEdge = NextConnectivityForEdge(edge, flowResult.FlowOnEdge(edge));
                SlimeEdge updatedSlimeEdge = new SlimeEdge(edge.A, edge.B, connectivityInNextStepForEdge);
                edges.Add(updatedSlimeEdge);
            }
            var connectedEdges = RemoveDisconnectedEdges(edges);
            var connectedNodes = Edges.GetNodesContainedIn(connectedEdges);
            // Food sources never disconnect. Otherwise slime *might* not be able to grow at the start from a single food source.
            // Also weird errors occur if food sources are allowed to disconnect, not sure why.
            connectedNodes.UnionWith(slimeNetwork.FoodSources);
            return new SlimeNetwork(connectedNodes, slimeNetwork.FoodSources,
                connectedEdges);
        }
        
        internal SlimeNetwork CalculateNextStep(SlimeNetwork slimeNetwork, List<FlowResult> flowResults)
        {
            ISet<SlimeEdge> edges = new HashSet<SlimeEdge>();
            foreach (SlimeEdge edge in slimeNetwork.SlimeEdges)
            {
                double sumOfNextConnectivityForEdge = 0;
                foreach (var flowResult in flowResults)
                {
                    sumOfNextConnectivityForEdge += NextConnectivityForEdge(edge, flowResult.FlowOnEdge(edge));
                }
                var connectivityInNextStepForEdge = sumOfNextConnectivityForEdge/flowResults.Count;
                SlimeEdge updatedSlimeEdge = new SlimeEdge(edge.A, edge.B, connectivityInNextStepForEdge);
                edges.Add(updatedSlimeEdge);
            }
            var connectedEdges = RemoveDisconnectedEdges(edges);
            var connectedNodes = Edges.GetNodesContainedIn(connectedEdges);
            // Food sources never disconnect. Otherwise slime *might* not be able to grow at the start from a single food source.
            // Also weird errors occur if food sources are allowed to disconnect, not sure why.
            connectedNodes.UnionWith(slimeNetwork.FoodSources);
            return new SlimeNetwork(connectedNodes, slimeNetwork.FoodSources,
                connectedEdges);
        }

        public double FunctionOfFlow(double flow)
        {
            double flowRaisedToSigma = Math.Pow(Math.Abs(flow), _feedbackParameter);
            double functionResult = flowRaisedToSigma / (1 + flowRaisedToSigma);
            return functionResult;
        }

        internal double NextConnectivityForEdge(SlimeEdge slimeEdge, double flow)
        {
            double rateOfChangeOfConnectivity = FunctionOfFlow(flow) - slimeEdge.Connectivity;
            double updatedConnectivity = slimeEdge.Connectivity + (rateOfChangeOfConnectivity * _timePerSimulationStep);
            return Math.Abs(updatedConnectivity);
        }
        
        internal HashSet<SlimeEdge> RemoveDisconnectedEdges(ISet<SlimeEdge> edges)
        {
            if (!_shouldAllowDisconnection)
            {
                return new HashSet<SlimeEdge>(edges);
            }
            HashSet<SlimeEdge> connected = new HashSet<SlimeEdge>();
            foreach (var slimeEdge in edges)
            {
                if (!slimeEdge.IsDisconnected())
                {
                    connected.Add(slimeEdge);
                }
            }
            Logger.Debug("[RemoveDisconnectedEdges] Out of {0}, remaining: {1} ({2}%)", 
                            edges.Count, connected.Count, (connected.Count / (double)edges.Count) * 100);
            return connected;
        }


    }
}
