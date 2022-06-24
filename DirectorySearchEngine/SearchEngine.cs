using System;
using System.IO;
using System.Threading;

namespace DirectorySearchEngine
{
    public class SearchEngine
    {
        public string GetDriveOverview()
        {
            var driveInformation = "";

            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType != DriveType.Fixed) continue;
                driveInformation += $"Drive name={drive.Name}"+ Environment.NewLine;
                driveInformation += $"Available free space={drive.AvailableFreeSpace}" + Environment.NewLine;
                driveInformation += $"Total size={drive.TotalSize}" + Environment.NewLine;
                driveInformation += $"Used bytes={drive.TotalSize - drive.AvailableFreeSpace}";

            }
            return driveInformation;
        }

        private DriveStatistic GetDriveStatistic(CancellationToken cancellationToken)
        {
            var drivestat=new DriveStatistic();
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (cancellationToken.IsCancellationRequested) break;
                if (drive.DriveType != DriveType.Fixed) continue ;
                GetStatistics(drive.RootDirectory, (drive.TotalSize/1000000 - drive.AvailableFreeSpace/1000000)/1000, cancellationToken);
                if (!cancellationToken.IsCancellationRequested) drivestat.ProgressInPercent = 100;
                ProgressNotification?.Invoke(this,new SearchProgress(drivestat));
            } 
            return drivestat;

        }
        public event EventHandler<SearchProgress> ProgressNotification;

        public DriveStatistic Statistic { get; }=new DriveStatistic();
        public void GetStatistics(DirectoryInfo rootDirectory, long reportEveryAdditionalMegaByte, CancellationToken cancellationToken)
        {
            var actualProgress = Statistic.NoOfTotalBytes /1000000/ reportEveryAdditionalMegaByte;
            Statistic.ActualDirectoryName = rootDirectory.FullName;
            try
            {
                foreach (var file in rootDirectory.GetFiles())
                {
                    if (cancellationToken.IsCancellationRequested) break;
                    Statistic.NoOfFiles++;
                    Statistic.NoOfTotalBytes+=file.Length;
                    var newProgress=Statistic.NoOfTotalBytes/1000000/reportEveryAdditionalMegaByte;
                    //notification based on the size
                    if (newProgress > actualProgress)
                    {
                        Statistic.ProgressInPercent += 0.1;
                        actualProgress = newProgress;
                        ProgressNotification?.Invoke(this,new SearchProgress(Statistic));
                    }
                    //notification based on file count
                    if (Statistic.NoOfFiles %100==0)
                    {
                        ProgressNotification?.Invoke(this, new SearchProgress(Statistic));
                    }

                }
            }
            catch 
            {
                Statistic.NoOfFilesWithNoAccess++;
            }

            foreach (var subdir in rootDirectory.GetDirectories())
            {
                try
                {
                    if (cancellationToken.IsCancellationRequested) break;
                    GetStatistics(subdir, reportEveryAdditionalMegaByte, cancellationToken);
                    Statistic.NoOfDirectories++;
                }
                catch
                {
                    Statistic.NoOfDirectoriesWithNoAccess++;

                }
            }
        }
    }
}