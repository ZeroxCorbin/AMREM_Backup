using AMREM_Backup.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Transactions;
using System.Windows;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace ApplicationSettingsNS
{
    public class ApplicationSettings_Serializer
    {
        public static ApplicationSettings Load(string file)
        {
            StreamReader sr;
            ApplicationSettings app;
            XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
            try
            {
                sr = new StreamReader(file);
            }
            catch (FileNotFoundException)
            {
                ApplicationSettings_Serializer.Save(file, new ApplicationSettings());
                sr = new StreamReader(file);
            }

            try
            {
                app = (ApplicationSettings)serializer.Deserialize(sr);
            }
            catch
            {
                sr.Close();
                ApplicationSettings_Serializer.Save(file, new ApplicationSettings());
                sr = new StreamReader(file);
                app = (ApplicationSettings)serializer.Deserialize(sr);
            }

            sr.Close();
            return app;
        }
        public static void Save(string file, ApplicationSettings app)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
            using (StreamWriter sw = new StreamWriter(file))
            {
                serializer.Serialize(sw, app);
            }
        }

        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class ApplicationSettings : INotifyPropertyChanged
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
            
            private string ethernetSlaveRobotIP;

            private Color pageTextColor = Color.White;
            private Color pageBackgroundColor = Color.Black;

            private Color buttonTextColor = Color.White;
            private Color buttonBackgroundColor = Color.SlateGray;

            private Color entryTextColor = Color.White;
            private Color entryBackgroundColor = Color.Black;
            private Color entryPlaceholderColor = Color.Gray;

            private  ObservableCollection<MainPageItemViewModel> _Items = new ObservableCollection<MainPageItemViewModel>();
            public ObservableCollection<MainPageItemViewModel> Items { get => _Items; set => _Items = value; }

            public string EthernetSlaveRobotIP { get => ethernetSlaveRobotIP; set => SetProperty(ref ethernetSlaveRobotIP, value); }

            public Color PageTextColor { get => pageTextColor; set => SetProperty(ref pageTextColor, value); }
            public Color PageBackgroundColor { get => pageBackgroundColor; set => SetProperty(ref pageBackgroundColor, value); }

            public Color ButtonTextColor { get => buttonTextColor; set => SetProperty(ref buttonTextColor, value); }
            public Color ButtonBackgroundColor { get => buttonBackgroundColor; set => SetProperty(ref buttonBackgroundColor, value); }

            public Color EntryTextColor { get => entryTextColor; set => SetProperty(ref entryTextColor, value); }
            public Color EntryBackgroundColor { get => entryBackgroundColor; set => SetProperty(ref entryBackgroundColor, value); }
            public Color EntryPlaceholderColor { get => entryPlaceholderColor; set => SetProperty(ref entryPlaceholderColor, value); }
        }
    }
}
