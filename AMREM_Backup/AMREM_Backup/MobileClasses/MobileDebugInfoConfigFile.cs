using MobileClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Xamarin.Essentials;

namespace AMREM_Backup.MobileClasses
{
    public class MobileDebugInfoConfigFile
    {
        //Static Methods
        public const string DefaultConfigFileName = "RobotConnectionData.xml";
        public static string DefaultConfigFilePath = FileSystem.AppDataDirectory;
        public static string DefaultConfigFilePathRoot = "C:\\";

        public List<MobileDebugInfoSettings> RobotConnectionData { get; private set; } = new List<MobileDebugInfoSettings>();
        public string FilePath { get; private set; }

        public static void GenerateConfigFile(string filePath)
        {
            if (filePath == null)
                filePath = Path.Combine(DefaultConfigFilePath, DefaultConfigFileName);

            List<MobileDebugInfoSettings> lst = new List<MobileDebugInfoSettings>
            {
                new MobileDebugInfoSettings("192.168.1.1", 443, "admin", "admin", "backup_{ip}_{MM-dd-yy_HH-mm-ss}.zip", DefaultConfigFilePath),
                new MobileDebugInfoSettings("192.168.1.2", 443, "admin", "admin", "backup_{ip}_{MM-dd-yy_HH-mm-ss}.zip", DefaultConfigFilePath)
            };

            if (!filePath.EndsWith(".xml"))
                filePath += ".xml";

            if (SerializeConfiguration(filePath, lst))
                Console.WriteLine($"The generated config file is located at: {filePath}");
            else
                Console.WriteLine($"ERROR: Could not generate config file located at: {filePath}");
        }
        public static bool SerializeConfiguration(string filePath, List<MobileDebugInfoSettings> robotConnectionData)
        {
            try
            {
                XmlSerializer serialiser = new XmlSerializer(typeof(List<MobileDebugInfoSettings>));
                using (TextWriter filestream = new StreamWriter(filePath))
                    serialiser.Serialize(filestream, robotConnectionData);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public MobileDebugInfoConfigFile(string filePath = null, bool init = false)
        {
            FilePath = filePath;

            if (init)
                Init();
        }

        public void Init(bool useRoot = false)
        {
            if (useRoot && string.IsNullOrEmpty(FilePath))
                FilePath = Path.Combine(DefaultConfigFilePathRoot, DefaultConfigFileName);
            else if (string.IsNullOrEmpty(FilePath))
                FilePath = Path.Combine(DefaultConfigFilePath, DefaultConfigFileName);

            if (File.Exists(FilePath))
                Console.WriteLine($"Using Configuration file: {FilePath}");
            else
            {
                Console.WriteLine($"ERROR: Configuration file does not exist: {FilePath}");
                return;
            }

            if (DeserializeConfiguration())
                RetrieveBackups();
        }

        private bool DeserializeConfiguration()
        {
            try
            {
                XmlSerializer serialiser = new XmlSerializer(typeof(List<MobileDebugInfoSettings>));
                using (TextReader filestream = new StreamReader(FilePath))
                    RobotConnectionData = (List<MobileDebugInfoSettings>)serialiser.Deserialize(filestream);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
        private void RetrieveBackups()
        {
            //foreach (MobileDebugInfoSettings mbd in RobotConnectionData)
            //{
            //    Console.WriteLine($"Retrieving debug file from IP: {mbd.IPAddress}");
            //    if (MobileDebugInfoDownload.GetDebugFile(mbd.IPAddress, mbd.UserName, mbd.Password, mbd.DestinationPath))
            //    {
            //        Console.WriteLine($"Debug file saved to: {mbd.DestinationPath}");
            //    }
            //    else
            //    {
            //        Console.WriteLine($"ERROR: Downloading debug file from IP: {mbd.IPAddress}");
            //    }
            //}

        }

        public bool SerializeConfiguration()
        {
            try
            {
                XmlSerializer serialiser = new XmlSerializer(typeof(List<MobileDebugInfoSettings>));
                using (TextWriter filestream = new StreamWriter (FilePath))
                    serialiser.Serialize(filestream, RobotConnectionData);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
