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
        public Create_Backup(List<Backup_Model.Backup> b, Action onclose)
        {
            InitializeComponent();
            Backups = b;
            _CB = onclose;
            this.FormClosing += Create_Backup_FormClosing;
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
            if(string.IsNullOrWhiteSpace(pos) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("You must select a volume and a name!");
            } else
            {
                if(Backups.Any(a => a.Name.ToLower().Trim() == pos))
                {
                    MessageBox.Show("A Backup with that name already exists!");
                } else
                {
                    Backups.Add(new Backup_Model.Backup(pos, textBox2.Text));
                    this.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var s = new BackUp.Select_Drive((a) => { textBox2.Text = a; });
            s.ShowDialog(this);
        }
    }
}
