using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using NLog;

namespace SlimeSimulation.FlowCalculation {
    public class FlowCalculator {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly static int DEFAULT_FLOW_EXPONENT = 2;
        private readonly static double DEFAULT_MAX_ERROR = 0.01;
        private readonly int flowExponent;
        private readonly double maxErrorAllowedWhileCalculatingHeadLoss;
        private readonly FlowFinder flowFinder = new FlowFinder();

        public FlowCalculator() : this(DEFAULT_FLOW_EXPONENT, DEFAULT_MAX_ERROR) {
        }

        public FlowCalculator(int flowExponent, double maximumErrorExclusiveAllowedDuringCalculation) {
            this.flowExponent = flowExponent;
            this.maxErrorAllowedWhileCalculatingHeadLoss = maximumErrorExclusiveAllowedDuringCalculation;
        }

        public int FlowExponent {
            get {
                return flowExponent;
            }
        }

        public double MaxErrorAllowedWhileCalculatingHeadLoss {
            get {
                return maxErrorAllowedWhileCalculatingHeadLoss;
            }
        }

        public FlowResult CalculateFlow(List<Edge> edges, List<Loop> loops,
                Node source, Node sink, int flowAmount) {
            if (edges == null) {
                throw new ArgumentNullException("Given null edges");
            } else if (loops == null) {
                throw new ArgumentNullException("Given null loops");
            } else if (source == null) {
                throw new ArgumentNullException("Given null source");
            } else if (sink == null) {
                throw new ArgumentNullException("Given null sink");
            } else if (flowAmount <= 0) {
                throw new ArgumentException("Flow must be a positive integer");
            } else if (source.Equals(sink)) {
                throw new ArgumentException("Source should not equal sink");
            }
            logger.Debug("[CalculateFlow] Entered");
            Graph graph = new Graph(edges);
            IntermediateFlowResult flowEstimate = GetInitialFlow(graph, loops, source, sink, flowAmount);
            while (flowEstimate.MaxErrorFoundOnCalculatingHeadLoss > maxErrorAllowedWhileCalculatingHeadLoss) {
                flowEstimate = ImproveEstimateForFlowInNetwork(flowEstimate);
            }
            return new FlowResult(edges, source, sink, flowAmount, flowEstimate.FlowOnEdges);
        }

        internal IntermediateFlowResult GetInitialFlow(Graph graph, List<Loop> loops,
            Node source, Node sink, int flowAmount) {
            List<LoopWithDirectionOfFlow> loopsWithDirection = flowFinder.GetLoopsWithDirectionForFlow(loops, source, sink, graph);
            FlowOnEdges flowOnEdges = flowFinder.EstimateFlowForEdges(graph, source, sink, flowAmount);
            return new IntermediateFlowResult(double.MaxValue, loopsWithDirection, flowOnEdges);
        }

        internal IntermediateFlowResult ImproveEstimateForFlowInNetwork(IntermediateFlowResult previousFlowResult) {
            logger.Debug("[ImproveEstimateForFlowInNetwork] Previous error in flow: " + previousFlowResult.MaxErrorFoundOnCalculatingHeadLoss);
            double maxError = 0;
            var flowOnEdges = new FlowOnEdges(previousFlowResult.FlowOnEdges);
            if (previousFlowResult.Loops.Count == 0) {
                logger.Warn("[ImproveEstimateForFlowInNetwork] No loops given for graph, is the graph empty?");
            }
            foreach (Loop loop in previousFlowResult.Loops) {
                logger.Debug("[ImproveEstimateForFlowInNetwork] Improving loop: " + loop);
                double headLossInLoop = CalculateHeadLossInLoop(loop, previousFlowResult.FlowOnEdges);
                logger.Debug("[ImproveEstimateForFlowInNetwork] Found head loss in loop as : " + headLossInLoop);
                double sumQuantities = FindSumOfQuantitiesInLoop(loop, previousFlowResult.FlowOnEdges);
                logger.Debug("[ImproveEstimateForFlowInNetwork] Found sum of quantites in loop as : " + sumQuantities);
                var deltaToBeApplied = headLossInLoop / sumQuantities;
                logger.Debug("[ImproveEstimateForFlowInNetwork] Found delta to be applied : " + deltaToBeApplied);
                maxError = Math.Max(maxError, deltaToBeApplied);
                ApplyDeltaToEdgesInLoop(ref flowOnEdges, deltaToBeApplied, loop);
                logger.Debug("[ImproveEstimateForFlowInNetwork] For loop: " + loop + ", found maxError: " + maxError);
            }
            flowOnEdges.LogFlowInLoops();
            return new IntermediateFlowResult(maxError, previousFlowResult.Loops, flowOnEdges);
        }
        
        private double CalculateHeadLossInLoop(Loop loop, FlowOnEdges previousFlowEstimate) {
            double clockwiseSum = CalculateHeadInEdges(previousFlowEstimate, loop.Clockwise);
            double antiClockwiseSum = CalculateHeadInEdges(previousFlowEstimate, loop.AntiClockwise);
            return clockwiseSum - antiClockwiseSum;
        }

        private double CalculateHeadInEdges(FlowOnEdges flowOnEdges, IEnumerable<Edge> edges) {
            double sum = 0;
            foreach (Edge edge in edges) {
                double headInSection = edge.Resistance * (Expon(flowOnEdges.GetFlowOnEdge(edge), flowExponent));
                sum += headInSection;
                logger.Debug("[CalculateHeadInEdges] For edge " + edge + ", found head as " + headInSection);
            }
            if (sum == 0) {
                logger.Warn("[CalculateHeadInEdges] Found sum of head as 0. Flow result will probably be wrong");
            }
            return sum;
        }

        private double Expon(double v, int exponent) {
            if (exponent < 0) {
                throw new ArgumentException("Unexpected negative exponent. Given: " + exponent);
            } else if (exponent == 0) {
                return 1;
            } else if (exponent == 1) {
                return v;
            } else {
                double sum = v;
                for (int i = exponent; i > 1; i--) {
                    sum *= v;
                }
                logger.Debug("[Expon] For exponent: " + exponent + ", and v: " + v + ", calculated sum: " + sum);
                return sum;
            }
        }

        private double FindSumOfQuantitiesInLoop(Loop loop, FlowOnEdges flowOnEdges) {
            double sum = 0;
            foreach (Edge edge in loop.Edges) {
                double quantityInSection = flowExponent * edge.Resistance * Expon(flowOnEdges.GetFlowOnEdge(edge), flowExponent - 1);
                sum += quantityInSection;
                logger.Debug("[FindSumOfQuantitiesInLoop] For edge: " + edge + ", found quantityInSection as: " + quantityInSection);
            }
            if (sum == 0) {
                logger.Warn("[FindSumOfQuantitiesInLoop] Found sum as 0. Flow result will probably be wrong");
            }
            return sum;
        }

        private void ApplyDeltaToEdgesInLoop(ref FlowOnEdges flowOnEdges, double deltaToBeApplied, Loop loop) {
            ApplyDeltaToEdges(ref flowOnEdges, loop.Clockwise, -deltaToBeApplied);
            ApplyDeltaToEdges(ref flowOnEdges, loop.AntiClockwise, deltaToBeApplied);
        }

        private void ApplyDeltaToEdges(ref FlowOnEdges flowOnEdges, IEnumerable<Edge> edges, double deltaToBeApplied) {
            foreach (Edge edge in edges) {
                flowOnEdges.IncreaseFlowOnEdgeBy(edge, deltaToBeApplied);
            }
        }
    }
}
