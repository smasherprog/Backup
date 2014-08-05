using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackUp
{
    public partial class Select_Drive : Form
    {
        private Action<string> Cb;
        public Select_Drive(Action<string> retfunc)
        {
            InitializeComponent();
            Cb = retfunc;

            foreach(var item in DriveInfo.GetDrives())
            {

                var lisit = new ListViewItem(item.Name, GetImageIndex(item));

                var driveformat = "";
                try
                {
                    driveformat = item.DriveFormat;

                } catch(Exception e)
                {
                    Debug.WriteLine(e.Message);
                    driveformat = "Unknown";
                }
                if(driveformat.ToLower().Contains("ntfs"))
                {
                    var subItems = new ListViewItem.ListViewSubItem[]
                {
                        new ListViewItem.ListViewSubItem(lisit, driveformat),
                        new ListViewItem.ListViewSubItem(lisit, item.DriveType.ToString()),
                        new ListViewItem.ListViewSubItem(lisit, item.IsReady.ToString()),
                };
                    lisit.SubItems.AddRange(subItems);
                    listView1.Items.Add(lisit);
                }
            }
            this.Height = listView1.PreferredSize.Height + this.PreferredSize.Height;

            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
        }
        private int GetImageIndex(DriveInfo d)
        {
            if(d.DriveType == DriveType.CDRom)
                return 1;
            else if(d.DriveType == DriveType.Network)
                return 3;
            else
            {
                if(d.Name.ToLower().Contains("c:"))
                    return 0;
                return 2;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                Cb(listView1.SelectedItems[0].Text);
                this.Close();
            }
        }

    }
}
