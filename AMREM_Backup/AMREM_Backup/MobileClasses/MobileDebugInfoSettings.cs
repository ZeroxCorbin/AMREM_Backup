using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AMREM_Backup.MobileClasses
{
    public class MobileDebugInfoSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private string _IPAddress;
        private int _Port;
        private string _UserName;
        private string _Password;
        private string _DestinationFileName;
        private string _DestinationPath;

        public string IPAddress { get=> _IPAddress; 
            set { SetProperty(ref _IPAddress, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DestinationFileNameFormatted")); } }
        public int Port { get => _Port; set => SetProperty(ref _Port, value); }
        public string UserName { get => _UserName; set => SetProperty(ref _UserName, value); }
        public string Password { get => _Password; set => SetProperty(ref _Password, value); }
        public string DestinationFileName { get => _DestinationFileName; set { SetProperty(ref _DestinationFileName, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DestinationFullPathFormatted"));  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DestinationFileNameFormatted"));} }
        public string DestinationFileNameFormatted => FormatFileName();
        public string DestinationPath { get => _DestinationPath; set => SetProperty(ref _DestinationPath, value); }
        public string DestinationFullPathFormatted => Path.Combine(_DestinationPath, FormatFileName());

        private string FormatFileName()
        {
            string @return = _DestinationFileName;
            foreach (Match match in Regex.Matches(@return, @"(?<={).*?(?=})"))
            {
                if (match.Value.Equals("ip"))
                    @return = @return.Replace($"{{ip}}", _IPAddress.Replace('.', '-'));
                else
                {
                    string dt;
                    try
                    {
                        dt = DateTime.Now.ToString(match.Value);
                        @return = @return.Replace($"{{{match.Value}}}", DateTime.Now.ToString(match.Value));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        @return = @return.Replace($"{{{match.Value}}}", DateTime.Now.ToString("MM-dd-yy_HH-mm-ss"));
                    }
                }
            }
            if (!@return.EndsWith(".zip"))
                @return += ".zip";

            return @return;
        }
        public MobileDebugInfoSettings() { }
        public MobileDebugInfoSettings(string ip, int port, string userName, string password, string destFileName, string destPath)
        {
            _IPAddress = ip;
            _Port = port;
            _UserName = userName;
            _Password = password;
            _DestinationFileName = destFileName;
            _DestinationPath = destPath;
        }
    }
}
