using AMREM_Backup.Views;
using MobileClasses;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AMREM_Backup.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public string Title { get; } = "AMREM Backup";
        public string DestinationPath { get => Path.Combine(FileSystem.AppDataDirectory, "backups"); }



        public ObservableCollection<MainPageItemViewModel> Items { get => App.Settings.Items; }

        public Command AddItem { get; }
        public Command OpenFolder { get; }

        public MainPageViewModel()
        {
            foreach (MainPageItemViewModel mpvi in Items)
                mpvi.RemoveItemEvent += Mpvi_RemoveItemEvent;

            AddItem = new Command(() =>
            {
                MainPageItemViewModel mpvi1 = new MainPageItemViewModel("192.168.0.20", 443, "admin", "admin", "{ip}_backup_{MM-dd-yy_HH-mm-ss}.zip", DestinationPath);

                mpvi1.RemoveItemEvent += Mpvi_RemoveItemEvent;

                Items.Add(mpvi1);
            });

            OpenFolder = new Command(async () =>
            {
                _ = Application.Current.MainPage.Navigation.PushAsync(new FileBrowserView());
                //string[] fileTypes = null;
                //if (Device.RuntimePlatform == Device.Android)
                //{
                //    fileTypes = new string[] { "zip/zip" };
                //}

                //if (Device.RuntimePlatform == Device.iOS)
                //{
                //    fileTypes = new string[] { "public.image" }; // same as iOS constant UTType.Image  
                //}

                //if (Device.RuntimePlatform == Device.UWP)
                //{
                //    fileTypes = new string[] { ".zip" };
                //}

                //await PickAndShow(fileTypes);

                ////var supportsUri = await Launcher.CanOpenAsync(DestinationPath);
                ////if (supportsUri)
                ////    await Launcher.OpenAsync(DestinationPath);
            });

        }



        private async void Mpvi_RemoveItemEvent(MainPageItemViewModel sender)
        {
            if (await Application.Current.MainPage.DisplayAlert("Remove Entry?", "Delete this entry?", "Yes", "No"))
                Items.Remove(sender);
        }

    }
}
