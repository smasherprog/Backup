using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Journal
{
    public static class Journal_Factory
    {
        public static NtfsUsnJournal Create(string drive)
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
            uint _volumeSerialNumber = 0;
            Raw_File_Handle rootHandle = null;
            if(0 == string.Compare(driveInfo.DriveFormat, "ntfs", true))
            {
                var usnRtnCode = GetRootHandle(out rootHandle, driveInfo);
                if(usnRtnCode == Journal.Win32Api.UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
                {
                    usnRtnCode = GetVolumeSerialNumber(driveInfo, out _volumeSerialNumber);
                    if(usnRtnCode != Journal.Win32Api.UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
                        throw new Win32Exception((int)usnRtnCode);
                } else
                    throw new Win32Exception((int)usnRtnCode);
            } else
                throw new Exception(string.Format("{0} is not an 'NTFS' volume.", driveInfo.Name));
            return new NtfsUsnJournal(driveInfo, rootHandle, _volumeSerialNumber);
        }
        private static Journal.Win32Api.UsnJournalReturnCode GetRootHandle(out Raw_File_Handle rootHandle, DriveInfo driveInfo)
        {
            var usnRtnCode = Journal.Win32Api.UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
            string vol = string.Concat("\\\\.\\", driveInfo.Name.TrimEnd('\\'));
            var handle = Win32Api.CreateFile(vol,
                 Win32Api.GENERIC_READ | Win32Api.GENERIC_WRITE,
                 Win32Api.FILE_SHARE_READ | Win32Api.FILE_SHARE_WRITE,
                 IntPtr.Zero,
                 Win32Api.OPEN_EXISTING,
                 0,
                 IntPtr.Zero);

            if(handle.ToInt32() == Win32Api.INVALID_HANDLE_VALUE)
                usnRtnCode = (Journal.Win32Api.UsnJournalReturnCode)Marshal.GetLastWin32Error();
            rootHandle = new Raw_File_Handle(handle);
            return usnRtnCode;
        }

        /// <summary>
        /// Gets a Volume Serial Number for the volume represented by driveInfo.
        /// </summary>
        /// <param name="driveInfo">DriveInfo object representing the volume in question.</param>
        /// <param name="volumeSerialNumber">out parameter to hold the volume serial number.</param>
        /// <returns></returns>
        private static Journal.Win32Api.UsnJournalReturnCode GetVolumeSerialNumber(DriveInfo driveInfo, out uint volumeSerialNumber)
        {
            Console.WriteLine("GetVolumeSerialNumber() function entered for drive '{0}'", driveInfo.Name);
            volumeSerialNumber = 0;
            var usnRtnCode = Journal.Win32Api.UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
            string pathRoot = string.Concat("\\\\.\\", driveInfo.Name);
            using(var hRoot = new Raw_File_Handle(Win32Api.CreateFile(pathRoot,
                0,
                Win32Api.FILE_SHARE_READ | Win32Api.FILE_SHARE_WRITE,
                IntPtr.Zero,
                Win32Api.OPEN_EXISTING,
                Win32Api.FILE_FLAG_BACKUP_SEMANTICS,
                IntPtr.Zero)))
            {
                if(hRoot.Handle.ToInt32() != Win32Api.INVALID_HANDLE_VALUE)
                {
                    Win32Api.BY_HANDLE_FILE_INFORMATION fi = new Win32Api.BY_HANDLE_FILE_INFORMATION();
                    if(Win32Api.GetFileInformationByHandle(hRoot.Handle, out fi))
                        volumeSerialNumber = fi.VolumeSerialNumber;
                    else
                        usnRtnCode = (Journal.Win32Api.UsnJournalReturnCode)Marshal.GetLastWin32Error();
                } else
                    usnRtnCode = (Journal.Win32Api.UsnJournalReturnCode)Marshal.GetLastWin32Error();
            }
            return usnRtnCode;
        }

    }
}
