using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimeSimulation.FlowCalculation.LinearEquations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.FlowCalculation.LinearEquations.Tests {
    [TestClass()]
    public class GaussianSolverTests {
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
    }
}
