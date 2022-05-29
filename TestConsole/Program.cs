using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectorySearchEngine;

namespace TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var engine = new SearchEngine();
            Console.WriteLine(engine.GetDriveOverview());
            Console.WriteLine("Analysing the drive, Please wait!");
            var stat = engine.GetDriveStatistic();
            Console.WriteLine();
            Console.WriteLine($"Total no of bytes {stat.NoOfTotalBytes}");
            Console.WriteLine($"Total no of files {stat.NoOfFiles}");
            Console.WriteLine($"Total no of files with no access {stat.NoOfFilesWithNoAccess}");
            Console.WriteLine($"Total no of directories {stat.NoOfDirectories}");
            Console.WriteLine($"Total no of directories with no access {stat.NoOfDirectoriesWithNoAccess}");

            Console.ReadLine();
        }
    }
}
