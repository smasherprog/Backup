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


        private DriveInfo _DriveInfo;
        public System.IO.DriveInfo Drive { get { return _DriveInfo; } }

        private SafeFileHandle _Root_Handle;
        private NTFS_Root _Volume_Structure;
        public Journal.Volume.IFile Root { get { return _Volume_Structure; } }

        private Win32Api.USN_JOURNAL_DATA _Current_JournalState;
        public List<Journal.Volume.IFile> Changes { get { return GetChanges(); } }
        public NTFSVolume(DriveInfo rootpath)
        {
            _DriveInfo = rootpath;
            _Root_Handle = NTFS_Volume.NTFS_Functions.GetRootHandle(rootpath);
            _Current_JournalState = new Win32Api.USN_JOURNAL_DATA();
            _Volume_Structure = new NTFS_Root(rootpath.Name);
            NTFS_Volume.NTFS_Functions.QueryUsnJournal(_Root_Handle, ref _Current_JournalState);//need to query the jounral to get the first usn number
        }
        public void Map_Volume()
        {
            if(NTFS_Volume.NTFS_Functions.Build_Volume_Mapping(_Root_Handle, _Current_JournalState, _Volume_Structure.Add))
            {
                _Volume_Structure.Build();
            }//else something went wrong and the build mapping will throw!
        }


        private List<Journal.Volume.IFile> GetChanges()
        {

            var files = new List<Volume.IFile>();
            var changes = NTFS_Volume.NTFS_Functions.Get_Changes(_Root_Handle, _Current_JournalState);

            StringBuilder st = new StringBuilder();

            foreach(var item in changes)
            {
                st.Append("\nName: " + item.Name);
                AddReasonData(st, item);
            }
            Debug.WriteLine("Changes " + changes.Count.ToString() + "  " + st.ToString());



            return files;


        }

        private static void AddReasonData(StringBuilder sb, Win32Api.UsnEntry usnEntry)
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
        public void Dispose()
        {
            _Root_Handle.Dispose();
        }

    }
}
