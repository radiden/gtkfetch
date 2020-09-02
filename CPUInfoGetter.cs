using System;

namespace gtkfetch
{
    class CPUInfoGetter
    {
        public struct CpuInfo 
        {
            public string vendor;
            public string model;
            public string speed;
        }
        public static CpuInfo CPU;
        static string modelexpr = @"model\sname\s*:\s*(.*)";
        static string vendorexpr = @"vendor_id\s*:\s*(.*)";
        public static void GetCPUInfo()
        {   
            CPU.model = FileReader.ReadFileAndFindGroup("/proc/cpuinfo", modelexpr, 1);
            CPU.vendor = FileReader.ReadFileAndFindGroup("/proc/cpuinfo", vendorexpr, 1);
            CPU.speed = $"{Math.Round(decimal.Parse(FileReader.ReadLine("/sys/devices/system/cpu/cpu0/cpufreq/cpuinfo_max_freq"))/1000000, 2)} GHz";
        }
    }
}