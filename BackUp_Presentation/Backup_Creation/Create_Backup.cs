using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackUp_Presentation
{
    public partial class Create_Backup : Form
    {
        List<Backup_Model.Backup> Backups;
        Action _CB;
        bool BackupName, Source, Destination;
        public Create_Backup(List<Backup_Model.Backup> b, Action onclose)
        {
            InitializeComponent();
            Backups = b;
            _CB = onclose;
            this.FormClosing += Create_Backup_FormClosing;
            textBox1.KeyUp += textBox1_KeyUp;
            BackupName = Source = Destination = false;
        }

        void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBox1.Text))
            {
                BackupName = false;
            } else if(textBox1.Text.Length > 2)
            {
                BackupName = true;
            } else
            {
                BackupName = false;
            }
            ContinueButtonShow();
        }

        void Create_Backup_FormClosing(object sender, FormClosingEventArgs e)
        {
            _CB();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var pos = textBox1.Text.ToLower().Trim();

            if(Backups.Any(a => a.Name.ToLower().Trim() == pos))
            {
                MessageBox.Show("A Backup with that name already exists!");
            } else
            {
                Backups.Add(new Backup_Model.Backup(pos, textBox2.Text, textBox3.Text));
                this.Close();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox2.Text = folderBrowserDialog1.SelectedPath;
            ValidateSource();
        }
        private void ValidateSource()
        {
            if(string.IsNullOrWhiteSpace(textBox2.Text))
            {
                Source = false;
            } else if(!System.IO.Directory.Exists(textBox2.Text))
            {
                label3.Text = "The source path does not exist!";
                MessageBox.Show(label3.Text);
                Source = false;
                textBox2.Text = "";
            } else if(!Backup_Model.Backup.ValidatePath(textBox2.Text))
            {
                label3.Text = "That is not a valid ntfs drive, try again!";
                MessageBox.Show(label3.Text);
                Source = false;
                textBox2.Text = "";
            } else
            {
                if(!string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    //check to ensure the paths are not inside of one another
                    var temp = textBox2.Text.ToLower().Contains(textBox3.Text.ToLower()) || textBox3.Text.ToLower().Contains(textBox2.Text.ToLower());
                    if(temp)
                    {
                        label3.Text = "The source path cannot be inside of the backup destination location!";
                        MessageBox.Show(label3.Text);
                        Source = false;
                    } else
                    {
                        Source = true;
                    }
                } else
                {
                    Source = true;
                }
                Source = true;
            }
            ContinueButtonShow();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox3.Text = folderBrowserDialog1.SelectedPath;
            ValidateDestination();
        }
        private void ValidateDestination()
        {
            if(string.IsNullOrWhiteSpace(textBox3.Text))
                Destination = false;
            else if(!System.IO.Directory.Exists(textBox3.Text))
            {
                label3.Text = "The destination path does not exist!";
                MessageBox.Show(label3.Text);
                textBox3.Text = "";
                Destination = false;
            } else
            {
                if(!string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    //check to ensure the paths are not inside of one another
                    var temp = textBox2.Text.ToLower().Contains(textBox3.Text.ToLower()) || textBox3.Text.ToLower().Contains(textBox2.Text.ToLower());
                    if(temp)
                    {
                        label3.Text = "The destination path cannot be inside of the backup source location!";
                        MessageBox.Show(label3.Text);
                        Destination = false;
                    } else
                    {
                        Destination = true;
                    }
                } else
                {
                    Destination = true;
                }

            }
            ContinueButtonShow();
        }
        private void ContinueButtonShow()
        {
            button1.Enabled = Destination && Source && BackupName;
        }
    }
}
