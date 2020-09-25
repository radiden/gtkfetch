//i already know this is gonna be hell
using System;
using System.IO;
using System.Collections;
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
        public static int GPUCount = 0;
        public static ArrayList GPUs = new ArrayList();
        public static string GetGPUInfo()
        {
            //definition of a hack FIXME
            GetGPUInstances();
            foreach(GPU arrayobject in GPUs)
            {
                PCISubSystem card = PCIIdentificationDatabase.GetSubSystem(arrayobject.VendorID, arrayobject.DeviceID, arrayobject.SubsystemVendorID, arrayobject.SubsystemDeviceID);
                return card.SubSystemName;
            }
            return null;
        }
        static void GetGPUInstances()
        {
            var devices = Directory.EnumerateDirectories("/sys/bus/pci/devices/");
            foreach(string directory in devices)
            {
                if(File.Exists($"{directory}/boot_vga"))
                {
                    GPUCount++;
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