using System;
using System.IO;

namespace DirectorySearchEngine
{
    public class SearchEngine
    {
        public string GetDriveInfo()
        {
            var driveInformation = "";

            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.Network) continue;
                driveInformation += $"DriveName={drive.Name}"+ Environment.NewLine;
                driveInformation += $"AvailableFreeSpace={drive.AvailableFreeSpace}" + Environment.NewLine;
                driveInformation += $"TotalSize={drive.TotalSize}" + Environment.NewLine;

            }
            return driveInformation;
        }

    }
}