using System;
using System.IO;
using System.Collections.Generic;
using PCIIdentificationResolver;

namespace gtkfetch
{
    class GPU
    {
        public ushort VendorID;
        public ushort DeviceID;
        public ushort SubsystemVendorID;
        public ushort SubsystemDeviceID;
        public GPU(ushort vid, ushort did, ushort svid, ushort sdid)
        {
            VendorID = vid;
            DeviceID = did;
            SubsystemVendorID = svid;
            SubsystemDeviceID = sdid;
        }
    }
    class GPUInfoGetter
    {
        public static List<GPU> GPUs = new List<GPU>();
        public static List<string> GetGPUInfo()
        {
            List<string> cards = new List<string>();
            foreach(GPU gpu in GPUs)
            {
                try
                {
                    PCISubSystem card = PCIIdentificationDatabase.GetSubSystem(gpu.VendorID, gpu.DeviceID, gpu.SubsystemVendorID, gpu.SubsystemDeviceID);
                    cards.Add(card.SubSystemName);
                }
                catch (System.NullReferenceException)
                {
                    try
                    {
                        PCIDevice cardalt = PCIIdentificationDatabase.GetDevice(gpu.VendorID, gpu.DeviceID);
                        cards.Add(cardalt.DeviceName.ToString());
                    }
                    catch
                    {
                        cards.Add("unknown");
                    }
                }
            }
            return cards;
        }
        public static void GetGPUInstances()
        {
            var devices = Directory.EnumerateDirectories("/sys/bus/pci/devices/");
            foreach(string directory in devices)
            {
                if(File.Exists($"{directory}/boot_vga"))
                {
                    ushort vid = Convert.ToUInt16(FileReader.ReadLine($"{directory}/vendor"), 16);
                    ushort did = Convert.ToUInt16(FileReader.ReadLine($"{directory}/device"), 16);
                    ushort svid = Convert.ToUInt16(FileReader.ReadLine($"{directory}/subsystem_vendor"), 16);
                    ushort sdid = Convert.ToUInt16(FileReader.ReadLine($"{directory}/subsystem_device"), 16);

                    GPUs.Add(new GPU(vid, did, svid, sdid));
                }
            }
        }
    }
}