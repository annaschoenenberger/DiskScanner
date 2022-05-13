namespace DirectorySearchEngine
{
    public class DirectorySearchEngine
    {
        public string GetDriveInfo()
        {
            var driveInformation = "";

            foreach (var drive in DriveInfo.GetDrives())
            {
                driveInformation += $"DriveName={drive.Name}"+ Environment.NewLine;
                driveInformation += $"AvailableFreeSpace={drive.AvailableFreeSpace}" + Environment.NewLine;
                driveInformation += $"TotalSize={drive.TotalSize}" + Environment.NewLine;



            }
            return driveInformation;
        }

    }
}