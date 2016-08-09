using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.FlowCalculation.LinearEquations
{
    public class SingularMatrixException : Exception
    {
        public SingularMatrixException()
        {

        }

        public SingularMatrixException(String message) : base(message)
        {

        }

        public SingularMatrixException(String message, Exception inner) : base(message, inner)
        {

        }
    }
}
