using System;

namespace gtkfetch
{
    public struct CpuInfo 
    {
        public string vendor;
        public string model;
        #nullable enable
        public string? speed;
        #nullable disable
    }
    class CPUInfoGetter
    {
        public static CpuInfo CPU;
        static string modelexpr = @"model\sname\s*:\s*(.*)";
        static string vendorexpr = @"vendor_id\s*:\s*(.*)";
        /// <summary> Gets CPU Information and populates CPU instance </summary>
        public static void GetCPUInfo()
        {   
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
            if (CPU.speed != null)
            {
                Labels.cpuLabel = new InfoLabel("cpu", "cpu", $"{CPUInfoGetter.CPU.model} @ {CPUInfoGetter.CPU.speed}");
            }
            else
            {
                Labels.cpuLabel = new InfoLabel("cpu", "cpu", $"{CPUInfoGetter.CPU.model}");
            }
        }
    }
}