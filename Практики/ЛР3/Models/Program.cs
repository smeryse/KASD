using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string path = @"C:\2 курс\KASD\Практики\ЛР3\test1.txt";
        FileInfo file = new FileInfo(path);
        if (file.Exists)
        {
            Console.WriteLine("Имя файла: {0}", file.Name);
            Console.WriteLine("Время создания: {0}", file.CreationTime);
            Console.WriteLine("Размер: {0}", file.Length);
            string newPath = @"C:\2 курс\KASD\Практики\ЛР3\test2.txt";
            file.CopyTo(newPath, false);
            //file.Delete(); // Удаление файла
        }
        else
        {
            Console.WriteLine("Файл не найден.");
        }
    }
}
