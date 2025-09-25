using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ЛР3
{
    class Program
    {
        static void Main(string[] args)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine($"Name: {0}", drive.Name);
                Console.WriteLine($"Type: {0}", drive.DriveType);
                if (drive.IsReady)
                {
                    Console.WriteLine($"Drive total: {0}", drive.TotalSize);

                }
            }
        }
    }
}
