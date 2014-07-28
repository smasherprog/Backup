using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;

namespace BackUp_Presentation.Volume
{
    public partial class Volume_Mapper : Form
    {
        private Action<NetworkDrive> _CB;
        public Volume_Mapper(Action<NetworkDrive> cb)
        {
            InitializeComponent();
            _CB = cb;
        }

 
        private void button3_Click_1(object sender, System.EventArgs e)
        {
     
                var oNetDrive = new NetworkDrive();
                zUpdateStatus("Mapping drive...");
                try
                {
                    //set propertys
                    oNetDrive.Force = this.conForce.Checked;
                    oNetDrive.Persistent = this.conPersistant.Checked;
                    oNetDrive.LocalDrive = txtDrive.Text;
                    oNetDrive.PromptForCredentials = conPromptForCred.Checked;
                    oNetDrive.ShareName = txtAddress.Text;
                    oNetDrive.SaveCredentials = conSaveCred.Checked;
                    //match call to options provided
                    if(txtPassword.Text == "" && txtUsername.Text == "")
                    {
                        oNetDrive.MapDrive();
                    } else if(txtUsername.Text == "")
                    {
                        oNetDrive.MapDrive(txtPassword.Text);
                    } else
                    {
                        oNetDrive.MapDrive(txtUsername.Text, txtPassword.Text);
                    }
                    //update status
                    zUpdateStatus("Drive map successful");
                    _CB(oNetDrive);
                } catch(Exception err)
                {
                    //report error
                    zUpdateStatus("Cannot map drive! - " + err.Message);
                    MessageBox.Show(this, "Cannot map drive!\nError: " + err.Message);
                }
                oNetDrive = null;
            
        }

        private void frmTest_Load(object sender, System.EventArgs e)
        {
            //set the address field to a share name for the current computer
            txtAddress.Text = "\\\\" + SystemInformation.ComputerName + "\\C$";
        }

        private void zUpdateStatus(string psStatus)
        {
            //update the status bar and refresh
            this.conStatusBar.Panels[0].Text = psStatus;
            this.Refresh();
        }


        private void conMenu_Dialog1_Click(object sender, System.EventArgs e)
        {
            //show the connection dialog
            NetworkDrive oNetDrive = new NetworkDrive();
            oNetDrive.ShowConnectDialog(this);
            oNetDrive = null;
        }

        private void conMenu_Dialog2_Click(object sender, System.EventArgs e)
        {
            //show the disconnection dialog
            NetworkDrive oNetDrive = new NetworkDrive();
            oNetDrive.ShowDisconnectDialog(this);
            oNetDrive = null;
        }
    }
}
