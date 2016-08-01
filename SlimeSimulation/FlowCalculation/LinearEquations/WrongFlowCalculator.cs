using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace SlimeSimulation.FlowCalculation.LinearEquations {
    class WrongFlowCalculator {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public FlowResult CalculateFlow(ISet<Edge> edges, ISet<Node> nodes, Node source, Node sink, int flowAmount) {
            Graph graph = new Graph(edges, nodes);
            List<Node> nodeList = new List<Node>(nodes);
            double[][] A = GetSystemOfEquations(graph, nodeList);
            double[] B = GetMatrixOfFlowGainedAtNodeFromZeroToN(flowAmount, nodes.Count - 1);
            PerformGaussianElimination(A, B);
            Pressures pressures = new Pressures(PerformBackSubstitution(A, B), nodeList);
            FlowOnEdges flowOnEdges = GetFlowOnEdges(graph, pressures, nodeList);
            return new FlowResult(edges, source, sink, flowAmount, flowOnEdges);
        }

        private FlowOnEdges GetFlowOnEdges(Graph graph, Pressures pressures, List<Node> nodes) {
            FlowOnEdges result = new FlowOnEdges(graph.Edges);
            foreach (Edge edge in graph.Edges) {
                double pi = pressures.PressureAt(edge.A);
                double pj = pressures.PressureAt(edge.A);
                double flow = edge.Connectivity * (pi - pj);
                result.IncreaseFlowOnEdgeBy(edge, Math.Abs(flow));
                logger.Debug("For edge {0}, got pi {1}, pj {2}, and flow {3}", edge, pi, pj, flow);
            }
            return result;
        }

        private double GetFlowForEdge(double connectivityOnEdge, int i, int j, Pressures pressures) {
           return connectivityOnEdge * (pressures[i] - pressures[j]);
        }

        private double[] GetMatrixOfFlowGainedAtNodeFromZeroToN(int flowAmount, int n) {
            double[] arr = new double[n];
            arr[0] = flowAmount;
            logger.Debug("[GetMatrixOfFlowGainedAtNodeFromZeroToN] Printing ");
            PrintArr(arr);
            return arr;
        }

        private double[] PerformBackSubstitution(double[][] A, double[] B) {
            double[] X = new double[B.Length];
            for (int i = A.Length - 1; i >= 0; i--) {
                X[i] = B[i];
                for (int j = i + 1; j < A.Length; j++) {
                    X[i] = X[i] - A[i][j] * X[j];
                }
                X[i] = X[i] / A[i][i];
                logger.Debug("[PerformBackSubstitution] Performing. i: {0}, B[i]: {1}, X[i]: {2}", i, B[i], X[i]);
            }
            return X;
        }

        private void PerformGaussianElimination(double[][] A, double[] B) {
            logger.Debug("[PerformGaussianElimination] Performing");
            for (int j = 0; j < A.Length - 1; j++) {
                for (int i = j + 1; i < A.Length; i++) {
                    double m_ij = A[i][j] / A[j][j];
                    for (int k = j + 1; k < A.Length; k++) {
                        A[i][k] = A[i][k] - m_ij * A[j][k];
                    }
                    B[i] = B[i] - m_ij * B[j];
                }
            }
            logger.Debug("[PerformGaussianElimination] Finished. Result: ");
            logger.Debug("[PerformGaussianElimination] A: ");
            PrintArr(A);
            logger.Debug("[PerformGaussianElimination] B ");
            PrintArr(B);
        }

        private double[][] GetSystemOfEquations(Graph graph, List<Node> nodes) {
            int upperlimit = graph.Nodes.Count() - 1;
            double[][] equations = new double[upperlimit][];
            for (int row = 0; row < upperlimit; row++) {
                equations[row] = new double[upperlimit];
                for (int col = 0; col < upperlimit; col++) {
                    equations[row][col] = GetValueForSystemOfEquations(graph, nodes[row], nodes[col]);
                }
            }
            logger.Debug("[GetSystemOfEquations] Printing return value ");
            PrintArr(equations);
            return equations;
        }

        private void PrintArr(double[] arr) {
            logger.Debug(GetRow(arr));
        }
        private void PrintArr(double[][] arr) {
            logger.Debug("[PrintArr] Start");
            for (int i = 0; i < arr.Length; i++) {
                logger.Debug(GetRow(arr[i]));
            }
            logger.Debug("[PrintArr] Finish ");
        }
        private String GetRow(double[] row) {
            StringBuilder builder = new StringBuilder();
            for (int j = 0; j < row.Length; j++) {
                if (row[j] < 0) {
                    builder.Append(String.Format("{0, 5:f4}", row[j]));
                } else {
                    builder.Append("+")
                        .Append(String.Format("{0, 5:f4}", row[j]));
                }
                builder.Append(" ");
            }
            return builder.ToString();
        }

        private double GetValueForSystemOfEquations(Graph graph, Node a, Node b) {
            double value = 0;
            if (a == b) {
                foreach (Edge connected in graph.EdgesConnectedToNode(a)) {
                    value += connected.Connectivity;
                }
            } else {
                value = -graph.GetEdgeConnectivityOrZero(a, b);
            }
            return value;
        }
    }
}
