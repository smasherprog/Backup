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
            DriveInfo driveInfo = null;
            foreach(var item in DriveInfo.GetDrives())
            {
                if(item.Name == drive)
                {
                    driveInfo = item;
                    break;
                }
            }
            if(driveInfo == null)
                throw new Exception("The drive " + drive + " is not a valid drive.");


            if(driveInfo.DriveFormat.ToLower().Contains("ntfs"))
            {
                return new NTFSVolume(driveInfo, GetRootHandle(driveInfo));
            } else
                return new Fat32Volume(driveInfo);

        }
        private static SafeFileHandle GetRootHandle(DriveInfo driveInfo)
        {
            string vol = string.Concat("\\\\.\\", driveInfo.Name.TrimEnd('\\'));
            var handle = Win32Api.CreateFile(vol,
                 Win32Api.GENERIC_READ | Win32Api.GENERIC_WRITE,
                 Win32Api.FILE_SHARE_READ | Win32Api.FILE_SHARE_WRITE,
                 IntPtr.Zero,
                 Win32Api.OPEN_EXISTING,
                 0,
                 IntPtr.Zero);
            if(handle.IsInvalid)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return handle;
        }

    }
}
