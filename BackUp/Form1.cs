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

namespace BackUp
{
    public partial class Form1 : Form
    {
        Journal.NtfsUsnJournal NtfsJournal;
        public Form1()
        {
            InitializeComponent();
        }

        private void selectDriveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var s = new Select_Drive(Selected_Drive);
            s.ShowDialog(this);
        }
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                NtfsJournal.BuildDriveMapping();
                Debug.WriteLine("Drive mapping built");

            } catch(Exception ee)
            {
                Debug.WriteLine(ee.Message);
            }
            TreeNode rootNode = new TreeNode(NtfsJournal.RootDirectory.Name);
            rootNode.Tag = NtfsJournal._Volume_Structure;
            GetDirs(NtfsJournal._Volume_Structure.Children, rootNode);
            treeView1.Nodes.Add(rootNode);

            var s = new Select_Drive(Selected_Drive);
            s.ShowDialog(this);
        }
        private void Selected_Drive(string path)
        {
            try
            {
                NtfsJournal = Journal.Journal_Factory.Create(path);
            } catch(Exception ee)
            {
                Debug.WriteLine(ee.Message);
            }
        }

        private void GetDirs(List<Journal.Volume.IFile> files, TreeNode n)
        {
            TreeNode aNode;
            foreach(var subDir in files.Where(a => a.IsFolder()))
            {
                aNode = new TreeNode(subDir.Name(), 0, 0);
                aNode.Tag = subDir;
                aNode.ImageKey = "folder";
                var subSubDirs = subDir.Children;
                if(subSubDirs.Any())
                    GetDirs(subSubDirs, aNode);

                n.Nodes.Add(aNode);
            }
        }
        void treeView1_NodeMouseClick(object sender,
            TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node;
            listView1.Items.Clear();
            var nodeDirInfo = (Journal.Volume.IFile)newSelected.Tag;
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;

            foreach(var dir in nodeDirInfo.Children.Where(a=>a.IsFolder()))
            {
                item = new ListViewItem(dir.Name(), 0);
                subItems = new ListViewItem.ListViewSubItem[]
                {
                        new ListViewItem.ListViewSubItem(item, dir.FolderCount().ToString()),
                        new ListViewItem.ListViewSubItem(item, dir.FileCount().ToString()),
                        new ListViewItem.ListViewSubItem(item, "Directory")
                };
                item.SubItems.AddRange(subItems);
                listView1.Items.Add(item);
            }
            foreach(var file in nodeDirInfo.Children.Where(a => a.IsFile()))
            {
                item = new ListViewItem(file.Name(), 1);
                subItems = new ListViewItem.ListViewSubItem[]
                {
                        new ListViewItem.ListViewSubItem(item, file.FolderCount().ToString()),
                        new ListViewItem.ListViewSubItem(item, file.FileCount().ToString()),
                        new ListViewItem.ListViewSubItem(item, "File")
                };
                item.SubItems.AddRange(subItems);
                listView1.Items.Add(item);
            }

            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void begToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(NtfsJournal == null)
                MessageBox.Show("You have not selected a drive yet.");
            else 
                NtfsJournal.Begin();
        }

        private void endToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(NtfsJournal == null)
                MessageBox.Show("You have not selected a drive yet.");
            else 
                NtfsJournal.End();
        }



    }
}
