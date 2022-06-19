using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DirectorySearchEngine;

namespace TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var engine = new SearchEngine();
            engine.ProgressNotification += Engine_ProgressNotification;
            Console.WriteLine(engine.GetDriveOverview());
            Console.WriteLine("Analysing the drive, please wait!");
            var cancellationTokenSource = new CancellationTokenSource();
            var stat = engine.GetDriveStatistic(cancellationTokenSource.Token);
            Console.WriteLine();
            Console.WriteLine($"Final results:");
            Console.WriteLine($"Total no of bytes {stat.NoOfTotalBytes}");
            Console.WriteLine($"Total no of files {stat.NoOfFiles}");
            Console.WriteLine($"Total no of files with no access {stat.NoOfFilesWithNoAccess}");
            Console.WriteLine($"Total no of directories {stat.NoOfDirectories}");
            Console.WriteLine($"Total no of directories with no access {stat.NoOfDirectoriesWithNoAccess}");

            Console.ReadLine();
        }

        private static void Engine_ProgressNotification(object sender, SearchProgress e)
        {
            Console.CursorTop=7;
            Console.CursorLeft = 0;
            Console.WriteLine($"Progress: {e.DriveStatisticProgress.ProgressInPercent:F1} %");
            Console.WriteLine($"Total no of bytes {e.DriveStatisticProgress.NoOfTotalBytes}");
            Console.WriteLine($"Total no of files {e.DriveStatisticProgress.NoOfFiles}");
            Console.WriteLine($"Total no of files with no access {e.DriveStatisticProgress.NoOfFilesWithNoAccess}");
            Console.WriteLine($"Total no of directories {e.DriveStatisticProgress.NoOfDirectories}");
            Console.WriteLine($"Total no of directories with no access {e.DriveStatisticProgress.NoOfDirectoriesWithNoAccess}");

        }
    }
}
