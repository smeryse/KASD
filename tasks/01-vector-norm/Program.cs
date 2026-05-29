using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Task1.Collections;

class Program
{
    #region Helpers
    
    static List<(Matrix, Vector)> ReadTests(string filename)
    {
        var tests = new List<(Matrix, Vector)>();
        string[] lines = File.ReadAllLines(filename);
        int i = 0;

        while (i < lines.Length)
        {
            
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
