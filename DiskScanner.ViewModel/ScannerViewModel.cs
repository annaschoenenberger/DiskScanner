using DirectorySearchEngine;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DiskScanner.ViewModel
{
    public class ScannerViewModel : ObservableObject
    {
        private double progressInPercent;

        public ScannerViewModel()
        {
            StartSearch = new RelayCommand(OnStart);
            StopSearch = new RelayCommand(OnStop);
            Engine = new SearchEngine();
            Engine.ProgressNotification += Engine_ProgressNotification;
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady) 
                { 
                    DriveList.Add(drive);       
                }
            }
            ActualDrive = DriveList.First();

        }

        private void Engine_ProgressNotification(object sender, SearchProgress e)
        {
            NoOfFiles = e.DriveStatisticProgress.NoOfFiles;
            ProgressInPercent = e.DriveStatisticProgress.ProgressInPercent;
            ActualDirectoryName = e.DriveStatisticProgress.ActualDirectoryName;
            if (ProgressInPercent >= 100)
            {
                SearchIsRunning = false;
                SearchIsStopped = true;
            }

        }

        public SearchEngine Engine { get; set; }

        private void OnStop()
        {
            TokenSource.Cancel();
            SearchIsRunning = false;
            SearchIsStopped = true;

        }

        public CancellationTokenSource TokenSource { get; set; }
        public DriveInfo ActualDrive 
        {
            get => driveInformation;
            set => SetProperty(ref driveInformation, value);
        }
        private void OnStart()
        {
            SearchIsRunning = true;
            SearchIsStopped = false;
            TokenSource = new CancellationTokenSource();
            Task.Run(() =>
                {
                    var totalUsedMegaBytes = (ActualDrive.TotalSize - ActualDrive.AvailableFreeSpace) / 1000000;
                    Engine.GetStatistics(ActualDrive.RootDirectory,totalUsedMegaBytes/1000, TokenSource.Token);
                    ProgressInPercent = 100;
                });
        }
        public ObservableCollection<DriveInfo> DriveList { get; }= new ObservableCollection<DriveInfo>();
        public ICommand StartSearch { get; set; }
        public ICommand StopSearch { get; set; }
        public double ProgressInPercent
        {
            get => progressInPercent;
            set => SetProperty(ref progressInPercent, value);
        }
        private int processedFiles;
        private string actualDirectoryName;
        private bool searchIsRunning;
        private bool searchIsStopped = true;
        private DriveInfo driveInformation;

        public int NoOfFiles
        {
            get { return processedFiles; }
            set { SetProperty(ref processedFiles, value); }
        }
        public string ActualDirectoryName
        {
            get => actualDirectoryName;
            set => SetProperty(ref actualDirectoryName, value);
        }
        public bool SearchIsRunning
        {
            get => searchIsRunning;
            set => SetProperty(ref searchIsRunning, value);
        }
        public bool SearchIsStopped
        {
            get => searchIsStopped;
            set => SetProperty(ref searchIsStopped, value);
        }
    }
}
