
using System;
using System.Collections.Generic;
using System.IO;

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

class Program
{
    #region Helpers
    // Reads test cases from a file and returns list of (Matrix, Vector)
    static List<(Matrix, Vector)> ReadTests(string filename)
    {
        var tests = new List<(Matrix, Vector)>();
        string[] lines = File.ReadAllLines(filename);
        int i = 0;

        while (i < lines.Length)
        {
            // Skip empty lines
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                i++;
                continue;
            }

            int N = int.Parse(lines[i++]);
            double[][] G = new double[N][];
            for (int r = 0; r < N; r++)
            {
                G[r] = Array.ConvertAll(lines[i++].Split(), double.Parse);
            }

            double[] x = Array.ConvertAll(lines[i++].Split(), double.Parse);

            tests.Add((new Matrix(N, G), new Vector(x)));
        }

        return tests;
    }
    #endregion

    #region Main
    // Program entry point: reads tests and prints norms
    static void Main()
    {
        string testFile = "test.txt"; 

        List<(Matrix, Vector)> tests = ReadTests(testFile);

        int testNumber = 1;
        foreach (var (mat, vec) in tests)
        {
            try
            {
                double norm = vec.Norm(mat);
                Console.WriteLine($"Тест {testNumber}: Норма = {norm:F2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Тест {testNumber}: Ошибка - {ex.Message}");
            }
            testNumber++;
        }
    }
    #endregion
}
