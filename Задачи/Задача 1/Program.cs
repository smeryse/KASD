
using System;
using System.Collections.Generic;
using System.IO;

class Matrix
{
    public int N { get; set; }
    public double[][] G;

    public Matrix(int n, double[][] g)
    {
        N = n;
        G = g;
    }

    public bool IsSymmetric()
    {
        for (int i = 0; i < N; i++)
            for (int j = i + 1; j < N; j++)
                if (G[i][j] != G[j][i])
                    return false;
        return true;
    }
}

class Vector
{
    public int N { get; set; }
    double[] x;
    Matrix G;

    public Vector(double[] arr)
    {
        x = arr;
        N = arr.Length;
    }

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

}

class Program
{
    static List<(Matrix, Vector)> ReadTests(string filename)
    {
        var tests = new List<(Matrix, Vector)>();
        string[] lines = File.ReadAllLines(filename);
        int i = 0;

        while (i < lines.Length)
        {
            // Пропускаем пустые строки
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

    static void Main()
    {
        string testFile = @"C:\2 курс\Конструирование алгоритмов и структур данных\Задачи\Задача 1\test.txt"; 

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
}
