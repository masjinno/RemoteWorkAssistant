using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteWorkAssistant.Service.Model
{
    class IpAddressUtil
    {
        public static string getIpAddress()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "ipconfig";
            proc.StartInfo.Arguments = "/all";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            return proc.StandardOutput.ReadToEnd();
        }
    }
}
