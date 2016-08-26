using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;

namespace SlimeSimulation.Controller.SimulationUpdaters
{
    public class SlimeNetworkAdaptionCalculator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly double _feedbackParameter;

        public SlimeNetworkAdaptionCalculator() : this(new SlimeNetworkAdaptionCalculatorConfig())
        {
        }
        public SlimeNetworkAdaptionCalculator(SlimeNetworkAdaptionCalculatorConfig config) {
            _feedbackParameter = config.FeedbackParam;
        }

        internal SlimeNetwork CalculateNextStep(SlimeNetwork slimeNetwork, FlowResult flowResult)
        {
            ISet<SlimeEdge> edges = new HashSet<SlimeEdge>();
            foreach (SlimeEdge edge in slimeNetwork.SlimeEdges)
            {
                double connectivityInNextStepForEdge = NextConnectivityForEdge(edge, flowResult.FlowOnEdge(edge));
                SlimeEdge updatedSlimeEdge = new SlimeEdge(edge.A, edge.B, connectivityInNextStepForEdge);
                edges.Add(updatedSlimeEdge);
            }
            var connectedEdges = RemoveDisconnectedEdges(edges);
            var connectedNodes = Edges.GetNodesContainedIn(connectedEdges);
            connectedNodes.UnionWith(slimeNetwork.FoodSources); // Food sources never disconnect
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
            double updatedConnectivity = slimeEdge.Connectivity + rateOfChangeOfConnectivity;
            return updatedConnectivity;
        }
        
        internal static HashSet<SlimeEdge> RemoveDisconnectedEdges(ISet<SlimeEdge> edges)
        {
            HashSet<SlimeEdge> connected = new HashSet<SlimeEdge>();
            foreach (var slimeEdge in edges)
            {
                if (!slimeEdge.IsDisconnected())
                {
                    connected.Add(slimeEdge);
                }
            }
            Logger.Debug("[RemoveDisconnectedEdges] Out of {0}, remaining: {1} ({2}%)", edges.Count, connected.Count, (connected.Count / (double)edges.Count) * 100);
            return connected;
        }
    }
}
