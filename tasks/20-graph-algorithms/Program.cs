using Task20.Graphs;

namespace Task20
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ЗАДАЧА 20: Алгоритмы на графах ===\n");

            // =========================================
            // Алгоритм 1: Мальгранж — компоненты сильной связности
            // =========================================
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Алгоритм 1: Компоненты сильной связности (Мальгранж)");
            Console.ResetColor();

            var graph1 = new Graph(8);
            // Граф с несколькими SCC
            graph1.AddEdge(0, 1); graph1.AddEdge(1, 2); graph1.AddEdge(2, 0); // SCC: {0,1,2}
            graph1.AddEdge(3, 4); graph1.AddEdge(4, 5); graph1.AddEdge(5, 3); // SCC: {3,4,5}
            graph1.AddEdge(2, 3); // связь между SCC
            graph1.AddEdge(6, 7); graph1.AddEdge(7, 6); // SCC: {6,7}
            graph1.AddEdge(5, 6); // связь между SCC

            Console.WriteLine("Рёбра графа:");
            Console.WriteLine("  0→1, 1→2, 2→0 (SCC 1)");
            Console.WriteLine("  3→4, 4→5, 5→3 (SCC 2)");
            Console.WriteLine("  2→3, 5→6 (связи между SCC)");
            Console.WriteLine("  6→7, 7→6 (SCC 3)");

            var sccs = graph1.MalgrangeSCC();
            Console.WriteLine($"\nНайдено {sccs.Count} компонент сильной связности:");
            for (int i = 0; i < sccs.Count; i++)
            {
                Console.WriteLine($"  SCC {i + 1}: [{string.Join(", ", sccs[i])}]");
            }

            // Транзитивное замыкание
            Console.WriteLine("\nТранзитивное замыкание:");
            var tc = graph1.TransitiveClosureDFS();
            int n = graph1.VerticesCount;
            Console.Write("   ");
            for (int j = 0; j < n; j++) Console.Write($"{j,3}");
            Console.WriteLine();
            for (int i = 0; i < n; i++)
            {
                Console.Write($"  {i}");
                for (int j = 0; j < n; j++)
                    Console.Write(tc[i, j] ? "  1" : "  0");
                Console.WriteLine();
            }

            // =========================================
            // Алгоритм 2: Проталкивание предпотока — максимальный поток
            // =========================================
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Алгоритм 2: Максимальный поток (проталкивание предпотока)");
            Console.ResetColor();

            var flowNetwork = new PushRelabelMaxFlow(6);
            // Классический пример сети
            flowNetwork.AddEdge(0, 1, 16); flowNetwork.AddEdge(0, 2, 13);
            flowNetwork.AddEdge(1, 2, 10); flowNetwork.AddEdge(1, 3, 12);
            flowNetwork.AddEdge(2, 1, 4);  flowNetwork.AddEdge(2, 4, 14);
            flowNetwork.AddEdge(3, 2, 9);  flowNetwork.AddEdge(3, 5, 20);
            flowNetwork.AddEdge(4, 3, 7);  flowNetwork.AddEdge(4, 5, 4);

            Console.WriteLine("Сеть (6 вершин, источник=0, сток=5):");
            Console.WriteLine("  0→1: 16, 0→2: 13");
            Console.WriteLine("  1→2: 10, 1→3: 12");
            Console.WriteLine("  2→1: 4,  2→4: 14");
            Console.WriteLine("  3→2: 9,  3→5: 20");
            Console.WriteLine("  4→3: 7,  4→5: 4");

            int maxFlow = flowNetwork.ComputeMaxFlow(0, 5);
            Console.WriteLine($"\nМаксимальный поток: {maxFlow}");

            // =========================================
            // Алгоритм 3: Брон-Кербош — максимальная клика
            // =========================================
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Алгоритм 3: Максимальная клика (Брон-Кербош)");
            Console.ResetColor();

            var graph3 = new BronKerbosch(7);
            // Граф с кликами
            graph3.AddEdge(0, 1); graph3.AddEdge(0, 2); graph3.AddEdge(1, 2); // клика {0,1,2}
            graph3.AddEdge(2, 3); graph3.AddEdge(3, 4); graph3.AddEdge(4, 2); // клика {2,3,4}
            graph3.AddEdge(4, 5); graph3.AddEdge(5, 6); graph3.AddEdge(6, 4); // клика {4,5,6}
            graph3.AddEdge(1, 3); // дополнительная связь

            Console.WriteLine("Рёбра графа (7 вершин):");
            Console.WriteLine("  {0,1,2}, {2,3,4}, {4,5,6} — треугольники");
            Console.WriteLine("  1→3 — дополнительная связь");

            var maxClique = graph3.FindMaximumClique();
            Console.WriteLine($"\nВсе максимальные клики ({graph3.MaximalCliques.Count}):");
            for (int i = 0; i < graph3.MaximalCliques.Count; i++)
            {
                Console.WriteLine($"  Клика {i + 1}: [{string.Join(", ", graph3.MaximalCliques[i])}]");
            }
            Console.WriteLine($"\nНаибольшая клика: [{string.Join(", ", maxClique)}] (размер {maxClique.Count})");

            Console.WriteLine("\n=== ЗАДАЧА 20 ВЫПОЛНЕНА ===");
        }
    }
}
