using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AMREM_Backup.ViewModels
{
    public class FileBrowserViewModel : INotifyPropertyChanged
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

        public string Title { get; } = "AMREM Files";

        public ObservableCollection<FileBrowserItemViewModel> Items { get; } = new ObservableCollection<FileBrowserItemViewModel>();

        private string DestinationPath { get => Path.Combine(FileSystem.AppDataDirectory, "backups"); }

        public FileBrowserViewModel()
        {
            if (!Directory.Exists(DestinationPath))
                Directory.CreateDirectory(DestinationPath);

            foreach (string s in Directory.GetFiles(DestinationPath))
            {
                var fbiv = new FileBrowserItemViewModel(s);
                fbiv.ShareFileEvent += Fbiv_ShareFileEvent;
                fbiv.DeleteFileEvent += Fbiv_DeleteFileEvent;
                Items.Add(fbiv);
            }
        }

        private async void Fbiv_DeleteFileEvent(FileBrowserItemViewModel sender)
        {
            if (await Application.Current.MainPage.DisplayAlert("Delete File?", "Delete this file?", "Yes", "No"))
            {
                File.Delete(sender.Uri);
                Items.Remove(sender);
            }
        }

        private void Fbiv_ShareFileEvent(FileBrowserItemViewModel sender)
        {
            _ = Share.RequestAsync(new ShareTextRequest
            {
                Uri = sender.Uri,
                Title = "Share Backup"
            });
        }


    }
}
