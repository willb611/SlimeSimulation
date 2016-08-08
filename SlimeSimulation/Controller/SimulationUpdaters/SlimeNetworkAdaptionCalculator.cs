using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Model;
using SlimeSimulation.FlowCalculation;

namespace SlimeSimulation.Controller.SimulationUpdaters
{
    public class SlimeNetworkAdaptionCalculator
    {
        private double feedbackParameter;

        public SlimeNetworkAdaptionCalculator(double feedbackParameter) {
            this.feedbackParameter = feedbackParameter;
        }

        internal SlimeNetwork CalculateNextStep(SlimeNetwork slimeNetwork, FlowResult flowResult)
        {
            ISet<Edge> edges = new HashSet<Edge>();
            foreach (Edge edge in slimeNetwork.Edges)
            {
                double connectivityInNextStepForEdge = NextConnectivityForEdge(edge, flowResult.FlowOnEdge(edge));
                Edge updatedEdge = new Edge(edge.A, edge.B, connectivityInNextStepForEdge);
                edges.Add(updatedEdge);
            }
            SlimeNetwork networkInNextStep = new SlimeNetwork(slimeNetwork.Nodes, slimeNetwork.FoodSources, edges);
            return networkInNextStep;
        }

        public double FunctionOfFlow(double flow)
        {
            double flowRaisedToSigma = Math.Pow(Math.Abs(flow), feedbackParameter);
            double functionResult = flowRaisedToSigma / (1 + flowRaisedToSigma);
            return functionResult;
        }

        internal double NextConnectivityForEdge(Edge edge, double flow)
        {
            double rateOfChangeOfConnectivity = FunctionOfFlow(flow) - edge.Connectivity;
            double updatedConnectivity = edge.Connectivity + rateOfChangeOfConnectivity;
            return updatedConnectivity;
        }
    }
}
