using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace gtkfetch
{
    public struct CpuInfo 
    {
        public string vendor;
        public string model;
        #nullable enable
        public string? speed;
        public string? count;
        #nullable disable
        public int threadCount;
    }
    class CPUInfoGetter
    {
        public static CpuInfo CPU;
        /// <summary> Gets CPU Information and populates CPU instance </summary>
        public static void GetCPUInfo()
        {
            string modelexpr = @"model\sname\s*:\s*(.*)";
            string vendorexpr = @"vendor_id\s*:\s*(.*)";
            string idexpr = @"physical id\s*:\s*(\d+)";
            CPU.model = FileReader.ReadFileAndFindGroup("/proc/cpuinfo", modelexpr, 1);
            CPU.vendor = FileReader.ReadFileAndFindGroup("/proc/cpuinfo", vendorexpr, 1);
            // workaround for VMs - doesn't seem to exist in KVM-based VMs
            try 
            {
                CPU.speed = $"{Math.Round(decimal.Parse(FileReader.ReadLine("/sys/devices/system/cpu/cpu0/cpufreq/cpuinfo_max_freq"))/1000000, 2)} GHz";
            }
            catch 
            {
                CPU.speed = null;
                Console.WriteLine("could not get cpu speed!");
            }
            var cpus = Directory.EnumerateDirectories("/sys/devices/system/cpu");
            foreach(string s in cpus)
            {
                if(Regex.IsMatch(s, @"cpu\d+"))
                {
                    CPU.threadCount++;
                }
            }

            List<String> idarr = FileReader.ReadFileMatchMultiple("/proc/cpuinfo", idexpr, 1);
            Int32 CPUCountInt = Int32.Parse(idarr[idarr.Count-1]) + 1; 
            switch(CPUCountInt)
            {
                case 1:
                    CPU.count = null;
                    break;
                default:
                    CPU.count = $"{CPUCountInt}x ";
                    break;
            }
        }
    }
}