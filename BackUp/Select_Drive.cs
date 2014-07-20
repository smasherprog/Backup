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
            foreach (var item in DriveInfo.GetDrives())
            {
                listBox1.Items.Add(item.Name);
            }

            this.Height = listBox1.PreferredHeight+ this.PreferredSize.Height;
   
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cb(listBox1.SelectedItem.ToString());
            this.Close();
        }

    }
}
