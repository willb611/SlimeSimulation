using System;
using NLog;

namespace SlimeSimulation.LinearEquations
{
    public class LupDecompositionSolver : ILinearEquationSolver
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static int ErrorColumnNumber { get; private set; }

        // Ax = b
        public double[] FindX(double[][] a, double[] b)
        {
            if (Logger.IsTraceEnabled)
            {
                Logger.Trace("A: " + LogHelper.PrintArrWithSpaces(a) + ", B: " + LogHelper.PrintArrWithNewLines(b));
            }
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
            Logger.Debug("[LogDensity] Found nonzero: {0}, total: {1}, density %: {2}%", count, total, (count / (double)total) * 100);
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
                Logger.Trace("[LupSolve] for i {0} sequenceSum: {1}, x[i]: {2}", i, sequenceSum, x[i]);
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
                Logger.Trace("[ForwardSubstituteForY] for i {0} sequenceSum: {1}, y[i]: {2}", i, sequenceSum, y[i]);
            }
            return y;
        }

        internal int[] LupDecompose(double[][] a)
        {
            int n = a.Length;
            int[] pi = MakePi(a.Length);
            for (int k = 0; k < n; k++)
            {
                int kdash = GetRowWithBiggestValueForColumn(a, k, pi);
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

        private int GetRowWithBiggestValueForColumn(double[][] array, int columnNumber, int[] pi)
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
                var arrayString = LogHelper.PrintArr(array);
                ErrorColumnNumber = pi[columnNumber];
                var logstr = "No values found in column " + columnNumber + ". Meaning array A was singular: " + arrayString;
                Logger.Error(logstr);
                throw new SingularMatrixException(logstr);
            }
            return kdash;
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
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly double[][] _original;
        private readonly double[][] _upper;

        public UpperLowerMatrix(double[][] a)
        {
            _original = a;
            _upper = MakeUpper(a);
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
            if (Logger.IsTraceEnabled)
            {
                Logger.Trace("[LogUpper] Printing");
                Logger.Trace(LogHelper.PrintArr(_upper));
            }
        }

        private double CheckedUpper(int i, int j)
        {
            if (i > j)
            {
                return 0;
            }
            return _original[i][j];
        }

        internal double Lower(int i, int j)
        {
            CheckArg(i);
            CheckArg(j);
            if (i <= j)
            {
                Logger.Warn("Lower matrix requested element which would have been upper");
                return 0;
            }
            Logger.Trace("[Lower] Request for i, j: {0}, {1} returning {2}", i, j, _original[i][j]);
            return _original[i][j];
        }

        internal double Upper(int i, int j)
        {
            Logger.Trace("[Upper] Request for i, j: {0}, {1}", i, j);
            CheckArg(i);
            CheckArg(j);
            return _upper[i][j];
        }

        private void CheckArg(int i)
        {
            if (i < 0)
            {
                throw new ArgumentOutOfRangeException("Must be positive. Given: " + i);
            }
            if (i >= _original.Length)
            {
                throw new ArgumentOutOfRangeException("Must be 0 indexed index. Max: " + _original.Length + ", Given: " + i);
            }
        }
    }
}
