namespace DirectorySearchEngine
{
    public class SearchProgress
    {
        public SearchProgress(DriveStatistic actualprogress)
        {
            DriveStatisticProgress = actualprogress;
        }
        public DriveStatistic DriveStatisticProgress { get; set; }

    }
}