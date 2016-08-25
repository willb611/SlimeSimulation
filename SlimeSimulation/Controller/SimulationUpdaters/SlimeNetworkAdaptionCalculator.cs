using System;
using System.Collections.Generic;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;

namespace SlimeSimulation.Controller.SimulationUpdaters
{
    public class SlimeNetworkAdaptionCalculator
    {
        private readonly double _feedbackParameter;

        public SlimeNetworkAdaptionCalculator() : this(0.8)
        {
            
        }

        public SlimeNetworkAdaptionCalculator(double feedbackParameter) {
            _feedbackParameter = feedbackParameter;
        }

        internal SlimeNetwork CalculateNextStep(SlimeNetwork slimeNetwork, FlowResult flowResult)
        {
            ISet<SlimeEdge> edges = new HashSet<SlimeEdge>();
            foreach (SlimeEdge edge in slimeNetwork.Edges)
            {
                double connectivityInNextStepForEdge = NextConnectivityForEdge(edge, flowResult.FlowOnEdge(edge));
                SlimeEdge updatedSlimeEdge = new SlimeEdge(edge.A, edge.B, connectivityInNextStepForEdge);
                edges.Add(updatedSlimeEdge);
            }
            SlimeNetwork networkInNextStep = new SlimeNetwork(slimeNetwork.Nodes, slimeNetwork.FoodSources, edges);
            return networkInNextStep;
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
    }
}
