using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AMREM_Backup
{
    public partial class App : Application
    {
        private static string SettingsFilePath => Path.Combine(FileSystem.AppDataDirectory, "app_settings.xml");
        public static ApplicationSettingsNS.ApplicationSettings_Serializer.ApplicationSettings Settings { get; private set; }

        static void SetupStatic()
        {
            Settings = ApplicationSettingsNS.ApplicationSettings_Serializer.Load(SettingsFilePath);
        }

        public App()
        {
            InitializeComponent();

            if (!DesignMode.IsDesignModeEnabled)
            {
                SetupStatic();
            }

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            ApplicationSettingsNS.ApplicationSettings_Serializer.Save(SettingsFilePath, Settings);
        }

        protected override void OnResume()
        {
        }
    }
}
