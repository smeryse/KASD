using System;
using System.Collections.Generic;

namespace CT2.Tasks
{
    static class Astrograd
    {
        public static void Solve()
        {
            int n = int.Parse(Console.ReadLine());
            List<int> queue = new List<int>();
            Dictionary<int, int> positions = new Dictionary<int, int>();
            int front = 0; // Индекс начала очереди
            
            for (int i = 0; i < n; i++)
            {
                string[] input = Console.ReadLine().Split();
                int eventType = int.Parse(input[0]);
                
                switch (eventType)
                {
                    case 1: // Новый человек встает в конец очереди
                        int id = int.Parse(input[1]);
                        queue.Add(id);
                        positions[id] = queue.Count - 1;
                        break;
                        
                    case 2: // Первый человек купил билет и уходит
                        if (front < queue.Count)
                        {
                            positions.Remove(queue[front]);
                            front++;
                        }
                        break;
                        
                    case 3: // Последнему надоело ждать, он уходит
                        if (front < queue.Count)
                        {
                            int lastIdx = queue.Count - 1;
                            positions.Remove(queue[lastIdx]);
                            queue.RemoveAt(lastIdx);
                        }
                        break;
                        
                    case 4: // Запрос: сколько людей перед человеком с номером q
                        int q = int.Parse(input[1]);
                        int qPos = positions[q];
                        Console.WriteLine(qPos - front);
                        break;
                        
                    case 5: // Запрос: кто стоит первым
                        Console.WriteLine(queue[front]);
                        break;
                }
            }
        }
    }
}