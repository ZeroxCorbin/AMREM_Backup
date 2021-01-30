using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AMREM_Backup.Classes
{
    public class PingClass
    {
        public static async Task<bool> Ping(string host)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions(64, true);

            // Create a buffer of 32 bytes of data to be transmitted.
            // string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = new byte[128];
            int timeout = 15000;


            Task<PingReply> t = pingSender.SendPingAsync("google.com", timeout, buffer, options);
            t.Wait();
            PingReply reply = t.Result;

            //Process P = new Process();
            //P.StartInfo.FileName = "ping";
            //P.StartInfo.Arguments = "-c 2 192.168.4.20"; // Take 3 samples to 8.8.8.8
            //P.StartInfo.UseShellExecute = false;
            //P.StartInfo.RedirectStandardOutput = true;

            //string readData = "";
            //if (P.Start())
            //    readData = P.StandardOutput.ReadToEnd(); // This will also wait for the process to at least close its stdout

            if (reply.Status == IPStatus.Success)
                return true;
            else
                return false;
        }
    }
}
