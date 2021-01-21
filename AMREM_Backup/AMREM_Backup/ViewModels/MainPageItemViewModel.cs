using AMREM_Backup.MobileClasses;
using MobileClasses;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace AMREM_Backup.ViewModels
{
    public class MainPageItemViewModel : MobileDebugInfoSettings
    {
        public delegate void RemoveItemEventDel(MainPageItemViewModel sender);
        public event RemoveItemEventDel RemoveItemEvent;

        public Command GetDebugFile { get => new Command(() => GetDebugFileEvent()); }
        public Command RemoveItem { get => new Command(() => RemoveItemEvent?.Invoke(this)); }
        public Command ResetSettings { get => new Command(() => ResetSettingsEvent()); }

        private bool isRunning = false;
        private int percentComplete;

        public bool IsRunning { get => isRunning; set { SetProperty(ref isRunning, value); OnPropertyChanged("IsEnabled"); } }
        public bool IsEnabled { get => !isRunning; }
        public int PercentComplete { get => percentComplete; set => SetProperty(ref percentComplete, value); }

        public MainPageItemViewModel() : base()
        {
        }
        public MainPageItemViewModel(string ip, int port, string userName, string password, string destFileName, string destPath) : base(ip, port, userName, password, destFileName, destPath)
        {
        }


        private MobileDebugInfoDownload Debug { get; set; }

        private void GetDebugFileEvent()
        {
            if (!Directory.Exists(DestinationPath))
                Directory.CreateDirectory(DestinationPath);

            Debug = new MobileDebugInfoDownload();
            Debug.DownloadFileCompleted += Debug_DownloadFileCompleted;
            Debug.DownloadProgressChanged += Debug_DownloadProgressChanged;

            if (Debug.StartGetDebugFile(this))
                IsRunning = true;
        }

        private void Debug_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            PercentComplete = e.ProgressPercentage;
        }

        private void Debug_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Debug.Dispose();
            Debug = null;

            IsRunning = false;
        }
        private async void ResetSettingsEvent()
        {
            if (await Application.Current.MainPage.DisplayAlert("Default Values?", "Load default values?", "Yes", "No"))
            {
                UserName = "admin";
                Password = "admin";
                IPAddress = "1.2.3.4";
                DestinationFileName = "{ip}_backup_{MM-dd-yy_HH-mm-ss}.zip";
                DestinationPath = DestinationPath;
            }
        }
    }
}
