using System;

class Program
{
    const int INF = 1000000000;
    
    static void Main()
    {
        int n = int.Parse(Console.ReadLine());
        int[,] dist = new int[n, n];
        
        for (int i = 0; i < n; i++)
        {
            string[] line = Console.ReadLine().Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < n; j++)
            {
                dist[i, j] = int.Parse(line[j]);
            }
        }
        
        int[,] dp = new int[1 << n, n];
        int[,] parent = new int[1 << n, n];
        
        for (int mask = 0; mask < (1 << n); mask++)
        {
            for (int i = 0; i < n; i++)
            {
                dp[mask, i] = INF;
                parent[mask, i] = -1;
            }
        }
        
        dp[1, 0] = 0;
        
        for (int mask = 1; mask < (1 << n); mask++)
        {
            for (int last = 0; last < n; last++)
            {
                if ((mask & (1 << last)) == 0 || dp[mask, last] >= INF)
                    continue;
                
                for (int next = 0; next < n; next++)
                {
                    if ((mask & (1 << next)) != 0)
                        continue;
                    
                    int newMask = mask | (1 << next);
                    int newDist = dp[mask, last] + dist[last, next];
                    
                    if (newDist < dp[newMask, next])
                    {
                        dp[newMask, next] = newDist;
                        parent[newMask, next] = last;
                    }
                }
            }
        }
        
        int fullMask = (1 << n) - 1;
        int minDist = INF;
        int lastCity = -1;
        
        for (int i = 0; i < n; i++)
        {
            if (dp[fullMask, i] < minDist)
            {
                minDist = dp[fullMask, i];
                lastCity = i;
            }
        }
        
        int[] path = new int[n];
        int pathIndex = 0;
        int currentMask = fullMask;
        int currentCity = lastCity;
        
        while (currentCity != -1)
        {
            path[pathIndex++] = currentCity + 1; 
            int prevCity = parent[currentMask, currentCity];
            if (prevCity != -1)
            {
                currentMask ^= (1 << currentCity);
            }
            currentCity = prevCity;
        }
        
        for (int i = 0; i < n / 2; i++)
        {
            int temp = path[i];
            path[i] = path[n - 1 - i];
            path[n - 1 - i] = temp;
        }
        
        Console.WriteLine(minDist);
        for (int i = 0; i < n; i++)
        {
            if (i > 0) Console.Write(" ");
            Console.Write(path[i]);
        }
        Console.WriteLine();
    }
}