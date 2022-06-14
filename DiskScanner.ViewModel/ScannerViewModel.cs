using DirectorySearchEngine;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }

        private void Engine_ProgressNotification(object sender, SearchProgress e)
        {
            NoOfFiles = e.DriveStatisticProgress.NoOfFiles;
            ProgressInPercent = e.DriveStatisticProgress.ProgressInPercent;
            ActualDirectoryName = e.DriveStatisticProgress.ActualDirectoryName;
        }

        public SearchEngine Engine { get; set; }

        private void OnStop()
        {
            throw new NotImplementedException();
        }

        private void OnStart()
        {
            Task.Run(() =>
                {
                    Engine.GetDriveStatistic();
                });
        }

        public ICommand StartSearch { get; set; }
        public ICommand StopSearch { get; set; }
        public double ProgressInPercent
        {
            get => progressInPercent;
            set => SetProperty(ref progressInPercent, value);
        }
        private int processedFiles;
        private string actualDirectoryName;

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

    }
}
