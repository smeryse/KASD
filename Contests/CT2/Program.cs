using System;
using System.Text;
class Program
{
    static int[] nest;
    static void Main()
    {
        var first = Console.ReadLine().Split();
        int n = int.Parse(first[0]);
        int m = int.Parse(first[1]);
        int q = int.Parse(first[2]);
        nest = new int[n + 1];
        for (int i = 1; i <= n; i++)
            nest[i] = -1;
        for (int i = 0; i < m; i++)
        {
            var parts = Console.ReadLine().Split();
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            nest[x] = y;
        }
        var sb = new StringBuilder();
        for (int i = 0; i < q; i++)
        {
            string line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split();
            int t = int.Parse(parts[0]);
            if (t == 1 || t == 2)
            {
                int x = int.Parse(parts[1]);
                int y = int.Parse(parts[2]);
                bool terminates = WillTerminate(x, y);
                sb.Append(terminates ? "Yes" : "No").Append('\n');
                if (t == 2 && terminates)
                {
                    PlaceEgg(x, y);
                }
            }
            else // t == 3
            {
                long count = 0;
                for (int x = 1; x <= n; x++)
                {
                    for (int y = 1; y <= n; y++)
                    {
                        if (x == y) continue;
                        if (WillTerminate(x, y))
                            count++;
                    }
                }
                sb.Append(count).Append('\n');
            }
        }
        Console.Write(sb.ToString());
    }
    static bool WillTerminate(int x, int y)
    {
        bool[] visited = new bool[nest.Length];
        int currentNest = x;
        int currentEgg = y;
        
        while (true)
        {
            if (visited[currentNest])
                return false; // Cycle detected
            visited[currentNest] = true;
            
            if (nest[currentNest] == -1)
                return true; // Found empty nest, can place egg
            
            // Simulate the same process as PlaceEgg:
            // Save the old egg that was in currentNest
            int oldEgg = nest[currentNest];
            // Place our egg in currentNest (simulation, don't actually change nest)
            // The displaced egg (currentNest, oldEgg) should be placed in nest oldEgg
            // So we need to place egg (oldEgg, currentNest) in nest oldEgg
            currentEgg = currentNest;
            currentNest = oldEgg;
        }
    }
    static void PlaceEgg(int x, int y)
    {
        int currentNest = x;
        int currentEggOther = y;
        
        while (true)
        {
            if (nest[currentNest] == -1)
            {
                nest[currentNest] = currentEggOther;
                return;
            }
            
            // Save the old egg that was in currentNest
            int oldEgg = nest[currentNest];
            // Place our egg in currentNest
            nest[currentNest] = currentEggOther;
            // The displaced egg (currentNest, oldEgg) should be placed in nest oldEgg
            // So we need to place egg (oldEgg, currentNest) in nest oldEgg
            // Wait, no. The egg that was in currentNest is (currentNest, oldEgg)
            // We want to place it in nest oldEgg, so we need to place (oldEgg, currentNest) there
            // Actually, the egg is (currentNest, oldEgg), and we place it in nest oldEgg
            // So the new egg to place is (oldEgg, currentNest)
            currentEggOther = currentNest;
            currentNest = oldEgg;
        }
    }
}