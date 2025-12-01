using System;
using System.IO;
using Task6.Collections;

namespace Task7.PriorityQueueSimulation
{
    public class Request : IComparable<Request>
    {
        public int Priority { get; set; }
        public int Number { get; set; }
        public int StepAdded { get; set; }
        public int StepRemoved { get; set; } = -1;

        public int CompareTo(Request? other)
        {
            if (other == null) return 1;
            int pCmp = this.Priority.CompareTo(other.Priority);
            if (pCmp != 0) return pCmp;
            // При равном приоритете удаляем более раннюю заявку раньше
            return other.Number.CompareTo(this.Number);
        }
    }

    public static class SimulationRunner
    {
        public static void Run()
        {
            Console.Write("Введите количество шагов N: ");
            if (!int.TryParse(Console.ReadLine(), out int N) || N <= 0)
            {
                Console.WriteLine("Неверное значение N.");
                return;
            }

            var pq = new MyPriorityQueue<Request>();
            var rnd = new Random();
            int globalNumber = 1;
            int maxWait = -1;
            Request? maxReq = null;

            // Попытка определить папку проекта (в которой лежит PriorityQueueSimulation.csproj).
            string? projectDir = null;
            var dirInfo = new System.IO.DirectoryInfo(AppContext.BaseDirectory);
            while (dirInfo != null)
            {
                if (System.IO.File.Exists(System.IO.Path.Combine(dirInfo.FullName, "PriorityQueueSimulation.csproj")))
                {
                    projectDir = dirInfo.FullName;
                    break;
                }
                dirInfo = dirInfo.Parent;
            }
            var exeDir = projectDir ?? AppContext.BaseDirectory;
            var logPath = Path.Combine(exeDir, "log.txt");
            using (var log = new StreamWriter(logPath))
            {
                for (int step = 1; step <= N; step++)
                {
                    int k = rnd.Next(1, 11); // от 1 до 10 заявок
                    for (int i = 0; i < k; i++)
                    {
                        var req = new Request { Priority = rnd.Next(1, 6), Number = globalNumber++, StepAdded = step };
                        pq.Add(req);
                        log.WriteLine($"ADD {req.Number} {req.Priority} {req.StepAdded}");
                    }

                    if (pq.Size() > 0)
                    {
                        var rem = pq.Poll();
                        rem.StepRemoved = step;
                        int wait = rem.StepRemoved - rem.StepAdded;
                        log.WriteLine($"REMOVE {rem.Number} {rem.Priority} {rem.StepRemoved}");
                        if (wait > maxWait)
                        {
                            maxWait = wait;
                            maxReq = rem;
                        }
                    }
                }

                int stepAfter = N;
                while (pq.Size() > 0)
                {
                    stepAfter++;
                    var rem = pq.Poll();
                    rem.StepRemoved = stepAfter;
                    int wait = rem.StepRemoved - rem.StepAdded;
                    log.WriteLine($"REMOVE {rem.Number} {rem.Priority} {rem.StepRemoved}");
                    if (wait > maxWait)
                    {
                        maxWait = wait;
                        maxReq = rem;
                    }
                }
            }

            if (maxReq == null)
            {
                Console.WriteLine("Очередь была пуста, заявок не было.");
            }
            else
            {
                Console.WriteLine("Заявка с максимальным временем ожидания:");
                Console.WriteLine($"НомерЗаявки: {maxReq.Number}");
                Console.WriteLine($"Приоритет: {maxReq.Priority}");
                Console.WriteLine($"НомерШагаДобавления: {maxReq.StepAdded}");
                Console.WriteLine($"НомерШагаУдаления: {maxReq.StepRemoved}");
                Console.WriteLine($"ВремяОжидания: {maxWait}");
                Console.WriteLine("Логи записаны в файл log.txt (в текущей директории)");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Запуск симуляции приоритетной очереди.");
            SimulationRunner.Run();
        }
    }
}
