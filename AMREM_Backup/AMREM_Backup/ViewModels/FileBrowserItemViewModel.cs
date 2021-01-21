using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace AMREM_Backup.ViewModels
{
    public class FileBrowserItemViewModel : INotifyPropertyChanged
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

        public delegate void ShareFileEventDel(FileBrowserItemViewModel sender);
        public event ShareFileEventDel ShareFileEvent;

        public delegate void DeleteFileEventDel(FileBrowserItemViewModel sender);
        public event DeleteFileEventDel DeleteFileEvent;

        public Command ShareFile { get => new Command(() => ShareFileEvent?.Invoke(this)); }
        public Command DeleteFile { get => new Command(() => DeleteFileEvent?.Invoke(this)); }


        string _FileUri;
        public string Name { get => Path.GetFileName(_FileUri); }
        public string Directory { get => Path.GetDirectoryName(_FileUri); }
        public string Uri { get => _FileUri; }

        public FileBrowserItemViewModel(string fileUri) 
        {
            _FileUri = fileUri;
        }
    }
}
