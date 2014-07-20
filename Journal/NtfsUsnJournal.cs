using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Journal
{
    public class NtfsUsnJournal : IDisposable
    {
        public string VolumeName { get { return _DriveInfo.Name; } }

        public long AvailableFreeSpace { get { return _DriveInfo.AvailableFreeSpace; } }

        public long TotalFreeSpace { get { return _DriveInfo.TotalFreeSpace; } }

        public string Format { get { return _DriveInfo.DriveFormat; } }

        public DirectoryInfo RootDirectory { get { return _DriveInfo.RootDirectory; } }

        public long TotalSize { get { return _DriveInfo.TotalSize; } }

        public string VolumeLabel { get { return _DriveInfo.VolumeLabel; } }

        public uint VolumeSerialNumber { get { return _VolumeSerialNumber; } }

        private uint _VolumeSerialNumber;
        private DriveInfo _DriveInfo;
        private Raw_File_Handle _Root_Handle;
        public Volume_Structure _Volume_Structure;
        public NtfsUsnJournal(DriveInfo rootpath, Raw_File_Handle root, uint serial)
        {
            _DriveInfo = rootpath;
            _Root_Handle = root;
            _VolumeSerialNumber = serial;
            _Volume_Structure = new Volume_Structure(rootpath.Name);
        }
        public void BuildDriveMapping()
        {
            DateTime startTime = DateTime.Now;
  
            Win32Api.USN_JOURNAL_DATA usnState = new Win32Api.USN_JOURNAL_DATA();
            var usnRtnCode = QueryUsnJournal(ref usnState);//need to query the jounral to get the first usn number

            if(usnRtnCode == Journal.Win32Api.UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
            {

                Win32Api.MFT_ENUM_DATA med;
                med.StartFileReferenceNumber = 0;
                med.LowUsn = 0;
                med.HighUsn = usnState.NextUsn;
                using(var med_struct = new StructWrapper(med))
                using(var rawdata = new Raw_Array_Wrapper(sizeof(UInt64) + 10000))
                {
                    uint outBytesReturned = 0;
                    Win32Api.UsnEntry usnEntry = null;
                    while(Win32Api.DeviceIoControl(
                        _Root_Handle.Handle,
                        Win32Api.FSCTL_ENUM_USN_DATA,
                        med_struct.Ptr,
                        med_struct.Size,
                        rawdata.Ptr,
                        rawdata.Size,
                        out outBytesReturned,
                        IntPtr.Zero))
                    {
                        IntPtr pUsnRecord = System.IntPtr.Add(rawdata.Ptr, sizeof(Int64));//need to skip 8 bytes because the first 8 bytes are to a usn number, which isnt in the structure
                        while(outBytesReturned > 60)
                        {
                            usnEntry = new Win32Api.UsnEntry(pUsnRecord);
                            pUsnRecord = System.IntPtr.Add(pUsnRecord, (int)usnEntry.RecordLength);
                            _Volume_Structure.Add(usnEntry);
                            outBytesReturned -= usnEntry.RecordLength;
                        }
                        Marshal.WriteInt64(med_struct.Ptr, Marshal.ReadInt64(rawdata.Ptr, 0));//read the usn that we skipped and place it into the nextusn
                    }

                    usnRtnCode = ConvertWin32ErrorToUsnError((Win32Api.GetLastErrorEnum)Marshal.GetLastWin32Error());
                    if(usnRtnCode == Journal.Win32Api.UsnJournalReturnCode.ERROR_HANDLE_EOF) usnRtnCode = Journal.Win32Api.UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
                }
            }
            _Volume_Structure.Build();
        }
        private Journal.Win32Api.UsnJournalReturnCode QueryUsnJournal(ref Win32Api.USN_JOURNAL_DATA usnJournalState)
        {
            var usnReturnCode = Journal.Win32Api.UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
            int sizeUsnJournalState = Marshal.SizeOf(usnJournalState);
            UInt32 cb;
            if(!Win32Api.DeviceIoControl(
                  _Root_Handle.Handle,
                  Win32Api.FSCTL_QUERY_USN_JOURNAL,
                  IntPtr.Zero,
                  0,
                  out usnJournalState,
                  sizeUsnJournalState,
                  out cb,
                  IntPtr.Zero))
            {
                usnReturnCode = ConvertWin32ErrorToUsnError((Win32Api.GetLastErrorEnum)Marshal.GetLastWin32Error());
            }


            return usnReturnCode;
        }
        private Journal.Win32Api.UsnJournalReturnCode ConvertWin32ErrorToUsnError(Win32Api.GetLastErrorEnum Win32LastError)
        {
            Journal.Win32Api.UsnJournalReturnCode usnRtnCode;

            switch(Win32LastError)
            {
                case Win32Api.GetLastErrorEnum.ERROR_JOURNAL_NOT_ACTIVE:
                    usnRtnCode = Journal.Win32Api.UsnJournalReturnCode.USN_JOURNAL_NOT_ACTIVE;
                    break;
                case Win32Api.GetLastErrorEnum.ERROR_SUCCESS:
                    usnRtnCode = Journal.Win32Api.UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
                    break;
                case Win32Api.GetLastErrorEnum.ERROR_HANDLE_EOF:
                    usnRtnCode = Journal.Win32Api.UsnJournalReturnCode.ERROR_HANDLE_EOF;
                    break;
                default:
                    usnRtnCode = Journal.Win32Api.UsnJournalReturnCode.USN_JOURNAL_ERROR;
                    break;
            }

            return usnRtnCode;
        }
        public void Dispose()
        {
            _Root_Handle.Dispose();
        }
    }
}
