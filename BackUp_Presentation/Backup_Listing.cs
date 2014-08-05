using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackUp_Presentation
{
    public partial class Backup_Listing : Form
    {
        List<Backup_Model.Backup> Backups;
        public Backup_Listing()
        {
            InitializeComponent();
            LoadBackups();
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            UpdateListing();
            FormClosing += Backup_Listing_FormClosing;
        }

        private void Backup_Listing_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Backup_Service.BackUp_Manager.Save_Backups(AppDomain.CurrentDomain.BaseDirectory, Backups);
   
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadBackups()
        {
            try
            {
                Backups = Backup_Service.BackUp_Manager.Load_Backups(AppDomain.CurrentDomain.BaseDirectory);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = listView1.SelectedItems.Count <= 0;
        }

        private void volumeExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var v = new BackUp.Volume_Explorer();
            v.Show();
        }

        private void createToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var c = new Create_Backup(Backups, UpdateListing);
            c.ShowDialog(this);

        }
        private void UpdateListing()
        {
            listView1.Items.Clear();
            foreach(var backup in Backups)
            {
                var item = new ListViewItem(backup.Name);
                var dt = backup.LastBackup == DateTime.MinValue ? "N/A" : backup.LastBackup.ToShortDateString() + " " + backup.LastBackup.ToShortTimeString();
                var subItems = new ListViewItem.ListViewSubItem[]
                {
                        new ListViewItem.ListViewSubItem(item, dt),
                };
                item.SubItems.AddRange(subItems);
                listView1.Items.Add(item);
            }
         }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new AboutBox1();
            a.Show();
        }

        private void Play_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                var name = listView1.SelectedItems[0].Text.ToLower();
                var found = Backups.FirstOrDefault(a => a.Name == name);
                if(found != null)
                {
                    found.Do_Backup();
                }
            }
        }

        private void getDifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                var name = listView1.SelectedItems[0].Text.ToLower();
                var found = Backups.FirstOrDefault(a => a.Name == name);
                if(found != null)
                {
                    var old = found.Vol.Refresh();
                    foreach(var item in found.Vol.Update(old))
                    {
                        Debug.WriteLine(item.Name);

                    }
                }
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                var name = listView1.SelectedItems[0].Text.ToLower();
                var found = Backups.FirstOrDefault(a => a.Name == name);
                if(found != null)
                {
                    found.Vol.Refresh();
                }
            }
        }

 

    }
}
