using System;
using System.IO;

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

        public DriveStatistic GetDriveStatistic()
        {
            var drivestat=new DriveStatistic();
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType != DriveType.Fixed) continue ;
                GetStatistics(drive.RootDirectory, ref drivestat, (drive.TotalSize/1000000 - drive.AvailableFreeSpace/1000000)/1000);
                drivestat.ProgressInPercent = 100;
                ProgressNotification?.Invoke(this,new SearchProgress(drivestat));
            } 
            return drivestat;

        }
        public event EventHandler<SearchProgress> ProgressNotification;

        private void GetStatistics(DirectoryInfo rootDirectory, ref DriveStatistic stat, long reportEveryAdditionalMegaByte)
        {
            var actualProgress = stat.NoOfTotalBytes /1000000/ reportEveryAdditionalMegaByte;
            try
            {
                foreach (var file in rootDirectory.GetFiles())
                {
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
                    GetStatistics(subdir, ref stat, reportEveryAdditionalMegaByte);
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