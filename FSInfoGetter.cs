using System;
using System.Collections;

namespace gtkfetch
{
    class FSInfoGetter
    {
        static string driveexpr = @"^(?!#)[a-zA-Z0-9\/=\-\""]*\s*(?!none)([\/a-zA-Z]*)";
        static public void GetDrives()
        {
            ArrayList drives = new ArrayList(FileReader.ReadFileMatchMultiple("/etc/fstab", driveexpr, 1));
        }
    }
}