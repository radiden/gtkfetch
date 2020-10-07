using System.Collections.Generic;
using System.IO;
using System;

namespace gtkfetch
{
    class Drive
    {
        public DriveInfo driveInfo;
        public string driveName;
        public string sizeUnit;
        public Decimal totalSize;
        public Decimal usePercent;
        /// <summary> Constructor that creates a Drive instance which contains info about a Drive </summary>
        public Drive(string drivename)
        {
            DriveInfo drive = new DriveInfo(drivename);
            driveInfo = drive;
            driveName = drivename;
            // is the drive over 1024GiB? if so, totalsize is going to be displayed as TiB; 1024^4 = 1099511627776
            if (drive.TotalSize >= 1099511627776)
            {
                totalSize = drive.TotalSize/1099511627776;
                sizeUnit = "TiB";
            }
            // is the drive over 1GiB? if so, totalsize is going to be displayed as GiB; 1024^3 = 1073741824
            else if (drive.TotalSize >= 1073741824)
            {
                totalSize = drive.TotalSize/1073741824;
                sizeUnit = "GiB";
            }
            // otherwise, display the drive in MiB; 1024^2 = 1048576
            else
            {
                totalSize = drive.TotalSize/1048576;
                sizeUnit = "MiB";
            }
            usePercent = Math.Round((decimal)(drive.TotalSize - drive.TotalFreeSpace) / drive.TotalSize * 100, 1);
        }
    }
    class FSInfoGetter
        {
            static string driveexpr = @"^(?!#)[a-zA-Z0-9\/=\-\""]*\s*(?!none)([\/a-zA-Z]*)";
            static List<string> drivenames = new List<string>(FileReader.ReadFileMatchMultiple("/etc/fstab", driveexpr, 1));
            /// <summary> Creates labels containing drive info and adds them to array </summary>
            static public void GetDrives()
            {
                foreach (string d in drivenames)
                {
                    Drive drive = new Drive(d);
                    if (drive.driveInfo.IsReady && drive.driveInfo.DriveType != DriveType.CDRom)
                    {
                        InfoLabel driveLabel = new InfoLabel(drive.driveName, "drive-harddisk");
                        Labels.labels.Add(driveLabel);
                        driveLabel.contentLabel.Text = $"total: {drive.totalSize}{drive.sizeUnit}, {drive.usePercent}% used";
                    }
                }
            }
        }
    }