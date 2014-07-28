using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Journal
{
    public static class Journal_Factory
    {
        public static Journal.Interfaces.IVolume Create(string drive)
        {
            DriveInfo driveInfo = new DriveInfo(drive);
            var e = Environment.GetLogicalDrives();
            if(driveInfo.DriveFormat.ToLower().Contains("ntfs"))
            {
                return new NTFSVolume(driveInfo);
            } else
                return new Fat32Volume(driveInfo);

        }

    }
}
