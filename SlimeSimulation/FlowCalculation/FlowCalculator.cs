using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using SlimeSimulation.Model;
using SlimeSimulation.LinearEquations;

namespace SlimeSimulation.FlowCalculation
{
    public class FlowCalculator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private LinearEquationSolver linearEquationSolver;

        public FlowCalculator(LinearEquationSolver solver)
        {
            linearEquationSolver = solver;
        }

        public FlowResult CalculateFlow(ISet<Edge> edges, ISet<Node> nodes, Node source, Node sink, int flowAmount)
        {
            Graph graph = new Graph(edges, nodes);
            List<Node> nodeList = new List<Node>(nodes);
            EnsureSourceSinkInCorrectPositions(nodeList, source, sink);
            double[][] A = GetSystemOfEquations(graph, nodeList);
            double[] B = GetMatrixOfFlowGainedAtNodeFromZeroToN(flowAmount, nodes.Count - 1);
            double[] solution = linearEquationSolver.FindX(A, B);
            Pressures pressures = new Pressures(solution, nodeList);
            FlowOnEdges flowOnEdges = GetFlowOnEdges(graph, pressures, nodeList);
            return new FlowResult(edges, source, sink, flowAmount, flowOnEdges);
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

        private FlowOnEdges GetFlowOnEdges(Graph graph, Pressures pressures, List<Node> nodes)
        {
            FlowOnEdges result = new FlowOnEdges(graph.Edges);
            foreach (Edge edge in graph.Edges)
            {
                double pi = pressures.PressureAt(edge.A);
                double pj = pressures.PressureAt(edge.B);
                double flow = edge.Connectivity * (pi - pj);
                result.IncreaseFlowOnEdgeBy(edge, flow);
                logger.Trace("For edge {0}, got pi {1}, pj {2}, and flow {3}", edge, pi, pj, flow);
            }
            return result;
        }

        private double GetFlowForEdge(double connectivityOnEdge, int i, int j, Pressures pressures)
        {
            return connectivityOnEdge * (pressures[i] - pressures[j]);
        }

        private double[] GetMatrixOfFlowGainedAtNodeFromZeroToN(int flowAmount, int n)
        {
            double[] arr = new double[n];
            arr[0] = flowAmount;
            logger.Debug("[GetMatrixOfFlowGainedAtNodeFromZeroToN] Printing ");
            logger.Debug(LogHelper.PrintArr(arr));
            return arr;
        }

        private double[][] GetSystemOfEquations(Graph graph, List<Node> nodes)
        {
            int upperlimit = graph.Nodes.Count() - 1;
            double[][] equations = new double[upperlimit][];
            for (int row = 0; row < upperlimit; row++)
            {
                equations[row] = new double[upperlimit];
                for (int col = 0; col < upperlimit; col++)
                {
                    equations[row][col] = GetValueForSystemOfEquations(graph, nodes[row], nodes[col]);
                }
            }
            if (logger.IsTraceEnabled)
            {
                logger.Trace("[GetSystemOfEquations] Printing return value ");
                logger.Trace(LogHelper.PrintArr(equations));
            }
            return equations;
        }

        private double GetValueForSystemOfEquations(Graph graph, Node a, Node b)
        {
            double value = 0;
            if (a == b)
            {
                foreach (Edge connected in graph.EdgesConnectedToNode(a))
                {
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
