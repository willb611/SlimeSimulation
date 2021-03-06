using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using SlimeSimulation.Algorithms.LinearEquations;
using SlimeSimulation.Model;

namespace SlimeSimulation.Algorithms.FlowCalculation
{
    public class FlowCalculator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ILinearEquationSolver _linearEquationSolver;

        public FlowCalculator() : this(new LupDecompositionSolver())
        {
            
        }
        public FlowCalculator(ILinearEquationSolver solver)
        {
            _linearEquationSolver = solver;
        }

        public FlowResult CalculateFlow(SlimeNetwork network, Route route, double flowAmount)
        {
# if DEBUG
            var attemptedRoute = $"route attempted to take was: {route}";
            Logger.Debug(attemptedRoute);
#endif 
            List<Node> nodeList = new List<Node>(network.AllNodesConnectedTo(route.Source));
            EnsureSourceSinkInCorrectPositions(nodeList, route.Source, route.Sink);
            double[][] a = GetSystemOfEquations(network, nodeList);
            double[] b = GetMatrixOfFlowGainedAtNodeFromZeroToN(flowAmount, nodeList.Count() - 1);
            try
            {
                double[] solution = _linearEquationSolver.FindX(a, b);
                Pressures pressures = new Pressures(solution, nodeList);
                FlowOnEdges flowOnEdges = GetFlowOnEdges(network, pressures, nodeList);
                return new FlowResult(network, route, flowAmount, flowOnEdges);
            }
            catch (SingularMatrixException e)
            {
                var errorNode = nodeList[LupDecompositionSolver.ErrorColumnNumber];
                var edgesConnectedToBadNode = LogHelper.CollectionToString(new List<Edge>(network.EdgesConnectedToNode(errorNode)));
                var error =
                    $"Error column was {LupDecompositionSolver.ErrorColumnNumber}, meaning error node was {errorNode}. Connected edges: {edgesConnectedToBadNode}.";
                Logger.Error(error, e);
                return null;
            }
        }

        public void EnsureSourceSinkInCorrectPositions(List<Node> nodeList, Node source, Node sink)
        {
            if (source == sink)
            {
                throw new ArgumentException(String.Format("Source ({0}) matches sink ({1})", source, sink));
            }
            Swap(nodeList, 0, nodeList.IndexOf(source));
            Swap(nodeList, nodeList.Count - 1, nodeList.IndexOf(sink));
        }

        public void Swap(List<Node> nodeList, int a, int b)
        {
            if (a != b)
            {
                Node tmp = nodeList[a];
                nodeList[a] = nodeList[b];
                nodeList[b] = tmp;
            }
        }

        private FlowOnEdges GetFlowOnEdges(SlimeNetwork network, Pressures pressures, List<Node> nodes)
        {
            FlowOnEdges result = new FlowOnEdges((network as Graph).EdgesInGraph);
            foreach (SlimeEdge edge in network.SlimeEdges)
            {
                if (!nodes.Contains(edge.A) || !nodes.Contains(edge.B)) continue;
                double pi = pressures.PressureAt(edge.A);
                double pj = pressures.PressureAt(edge.B);
                double flow = edge.Connectivity*(pi - pj);
                result.IncreaseFlowOnEdgeBy(edge, flow);
                Logger.Trace("For edge {0}, got pi {1}, pj {2}, and flow {3}", edge, pi, pj, flow);
            }
            return result;
        }

        private double[] GetMatrixOfFlowGainedAtNodeFromZeroToN(double flowAmount, int n)
        {
            double[] arr = new double[n];
            arr[0] = flowAmount;
            if (Logger.IsTraceEnabled)
            {
                Logger.Trace("[GetMatrixOfFlowGainedAtNodeFromZeroToN] Printing ");
                Logger.Trace(LogHelper.PrintArr(arr));
            }
            return arr;
        }

        private double[][] GetSystemOfEquations(SlimeNetwork network, List<Node> nodes)
        {
            int upperlimit = nodes.Count() - 1;
            double[][] equations = new double[upperlimit][];
            for (int row = 0; row < upperlimit; row++)
            {
                equations[row] = new double[upperlimit];
                for (int col = 0; col < upperlimit; col++)
                {
                    equations[row][col] = GetValueForSystemOfEquations(network, nodes[row], nodes[col]);
                }
            }
            if (Logger.IsTraceEnabled)
            {
                Logger.Trace("[GetSystemOfEquations] Printing return value ");
                Logger.Trace(LogHelper.PrintArr(equations));
            }
            return equations;
        }

        private double GetValueForSystemOfEquations(SlimeNetwork graph, Node a, Node b)
        {
            double value = 0;
            if (a == b)
            {
                foreach (var edge in graph.EdgesConnectedToNode(a))
                {
                    var connected = (SlimeEdge) edge;
                    value += connected.Connectivity;
                }
            }
            else
            {
                value = -graph.GetEdgeConnectivityOrZero(a, b);
            }
            return value;
        }
    }
}
