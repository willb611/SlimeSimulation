using System;

namespace SlimeSimulation.Algorithms.LinearEquations
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
