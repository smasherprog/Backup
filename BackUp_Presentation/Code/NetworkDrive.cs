using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utilities
{
    public class NetworkDrive
    {

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2A(ref structNetResource pstNetRes, string psPassword, string psUsername, int piFlags);
        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2A(string psName, int piFlags, int pfForce);
        [DllImport("mpr.dll")]
        private static extern int WNetConnectionDialog(int phWnd, int piType);
        [DllImport("mpr.dll")]
        private static extern int WNetDisconnectDialog(int phWnd, int piType);
        [DllImport("mpr.dll")]
        private static extern int WNetRestoreConnectionW(int phWnd, string psLocalDrive);

        [StructLayout(LayoutKind.Sequential)]
        private struct structNetResource
        {
            public int iScope;
            public int iType;
            public int iDisplayType;
            public int iUsage;
            public string sLocalName;
            public string sRemoteName;
            public string sComment;
            public string sProvider;
        }

        private const int RESOURCETYPE_DISK = 0x1;

        //Standard	
        private const int CONNECT_INTERACTIVE = 0x00000008;
        private const int CONNECT_PROMPT = 0x00000010;
        private const int CONNECT_UPDATE_PROFILE = 0x00000001;
        //IE4+
        private const int CONNECT_REDIRECT = 0x00000080;
        //NT5 only
        private const int CONNECT_COMMANDLINE = 0x00000800;
        private const int CONNECT_CMD_SAVECRED = 0x00001000;

   
        private bool lf_SaveCredentials = false;

        public bool SaveCredentials
        {
            get { return (lf_SaveCredentials); }
            set { lf_SaveCredentials = value; }
        }
        private bool lf_Persistent = false;

        public bool Persistent
        {
            get { return (lf_Persistent); }
            set { lf_Persistent = value; }
        }
        private bool lf_Force = false;

        public bool Force
        {
            get { return (lf_Force); }
            set { lf_Force = value; }
        }
        private bool ls_PromptForCredentials = false;

        public bool PromptForCredentials
        {
            get { return (ls_PromptForCredentials); }
            set { ls_PromptForCredentials = value; }
        }

        private string ls_Drive = "s:";

        public string LocalDrive
        {
            get { return (ls_Drive); }
            set
            {
                if(value.Length >= 1)
                {
                    ls_Drive = value.Substring(0, 1) + ":";
                } else
                {
                    ls_Drive = "";
                }
            }
        }
        private string ls_ShareName = "\\\\Computer\\C$";

        public string ShareName
        {
            get { return (ls_ShareName); }
            set { ls_ShareName = value; }
        }
        public string Login;
        public string Password;
        public void MapDrive() { zMapDrive(null, null); }
     
        public void MapDrive(string Password) { zMapDrive(null, Password); }
        public void MapDrive(string Username, string Password) { zMapDrive(Username, Password); }
        public void UnMapDrive() { zUnMapDrive(this.lf_Force); }
        public void ShowConnectDialog(Form ParentForm) { zDisplayDialog(ParentForm, 1); }
        public void ShowDisconnectDialog(Form ParentForm) { zDisplayDialog(ParentForm, 2); }



        // Map network drive
        private void zMapDrive(string psUsername, string psPassword)
        {
            //create struct data
            structNetResource stNetRes = new structNetResource();
            stNetRes.iScope = 2;
            stNetRes.iType = RESOURCETYPE_DISK;
            stNetRes.iDisplayType = 3;
            stNetRes.iUsage = 1;
            stNetRes.sRemoteName = ls_ShareName;
            stNetRes.sLocalName = ls_Drive;
            //prepare params
            int iFlags = 0;
            if(lf_SaveCredentials) { iFlags += CONNECT_CMD_SAVECRED; }
            if(lf_Persistent) { iFlags += CONNECT_UPDATE_PROFILE; }
            if(ls_PromptForCredentials) { iFlags += CONNECT_INTERACTIVE + CONNECT_PROMPT; }
            if(psUsername == "") { psUsername = null; }
            if(psPassword == "") { psPassword = null; }
            Login = psUsername;
            Password = psPassword;
            //if force, unmap ready for new connection
            if(lf_Force) { try { zUnMapDrive(true); } catch { } }
            //call and return
            int i = WNetAddConnection2A(ref stNetRes, psPassword, psUsername, iFlags);
            if(i > 0) { throw new System.ComponentModel.Win32Exception(i); }
        }


        // Unmap network drive	
        private void zUnMapDrive(bool pfForce)
        {
            //call unmap and return
            int iFlags = 0;
            if(lf_Persistent) { iFlags += CONNECT_UPDATE_PROFILE; }
            int i = WNetCancelConnection2A(ls_Drive, iFlags, Convert.ToInt32(pfForce));
            if(i != 0)
                i = WNetCancelConnection2A(ls_ShareName, iFlags, Convert.ToInt32(pfForce));  //disconnect if localname was null
            if(i > 0) { throw new System.ComponentModel.Win32Exception(i); }
        }


        // Display windows dialog
        private void zDisplayDialog(Form poParentForm, int piDialog)
        {
            int i = -1;
            int iHandle = 0;
            //get parent handle
            if(poParentForm != null)
            {
                iHandle = poParentForm.Handle.ToInt32();
            }
            //show dialog
            if(piDialog == 1)
            {
                i = WNetConnectionDialog(iHandle, RESOURCETYPE_DISK);
            } else if(piDialog == 2)
            {
                i = WNetDisconnectDialog(iHandle, RESOURCETYPE_DISK);
            }
            if(i > 0) { throw new System.ComponentModel.Win32Exception(i); }
            //set focus on parent form
            poParentForm.BringToFront();
        }


    }
}
