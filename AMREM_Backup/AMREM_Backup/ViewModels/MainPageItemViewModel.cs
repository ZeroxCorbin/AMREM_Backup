﻿using AMREM_Backup.MobileClasses;
using MobileClasses;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
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
        private string message;
        public string Message { get => message; set { SetProperty(ref message, value); InvokePropertyChanged(this, "IsMessage"); } }
        public bool IsMessage { get => !string.IsNullOrEmpty(message); }

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

            Message = string.Empty; 
            PercentComplete = 0;
            IsRunning = true;
             
            ThreadPool.QueueUserWorkItem(new WaitCallback(GetDebugFileEvent_Thread));
        }

        private void GetDebugFileEvent_Thread(object sender)
        {
            Debug = new MobileDebugInfoDownload();
            Debug.DownloadFileCompleted += Debug_DownloadFileCompleted;
            Debug.DownloadProgressChanged += Debug_DownloadProgressChanged;

            if (!Debug.StartGetDebugFile(this))
            {
                IsRunning = false;
                Message = Debug.IsException ? Debug.Error.Message : "Unknown Error";
                PercentComplete = 0;

                Debug.Dispose();
                Debug = null;
            }
        }

        private void Debug_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e) => PercentComplete = e.ProgressPercentage;

        private void Debug_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if(e.Error != null)
            {
                Message = e.Error.Message;
            }

            Debug.Dispose();
            Debug = null;

            IsRunning = false;
        }
        private async void ResetSettingsEvent()
        {
            if (await Application.Current.MainPage.DisplayAlert("Default Values?", "Load default values?", "Yes", "No"))
            {
                Message = "";
                IsRunning = false;
                PercentComplete = 0;

                UserName = "admin";
                Password = "admin";
                IPAddress = "1.2.3.4";
                DestinationFileName = "{ip}_backup_{MM-dd-yy_HH-mm-ss}.zip";
                DestinationPath = DestinationPath;
            }
        }
    }
}
