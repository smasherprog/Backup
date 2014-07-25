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
    public class NTFSVolume : Journal.Interfaces.IVolume
    {
        private readonly int BUF_LEN = 8192 + 8;//8 bytes for the leading USN

        private DriveInfo _DriveInfo;
        public System.IO.DriveInfo Drive { get { return _DriveInfo; } }

        private SafeFileHandle _Root_Handle;
        private NTFS_Root _Volume_Structure;
        public Journal.Volume.IFile Root { get { return _Volume_Structure; } }

        private Win32Api.USN_JOURNAL_DATA _Current_JournalState;
        //private Win32Api.USN_JOURNAL_DATA _Last_JournalState;

        public NTFSVolume(DriveInfo rootpath, SafeFileHandle root)
        {
            _DriveInfo = rootpath;
            _Root_Handle = root;
            _Volume_Structure = new NTFS_Root(rootpath.Name);
          
        }
        public void Map_Volume()
        {
            DateTime startTime = DateTime.Now;

            QueryUsnJournal(ref _Current_JournalState);//need to query the jounral to get the first usn number

            Win32Api.MFT_ENUM_DATA med;
            med.StartFileReferenceNumber = 0;
            med.LowUsn = 0;
            med.HighUsn = _Current_JournalState.NextUsn;

            using(var med_struct = new StructWrapper(med))
            using(var rawdata = new Raw_Array_Wrapper(BUF_LEN))
            {
                uint outBytesReturned = 0;

                while(Win32Api.DeviceIoControl(
                    _Root_Handle.DangerousGetHandle(),
                    Win32Api.FSCTL_ENUM_USN_DATA,
                    med_struct.Ptr,
                    med_struct.Size,
                    rawdata.Ptr,
                    rawdata.Size,
                    out outBytesReturned,
                    IntPtr.Zero))
                {
                    outBytesReturned = outBytesReturned - sizeof(Int64);
                    IntPtr pUsnRecord = System.IntPtr.Add(rawdata.Ptr, sizeof(Int64));//need to skip 8 bytes because the first 8 bytes are to a usn number, which isnt in the structure
                    while(outBytesReturned > 60)
                    {
                        var usnEntry = new Win32Api.UsnEntry(pUsnRecord);
                        pUsnRecord = System.IntPtr.Add(pUsnRecord, (int)usnEntry.RecordLength);
                        _Volume_Structure.Add(usnEntry);
                        if(usnEntry.RecordLength > outBytesReturned)
                            outBytesReturned = 0;// prevent overflow
                        else
                            outBytesReturned -= usnEntry.RecordLength;
                    }
                    Marshal.WriteInt64(med_struct.Ptr, Marshal.ReadInt64(rawdata.Ptr, 0));//read the usn that we skipped and place it into the nextusn
                }
                var possiblerror = Marshal.GetLastWin32Error();
                if(possiblerror < 0)
                    throw new Win32Exception(possiblerror);
            }

            _Volume_Structure.Build();
        }
        private void QueryUsnJournal(ref Win32Api.USN_JOURNAL_DATA usnJournalState)
        {
            int sizeUsnJournalState = Marshal.SizeOf(usnJournalState);
            UInt32 cb;
            if(!Win32Api.DeviceIoControl(
                   _Root_Handle.DangerousGetHandle(),
                  Win32Api.FSCTL_QUERY_USN_JOURNAL,
                  IntPtr.Zero,
                  0,
                  out usnJournalState,
                  sizeUsnJournalState,
                  out cb,
                  IntPtr.Zero))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());

            }

        }

        public void GetUsnJournalEntries(Win32Api.USN_JOURNAL_DATA previousUsnState,
            UInt32 reasonMask,
            out List<Win32Api.UsnEntry> usnEntries,
            out Win32Api.USN_JOURNAL_DATA newUsnState)
        {

            usnEntries = new List<Win32Api.UsnEntry>();
            newUsnState = new Win32Api.USN_JOURNAL_DATA();

            QueryUsnJournal(ref newUsnState);

            Win32Api.READ_USN_JOURNAL_DATA rujd = new Win32Api.READ_USN_JOURNAL_DATA();
            rujd.StartUsn = previousUsnState.NextUsn;
            rujd.ReasonMask = reasonMask;
            rujd.ReturnOnlyOnClose = 0;
            rujd.Timeout = 0;
            rujd.bytesToWaitFor = 0;
            rujd.UsnJournalId = previousUsnState.UsnJournalID;

            using(var med_struct = new StructWrapper(rujd))
            using(var rawdata = new Raw_Array_Wrapper(BUF_LEN))
            {
                uint outBytesReturned = 0;
                var nextusn = previousUsnState.NextUsn;
                while(nextusn < newUsnState.NextUsn && Win32Api.DeviceIoControl(
                         _Root_Handle.DangerousGetHandle(),
                        Win32Api.FSCTL_READ_USN_JOURNAL,
                        med_struct.Ptr,
                        med_struct.Size,
                        rawdata.Ptr,
                        rawdata.Size,
                        out outBytesReturned,
                        IntPtr.Zero))
                {
                    outBytesReturned = outBytesReturned - sizeof(Int64);
                    IntPtr pUsnRecord = System.IntPtr.Add(rawdata.Ptr, sizeof(Int64));//point safe arithmetic!~!!
                    while(outBytesReturned > 60)   // while there are at least one entry in the usn journal
                    {
                        var usnEntry = new Win32Api.UsnEntry(pUsnRecord);
                        if(usnEntry.USN > newUsnState.NextUsn)
                            break;
                        usnEntries.Add(usnEntry);
                        pUsnRecord = System.IntPtr.Add(pUsnRecord, (int)usnEntry.RecordLength);//point safe arithmetic!~!!   
                        outBytesReturned -= usnEntry.RecordLength;
                    }
                    nextusn = Marshal.ReadInt64(rawdata.Ptr, 0);
                    Marshal.WriteInt64(med_struct.Ptr, nextusn);//read the usn that we skipped and place it into the nextusn
                }
            }

        }

        public void Dispose()
        {
            _Root_Handle.Dispose();
        }

        public void Begin()
        {
            QueryUsnJournal(ref _Current_JournalState);
            Debug.WriteLine("Got Begin");
        }
        public void End()
        {
            uint reasonMask = Win32Api.USN_REASON_DATA_OVERWRITE |
                   Win32Api.USN_REASON_DATA_EXTEND |
                   Win32Api.USN_REASON_NAMED_DATA_OVERWRITE |
                   Win32Api.USN_REASON_NAMED_DATA_TRUNCATION |
                   Win32Api.USN_REASON_FILE_CREATE |
                   Win32Api.USN_REASON_FILE_DELETE |
                   Win32Api.USN_REASON_EA_CHANGE |
                   Win32Api.USN_REASON_SECURITY_CHANGE |
                   Win32Api.USN_REASON_RENAME_OLD_NAME |
                   Win32Api.USN_REASON_RENAME_NEW_NAME |
                   Win32Api.USN_REASON_INDEXABLE_CHANGE |
                   Win32Api.USN_REASON_BASIC_INFO_CHANGE |
                   Win32Api.USN_REASON_HARD_LINK_CHANGE |
                   Win32Api.USN_REASON_COMPRESSION_CHANGE |
                   Win32Api.USN_REASON_ENCRYPTION_CHANGE |
                   Win32Api.USN_REASON_OBJECT_ID_CHANGE |
                   Win32Api.USN_REASON_REPARSE_POINT_CHANGE |
                   Win32Api.USN_REASON_STREAM_CHANGE |
                   Win32Api.USN_REASON_CLOSE;
            var changes = new List<Win32Api.UsnEntry>();
            Win32Api.USN_JOURNAL_DATA newUsnState;
            GetUsnJournalEntries(_Current_JournalState, reasonMask, out changes, out newUsnState);
            StringBuilder st = new StringBuilder();

            foreach(var item in changes)
            {
                st.Append("\nName: " + item.Name);
                AddReasonData(st, item);
            }
            Debug.WriteLine("Changes " + changes.Count.ToString() + "  " + st.ToString());
        }

        private void AddReasonData(StringBuilder sb, Win32Api.UsnEntry usnEntry)
        {
            sb.AppendFormat("\n  Reason Codes:");
            uint value = usnEntry.Reason & Win32Api.USN_REASON_DATA_OVERWRITE;
            if(0 != value)
            {
                sb.AppendFormat("\n     -DATA OVERWRITE");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_DATA_EXTEND;
            if(0 != value)
            {
                sb.AppendFormat("\n     -DATA EXTEND");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_DATA_TRUNCATION;
            if(0 != value)
            {
                sb.AppendFormat("\n     -DATA TRUNCATION");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_NAMED_DATA_OVERWRITE;
            if(0 != value)
            {
                sb.AppendFormat("\n     -NAMED DATA OVERWRITE");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_NAMED_DATA_EXTEND;
            if(0 != value)
            {
                sb.AppendFormat("\n     -NAMED DATA EXTEND");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_NAMED_DATA_TRUNCATION;
            if(0 != value)
            {
                sb.AppendFormat("\n     -NAMED DATA TRUNCATION");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_FILE_CREATE;
            if(0 != value)
            {
                sb.AppendFormat("\n     -FILE CREATE");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_FILE_DELETE;
            if(0 != value)
            {
                sb.AppendFormat("\n     -FILE DELETE");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_EA_CHANGE;
            if(0 != value)
            {
                sb.AppendFormat("\n     -EA CHANGE");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_SECURITY_CHANGE;
            if(0 != value)
            {
                sb.AppendFormat("\n     -SECURITY CHANGE");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_RENAME_OLD_NAME;
            if(0 != value)
            {
                sb.AppendFormat("\n     -RENAME OLD NAME");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_RENAME_NEW_NAME;
            if(0 != value)
            {
                sb.AppendFormat("\n     -RENAME NEW NAME");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_INDEXABLE_CHANGE;
            if(0 != value)
            {
                sb.AppendFormat("\n     -INDEXABLE CHANGE");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_BASIC_INFO_CHANGE;
            if(0 != value)
            {
                sb.AppendFormat("\n     -BASIC INFO CHANGE");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_HARD_LINK_CHANGE;
            if(0 != value)
            {
                sb.AppendFormat("\n     -HARD LINK CHANGE");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_COMPRESSION_CHANGE;
            if(0 != value)
            {
                sb.AppendFormat("\n     -COMPRESSION CHANGE");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_ENCRYPTION_CHANGE;
            if(0 != value)
            {
                sb.AppendFormat("\n     -ENCRYPTION CHANGE");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_OBJECT_ID_CHANGE;
            if(0 != value)
            {
                sb.AppendFormat("\n     -OBJECT ID CHANGE");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_REPARSE_POINT_CHANGE;
            if(0 != value)
            {
                sb.AppendFormat("\n     -REPARSE POINT CHANGE");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_STREAM_CHANGE;
            if(0 != value)
            {
                sb.AppendFormat("\n     -STREAM CHANGE");
            }
            value = usnEntry.Reason & Win32Api.USN_REASON_CLOSE;
            if(0 != value)
            {
                sb.AppendFormat("\n     -CLOSE");
            }
        }
    }
}
