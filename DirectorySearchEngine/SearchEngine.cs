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

        public DriveStatistic GetDriveStatistic(CancellationToken cancellationToken)
        {
            var drivestat=new DriveStatistic();
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (cancellationToken.IsCancellationRequested) break;
                if (drive.DriveType != DriveType.Fixed) continue ;
                GetStatistics(drive.RootDirectory, ref drivestat, (drive.TotalSize/1000000 - drive.AvailableFreeSpace/1000000)/1000, cancellationToken);
                if (!cancellationToken.IsCancellationRequested) drivestat.ProgressInPercent = 100;
                ProgressNotification?.Invoke(this,new SearchProgress(drivestat));
            } 
            return drivestat;

        }
        public event EventHandler<SearchProgress> ProgressNotification;

        private void GetStatistics(DirectoryInfo rootDirectory, ref DriveStatistic stat, long reportEveryAdditionalMegaByte, CancellationToken cancellationToken)
        {
            var actualProgress = stat.NoOfTotalBytes /1000000/ reportEveryAdditionalMegaByte;
            stat.ActualDirectoryName = rootDirectory.FullName;
            try
            {
                foreach (var file in rootDirectory.GetFiles())
                {
                    if (cancellationToken.IsCancellationRequested) break;
                    stat.NoOfFiles++;
                    stat.NoOfTotalBytes+=file.Length;
                    var newProgress=stat.NoOfTotalBytes/1000000/reportEveryAdditionalMegaByte;
                    //notification based on the size
                    if (newProgress > actualProgress)
                    {
                        stat.ProgressInPercent += 0.1;
                        actualProgress = newProgress;
                        ProgressNotification?.Invoke(this,new SearchProgress(stat));
                    }
                    //notification based on file count
                    if (stat.NoOfFiles %100==0)
                    {
                        ProgressNotification?.Invoke(this, new SearchProgress(stat));
                    }

                }
            }
            catch 
            {
                stat.NoOfFilesWithNoAccess++;
            }

            foreach (var subdir in rootDirectory.GetDirectories())
            {
                try
                {
                    if (cancellationToken.IsCancellationRequested) break;
                    GetStatistics(subdir, ref stat, reportEveryAdditionalMegaByte, cancellationToken);
                    stat.NoOfDirectories++;
                }
                catch
                {
                    stat.NoOfDirectoriesWithNoAccess++;

                }
            }
        }
    }
}