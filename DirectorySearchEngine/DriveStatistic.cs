using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorySearchEngine
{
    public class DriveStatistic
    {
        public int NoOfFiles { get; set; }
        public int NoOfDirectoriesWithNoAccess { get; set; }
        public int NoOfDirectories { get; set; }
        public int NoOfFilesWithNoAccess { get; set; }
        public long NoOfTotalBytes { get; set; }
        public double ProgressInPercent { get; set; }
        public string ActualDirectoryName { get; set; }
        public void Reset()
        {
            NoOfFiles = 0;
            NoOfDirectoriesWithNoAccess = 0;
            NoOfDirectories= 0;
            NoOfFilesWithNoAccess= 0;
            NoOfTotalBytes= 0;
            ProgressInPercent = 0;
            ActualDirectoryName =String.Empty;
        }

    }
}
