using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using SlimeSimulation.FlowCalculation.LinearEquations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.FlowCalculation.LinearEquations.Tests {
    [TestClass()]
    public class GaussianSolverTests {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [TestMethod()]
        public void exchangeTest() {
            int v1 = 6;
            int a = v1;
            int v2 = 4;
            int b = v2;

            GaussianSolver gaussianSolver = new GaussianSolver();
            gaussianSolver.Exchange(ref a, ref b);
            Assert.AreEqual(a, v2);
            Assert.AreEqual(b, v1);
        }

        [TestMethod()]
        public void testFindX() {
            double[][] arr = new double[][] {
                new double[] { 1, 2, 0 }, new double[] { 3, 4, 4 }, new double[] { 5, 6, 3 }
            };
            double[] b = new double[] { 3, 7, 8 };

            var solver = new GaussianSolver();
            double[] x = solver.FindX(arr, b);
            Assert.AreEqual(-1.4, x[0], 0.000001);
            Assert.AreEqual(2.2, x[1], 0.000001);
            Assert.AreEqual(0.6, x[2], 0.000001);
        }

        [TestMethod()]
        public void testLupDecompose() {
            double[][] arr = new double[][] {
                new double[] { 2, 0, 2, 0.6 }, new double[] { 3, 3, 4, -2 },
                new double[] { 5, 5, 4, 2 }, new double[] { -1, -2, 3.4, -1 }
            };
            var solver = new GaussianSolver();
            int[] pi = solver.LupDecompose(arr);
            int[] expectedPi = { 2, 0, 3, 1 };
            for (int i = 0; i < pi.Length; i++) {
                logger.Debug("[testLupDecompose] i: {0}, pi[i]: {1}", i, pi[i]);
                Assert.AreEqual(expectedPi[i], pi[i]);
            }

            double[][] expectedArr = new double[][] {
                new double[] { 0.4, -2, 0.4, -0.2 }, new double[] { 0.6, 0, 0.4, -3 },
                new double[] { 5, 5, 4, 2 }, new double[] { -0.2, 0.5, 4, -0.5 }
            };
            int cols = arr.Length;
            for (int row = 0; row < arr.Length; row++) {
                for (int col = 0; col < cols; col++) {
                    logger.Debug("[testLupDecompose] row {0}, col {1}, arr[row][col] = {2}", row, col, arr[row][col]);
                    Assert.AreEqual(expectedArr[pi[row]][col], arr[row][col], 0.000001);
                }
            }
        }

        [TestMethod()]
        public void testForwardSubstituteForY() {
            double[][] lowerUpper = new double[][] {
                new double[] { 5, 6, 3 }, new double[] { 0.2, 0.8, -0.6 },
                new double[] { 0.6, 0.5, 2.5 }
            };
            var matrix = new UpperLowerMatrix(lowerUpper);
            double[] b = new double[] { 3, 8, 7 };
            int[] pi = new int[] { 1, 0, 2 };

            var solver = new GaussianSolver();
            double[] y = solver.ForwardSubstituteForY(matrix, pi, b, b.Length);
            double[] expectedY = new double[] { 8, 1.4, 1.5 };
            for (int i = 0; i < expectedY.Length; i++) {
                Assert.AreEqual(expectedY[i], y[i], 0.00001);
            }
        }


        [TestMethod()]
        public void testBackSubstitutionForX() {
            double[][] lowerUpper = new double[][] {
                new double[] { 5, 6, 3 }, new double[] { 0.2, 0.8, -0.6 },
                new double[] { 0.6, 0.5, 2.5 }
            };
            var matrix = new UpperLowerMatrix(lowerUpper);
            int[] pi = new int[] { 0, 1, 2 };

            double[] y = new double[] { 8, 1.4, 1.5 };

            double[] expectedX = new double[] { -1.4, 2.2, 0.6 };
            var solver = new GaussianSolver();
            double[] x = solver.BackSubstitutionForX(matrix, pi, y, y.Length);
            for (int i = 0; i < expectedX.Length; i++) {
                Assert.AreEqual(expectedX[i], x[i], 0.00001);
            }
        }
    }
}
