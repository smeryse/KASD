
using System;
using System.Collections.Generic;
using System.IO;

namespace Task1.Collections
{
    class Matrix
    {
        #region Properties and fields
        public int N { get; set; }
        public double[][] G;
        #endregion

        #region Constructors
        // Creates a matrix of size n with provided data
        public Matrix(int n, double[][] g)
        {
            N = n;
            G = g;
        }
        #endregion

        #region Methods
        // Checks whether the matrix is symmetric
        public bool IsSymmetric()
        {
            for (int i = 0; i < N; i++)
                for (int j = i + 1; j < N; j++)
                    if (G[i][j] != G[j][i])
                        return false;
            return true;
        }
        #endregion
    }

    class Vector
    {
        #region Properties and fields
        public int N { get; set; }
        double[] x;
        Matrix G;
        #endregion

        #region Constructors
        // Initializes vector from given array
        public Vector(double[] arr)
        {
            x = arr;
            N = arr.Length;
        }
        #endregion

        #region Methods
        // Computes the quadratic form norm using matrix G
        public double Norm(Matrix G)
        {
            if (!G.IsSymmetric())
                throw new InvalidOperationException("Матрица G не симметрична. Норма не может быть рассчитана.");

            double sum = 0;
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    sum += x[i] * G.G[i][j] * x[j];

            return Math.Sqrt(sum);
        }
        #endregion

    }
}

