using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.FlowCalculation.LinearEquations
{
    public class LupDecompositionSolver : LinearEquationSolver
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // Ax = b
        public double[] FindX(double[][] a, double[] b)
        {
            LogDensity(a);
            var pi = LupDecompose(a);
            var matrix = new UpperLowerMatrix(a);
            matrix.LogUpper();
            return LupSolve(matrix, pi, b);
        }

        private void LogDensity(double[][] a)
        {
            int count = 0;
            int total = 0;
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < a[i].Length; j++)
                {
                    total++;
                    if (a[i][j] != 0)
                    {
                        count++;
                    }
                }
            }
            logger.Debug("[LogDensity] Found nonzero: {0}, total: {1}, density %: {2}%", count, total, (count / (double)total) * 100);
        }

        private int[] MakePi(int length)
        {
            var pi = new int[length];
            for (int i = 0; i < length; i++)
            {
                pi[i] = i;
            }
            return pi;
        }

        internal double[] LupSolve(UpperLowerMatrix matrix, int[] pi, double[] b)
        {
            int n = b.Length;
            double[] y = ForwardSubstituteForY(matrix, pi, b, n);
            double[] x = BackSubstitutionForX(matrix, pi, y, n);
            return x;
        }

        internal double[] BackSubstitutionForX(UpperLowerMatrix matrix, int[] pi, double[] y, int n)
        {
            double[] x = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                double sequenceSum = 0;
                for (int j = i + 1; j < n; j++)
                {
                    sequenceSum += matrix.Upper(i, j) * x[j];
                }
                x[i] = (y[i] - sequenceSum) / matrix.Upper(i, i);
                logger.Trace("[LupSolve] for i {0} sequenceSum: {1}, x[i]: {2}", i, sequenceSum, x[i]);
            }
            return x;
        }

        internal double[] ForwardSubstituteForY(UpperLowerMatrix matrix, int[] pi, double[] b, int n)
        {
            double[] y = new double[n];
            for (int i = 0; i < n; i++)
            {
                double sequenceSum = 0;
                for (int j = 0; j < i; j++)
                {
                    sequenceSum += matrix.Lower(i, j) * y[j];
                }
                y[i] = b[pi[i]] - sequenceSum;
                logger.Trace("[ForwardSubstituteForY] for i {0} sequenceSum: {1}, y[i]: {2}", i, sequenceSum, y[i]);
            }
            return y;
        }

        internal int[] LupDecompose(double[][] a)
        {
            int n = a.Length;
            int[] pi = MakePi(a.Length);
            for (int k = 0; k < n; k++)
            {
                int kdash = GetRowWithBiggestValueForColumn(a, k);
                Exchange(ref pi[k], ref pi[kdash]);
                ExchangeRows(a, k, kdash);
                for (int i = k + 1; i < n; i++)
                {
                    a[i][k] = a[i][k] / a[k][k];
                    for (int j = k + 1; j < n; j++)
                    {
                        a[i][j] = a[i][j] - a[i][k] * a[k][j];
                    }
                }
            }
            return pi;
        }

        private void ExchangeRows(double[][] array, int rowOne, int rowTwo)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Exchange(ref array[rowOne][i], ref array[rowTwo][i]);
            }
        }

        private int GetRowWithBiggestValueForColumn(double[][] array, int columnNumber)
        {
            double p = 0;
            int kdash = -1;
            for (int i = columnNumber; i < array.Length; i++)
            {
                if (Math.Abs(array[i][columnNumber]) > p)
                {
                    p = Math.Abs(array[i][columnNumber]);
                    kdash = i;
                }
            }
            if (p == 0)
            {
                String arrayString = LogHelper.PrintArr(array);
                throw new ArgumentException("Array was singular: " + arrayString);
            }
            else
            {
                return kdash;
            }
        }

        private void Exchange(ref double v1, ref double v2)
        {
            var tmp = v1;
            v1 = v2;
            v2 = tmp;
        }

        public void Exchange(ref int v1, ref int v2)
        {
            var tmp = v1;
            v1 = v2;
            v2 = tmp;
        }
    }

    internal class UpperLowerMatrix
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private double[][] original;
        private double[][] upper;

        public UpperLowerMatrix(double[][] a)
        {
            this.original = a;
            upper = MakeUpper(a);
        }

        private double[][] MakeUpper(double[][] array)
        {
            double[][] upper = new double[array.Length][];
            for (int i = 0; i < upper.Length; i++)
            {
                upper[i] = new double[array[i].Length];
                for (int j = 0; j < array[i].Length; j++)
                {
                    upper[i][j] = CheckedUpper(i, j);
                }
            }
            return upper;
        }

        public void LogUpper()
        {
            if (logger.IsTraceEnabled)
            {
                logger.Trace("[LogUpper] Printing");
                logger.Trace(LogHelper.PrintArr(upper));
            }
        }

        private double CheckedUpper(int i, int j)
        {
            if (i > j)
            {
                return 0;
            }
            else
            {
                return original[i][j];
            }
        }

        internal double Lower(int i, int j)
        {
            CheckArg(i);
            CheckArg(j);
            if (i <= j)
            {
                logger.Warn("Lower matrix requested element which would have been upper");
                return 0;
            }
            logger.Trace("[Lower] Request for i, j: {0}, {1} returning {2}", i, j, original[i][j]);
            return original[i][j];
        }

        internal double Upper(int i, int j)
        {
            logger.Trace("[Upper] Request for i, j: {0}, {1}", i, j);
            CheckArg(i);
            CheckArg(j);
            return upper[i][j];
        }

        private void CheckArg(int i)
        {
            if (i < 0)
            {
                throw new ArgumentOutOfRangeException("Must be positive. Given: " + i);
            }
            else if (i >= original.Length)
            {
                throw new ArgumentOutOfRangeException("Must be 0 indexed index. Max: " + original.Length + ", Given: " + i);
            }
        }
    }
}
